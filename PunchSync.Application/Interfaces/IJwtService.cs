namespace PunchSync.Application.Interfaces;

public sealed record AccessTokenResult(string Token, DateTime ExpiresAt);

public interface IJwtService
{
    AccessTokenResult GenerateAccessToken(Guid userId, string email, string role, Guid tenantId);
    string GenerateRefreshToken();
    DateTime GetRefreshTokenExpiry();
    Guid? GetUserIdFromToken(string token);
}
