using IdentityCampaign.Application.Features.Campaigns.GetCampaignById;
using MediatR;

namespace IdentityCampaign.Application.Features.Donation.GetDonationById
{
    public sealed record GetDonationByIdCommand(Guid Id) : IRequest<GetDonationByIdResponse> { }
}
