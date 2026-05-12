using IdentityCampaign.Application.Features.Campaigns.UpdateCampaign;

namespace IdentityCampaign.IntegrationTests.Commands.Campaign;

public static class UpdateCampaignCommandBuilder
{
    public static UpdateCampaignCommand Build()
    {
        return new UpdateCampaignCommand(
            Guid.NewGuid(),
            "Campanha Atualizada",
            "Descrição válida para atualização da campanha.",
            5000,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(30)
        );
    }
}