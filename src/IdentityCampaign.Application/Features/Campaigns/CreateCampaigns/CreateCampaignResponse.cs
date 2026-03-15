namespace IdentityCampaign.Application.Features.Campaigns.CreateCampaign;

public sealed record CreateCampaignResponse(
    Guid Id,
    string Title,
    string Description,
    decimal GoalAmount,
    decimal AmountRaised,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive
);