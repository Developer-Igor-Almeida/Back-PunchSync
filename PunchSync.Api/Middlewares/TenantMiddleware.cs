using System.Security.Claims;
using PunchSync.Application.Interfaces;

namespace PunchSync.Api.Middlewares;

public class TenantMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext)
    {
        // TenantContext é populado pelo HttpTenantContext via claims do JWT
        await next(context);
    }
}
