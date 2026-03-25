using MediatR;

namespace IdentityCampaign.Application.Features.Campaigns.GetAllCampaign
{
    public sealed record GetAllCampaignCommand : IRequest<IEnumerable<GetAllCampaignCommandResponse>>
    {
    }
}
