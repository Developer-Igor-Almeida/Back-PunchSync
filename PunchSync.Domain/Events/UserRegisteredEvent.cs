using PunchSync.Domain.Common;

namespace PunchSync.Domain.Events;

public sealed record UserRegisteredEvent(Guid UserId, string Email, string Name) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
