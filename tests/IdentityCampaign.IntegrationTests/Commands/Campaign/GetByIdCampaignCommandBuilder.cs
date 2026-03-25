using Bogus;
using IdentityCampaign.Application.Features.Campaigns.GetCampaignById;

namespace IdentityCampaign.IntegrationTests.Commands.Campaign
{
    public class GetByIdCampaignCommandBuilder
    {
        public static GetByIdCampaignCommand Build()
        {
            return new Faker<GetByIdCampaignCommand>()
                .CustomInstantiator(faker => new GetByIdCampaignCommand(Id: Guid.NewGuid()));
        }
    }
}
