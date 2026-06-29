using System.Security.Claims;
using PunchSync.Application.Interfaces;

namespace PunchSync.Api.Context;

public class HttpTenantContext(IHttpContextAccessor accessor) : ITenantContext
{
    private ClaimsPrincipal? User => accessor.HttpContext?.User;

    public Guid TenantId => Guid.TryParse(User?.FindFirstValue("tenant_id"), out var id) ? id : Guid.Empty;
    public Guid UserId => Guid.TryParse(User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? User?.FindFirstValue("sub"), out var id) ? id : Guid.Empty;
    public string UserRole => User?.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
}
