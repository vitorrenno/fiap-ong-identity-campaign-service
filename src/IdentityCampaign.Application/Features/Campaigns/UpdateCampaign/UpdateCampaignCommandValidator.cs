using FluentValidation;

namespace IdentityCampaign.Application.Features.Campaigns.UpdateCampaign;

public sealed class UpdateCampaignCommandValidator : AbstractValidator<UpdateCampaignCommand>
{
    public UpdateCampaignCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("O ID da campanha é obrigatório.");

        RuleFor(command => command.Title)
            .NotEmpty().WithMessage("O título da campanha é obrigatório.")
            .Length(3, 200).WithMessage("O título da campanha deve ter entre 3 e 200 caracteres.");

        RuleFor(command => command.Description)
            .NotEmpty().WithMessage("A descrição da campanha é obrigatória.")
            .Length(10, 1000).WithMessage("A descrição da campanha deve ter entre 10 e 1000 caracteres.");

        RuleFor(command => command.GoalAmount)
            .GreaterThan(0).WithMessage("A meta financeira deve ser maior que zero.");

        RuleFor(command => command.EndDate)
            .GreaterThan(command => command.StartDate)
            .WithMessage("A data de término deve ser posterior à data de início.");
    }
}
