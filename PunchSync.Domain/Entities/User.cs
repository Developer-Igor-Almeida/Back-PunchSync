using PunchSync.Domain.Common;
using PunchSync.Domain.Enums;
using PunchSync.Domain.Events;

namespace PunchSync.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public Guid TenantId { get; private set; }
    public bool IsActive { get; private set; }

    public Academia? Tenant { get; private set; }

    private readonly List<RefreshToken> _refreshTokens = [];
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private User() { }

    public User(string name, string email, string passwordHash, UserRole role, Guid tenantId)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        TenantId = tenantId;
        IsActive = true;

        RaiseDomainEvent(new UserRegisteredEvent(Id, email, name));
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }

    public RefreshToken AddRefreshToken(string token, DateTime expiresAt)
    {
        var refreshToken = new RefreshToken(Id, token, expiresAt);
        _refreshTokens.Add(refreshToken);
        return refreshToken;
    }
}
