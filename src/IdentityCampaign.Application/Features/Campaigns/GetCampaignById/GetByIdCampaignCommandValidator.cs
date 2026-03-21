using FluentValidation;

namespace IdentityCampaign.Application.Features.Campaigns.GetCampaignById
{
    public class GetByIdCampaignCommandValidator : AbstractValidator<GetByIdCampaignCommand>
    {
        public GetByIdCampaignCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("O ID da campanha é obrigatório.");
        }
    }
}
