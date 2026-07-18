using FluentValidation;

namespace PunchSync.Application.UseCases.Gyms.CreateGym;

public sealed class CreateGymCommandValidator : AbstractValidator<CreateGymCommand>
{
    public CreateGymCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Slug)
            .NotEmpty().MaximumLength(80)
            .Matches("^[a-z0-9-]+$").WithMessage("Slug deve conter apenas letras minúsculas, números e hífens.");
    }
}
