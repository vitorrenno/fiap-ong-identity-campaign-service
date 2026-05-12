using MediatR;

namespace IdentityCampaign.Application.Features.Campaigns.UpdateCampaign;

public sealed record UpdateCampaignCommand(
    Guid Id,
    string Title,
    string Description,
    decimal GoalAmount,
    DateTime StartDate,
    DateTime EndDate
) : IRequest<UpdateCampaignCommandResponse>;
