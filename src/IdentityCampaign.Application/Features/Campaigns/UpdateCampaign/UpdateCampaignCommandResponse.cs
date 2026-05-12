using IdentityCampaign.Domain.Enums;

namespace IdentityCampaign.Application.Features.Campaigns.UpdateCampaign;

public sealed record UpdateCampaignCommandResponse(
    Guid Id,
    string Title,
    string Description,
    decimal GoalAmount,
    decimal AmountRaised,
    DateTime StartDate,
    DateTime EndDate,
    CampaignStatus Status
);
