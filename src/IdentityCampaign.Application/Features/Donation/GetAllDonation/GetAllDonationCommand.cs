using MediatR;

namespace IdentityCampaign.Application.Features.Donation.GetAllDonation
{
    public class GetAllDonationCommand : IRequest<IEnumerable<GetAllDonationResponse>>
    {
    }
}
