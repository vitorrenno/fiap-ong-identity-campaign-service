using MediatR;

namespace IdentityCampaign.Application.Features.Campaigns.GetCampaignById
{
    public sealed record GetByIdCampaignCommand(Guid Id) : IRequest<GetByIdCampaignCommandResponse>;
}
