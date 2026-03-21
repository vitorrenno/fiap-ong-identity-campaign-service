using FluentValidation;
using IdentityCampaign.Application.Features.Campaigns.CreateCampaign;

namespace IdentityCampaign.Application.Features.Campaigns.CreateCampaigns
{
    public class CreateCampaignCommandValidator : AbstractValidator<CreateCampaignCommand>
    {
        public CreateCampaignCommandValidator()
        {
            RuleFor(command => command.Title)
                .NotEmpty().WithMessage("O título da campanha é obrigatório.")
                .Length(3, 200).WithMessage("O título da campanha deve ter entre 3 e 200 caracteres.");

            RuleFor(command => command.Description)
                .NotEmpty().WithMessage("A descrição da campanha é obrigatória.")
                .Length(10, 1000).WithMessage("A descrição da campanha deve ter entre 10 e 1000 caracteres.");

            RuleFor(command => command.GoalAmount)
                .GreaterThan(0).WithMessage("A meta financeira deve ser maior que zero.");

            RuleFor(command => command.EndDate)
                .GreaterThan(DateTime.Now).WithMessage("A data de término não pode estar no passado.");

            RuleFor(command => command.EndDate)
                .GreaterThan(command => command.StartDate)
                .WithMessage("A data de término deve ser posterior à data de início.");
        }
    }
}
