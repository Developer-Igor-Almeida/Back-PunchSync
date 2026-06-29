using System.Net;
using System.Text.Json;
using FluentValidation;

namespace PunchSync.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { errors }));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro não tratado");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = env.IsDevelopment()
                ? new { error = ex.Message, detail = ex.StackTrace }
                : (object)new { error = "Ocorreu um erro interno. Tente novamente mais tarde." };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
