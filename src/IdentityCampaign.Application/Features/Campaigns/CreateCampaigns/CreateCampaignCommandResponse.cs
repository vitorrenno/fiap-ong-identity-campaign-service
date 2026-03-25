using IdentityCampaign.Domain.Enums;

namespace IdentityCampaign.Application.Features.Campaigns.CreateCampaign;

public sealed record CreateCampaignCommandResponse(
    Guid Id,
    string Title,
    string Description,
    decimal GoalAmount,
    decimal AmountRaised,
    DateTime StartDate,
    DateTime EndDate,
    CampaignStatus Status
);
