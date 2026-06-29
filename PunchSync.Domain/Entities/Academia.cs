using PunchSync.Domain.Common;
using PunchSync.Domain.Enums;

namespace PunchSync.Domain.Entities;

public class Academia : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public string? LogoUrl { get; private set; }
    public AcademiaPlan Plan { get; private set; }
    public bool IsActive { get; private set; }

    private Academia() { }

    public Academia(string name, string slug, AcademiaPlan plan = AcademiaPlan.Free)
    {
        Name = name;
        Slug = slug;
        Plan = plan;
        IsActive = true;
    }

    public void UpdatePlan(AcademiaPlan plan)
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
