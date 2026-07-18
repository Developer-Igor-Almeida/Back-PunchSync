using PunchSync.Domain.Common;
using PunchSync.Domain.Enums;

namespace PunchSync.Domain.Entities;

public class Gym : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public string? LogoUrl { get; private set; }
    public GymPlan Plan { get; private set; }
    public bool IsActive { get; private set; }

    private Gym() { }

    public Gym(string name, string slug, GymPlan plan = GymPlan.Free)
    {
        Name = name;
        Slug = slug;
        Plan = plan;
        IsActive = true;
    }

    public void UpdatePlan(GymPlan plan)
    {
        Plan = plan;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }
}
