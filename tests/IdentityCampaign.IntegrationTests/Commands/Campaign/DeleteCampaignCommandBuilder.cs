using IdentityCampaign.Application.Features.Campaigns.DeleteCampaign;

namespace IdentityCampaign.IntegrationTests.Commands.Campaign;

public static class DeleteCampaignCommandBuilder
{
    public static DeleteCampaignCommand Build()
    {
        return new DeleteCampaignCommand(Guid.NewGuid());
    }
}