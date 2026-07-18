using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PunchSync.Application.Behaviors;
using PunchSync.Application.Interfaces;
using PunchSync.Domain.Interfaces;
using PunchSync.Domain.Interfaces.Repositories;
using PunchSync.Infra.Auth;
using PunchSync.Infra.Cache;
using PunchSync.Infra.Data;
using PunchSync.Infra.Repositories;
using StackExchange.Redis;

namespace PunchSync.Ioc;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddCache(configuration);
        services.AddJwt(configuration);
        services.AddRepositories();
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGymRepository, GymRepository>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ICacheService).Assembly;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(applicationAssembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("Default")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    // Cache configurável por "Cache:Provider":
    //   "InMemory" (padrão)  -> roda sem dependências externas (dev/estudos)
    //   "Redis"              -> usa o Redis real (produção). Toda a infra do Redis
    //                           está pronta; basta mudar a flag e ter o Redis no ar.
    private static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        var provider = configuration["Cache:Provider"] ?? "InMemory";

        if (provider.Equals("Redis", StringComparison.OrdinalIgnoreCase))
        {
            var connectionString = configuration["Redis:ConnectionString"]
                ?? throw new InvalidOperationException("Redis:ConnectionString não configurado");

            // abortConnect=false: não derruba a aplicação se o Redis estiver indisponível no boot;
            // a conexão é tentada/reconectada em background.
            var options = ConfigurationOptions.Parse(connectionString);
            options.AbortOnConnectFail = false;

            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(options));
            services.AddScoped<ICacheService, RedisCacheService>();
        }
        else
        {
            services.AddMemoryCache();
            services.AddScoped<ICacheService, InMemoryCacheService>();
        }

        return services;
    }

    private static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var secret = configuration["Jwt:Secret"]
            ?? throw new InvalidOperationException("Jwt:Secret não configurado");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddScoped<IJwtService, JwtService>();
        return services;
    }
}
