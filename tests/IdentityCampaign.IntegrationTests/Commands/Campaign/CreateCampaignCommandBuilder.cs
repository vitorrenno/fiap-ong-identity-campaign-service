using Bogus;
using IdentityCampaign.Application.Features.Campaigns.CreateCampaign;

namespace IdentityCampaign.IntegrationTests.Commands.Campaign
{
    public class CreateCampaignCommandBuilder
    {
        public static CreateCampaignCommand Build()
        {
            return new Faker<CreateCampaignCommand>().CustomInstantiator(faker => new CreateCampaignCommand(
                    Title: faker.Commerce.ProductName(),
                    Description: faker.Lorem.Paragraph(),
                    GoalAmount: faker.Finance.Amount(100, 100000),
                    StartDate: DateTime.Today,
                    EndDate: DateTime.Today.AddMonths(3)
                ));
        }
    }
}
