using FluentValidation;

namespace IdentityCampaign.Application.Features.Campaigns.DeleteCampaign;

public sealed class DeleteCampaignCommandValidator : AbstractValidator<DeleteCampaignCommand>
{
    public DeleteCampaignCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("O ID da campanha é obrigatório.");
    }
}
