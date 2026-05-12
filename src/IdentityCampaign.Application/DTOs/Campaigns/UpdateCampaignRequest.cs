namespace IdentityCampaign.Application.DTOs.Campaigns;

public sealed record UpdateCampaignRequest(
    string Title,
    string Description,
    decimal GoalAmount,
    DateTime StartDate,
    DateTime EndDate
);
