namespace PunchSync.Application.UseCases.Auth.Common;

public sealed record AuthResponse(
    Guid UserId,
    string Name,
    string Email,
    string Role,
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt);
