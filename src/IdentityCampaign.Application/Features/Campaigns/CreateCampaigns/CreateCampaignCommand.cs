using MediatR;

namespace IdentityCampaign.Application.Features.Campaigns.CreateCampaign;

public sealed record CreateCampaignCommand(
    string Title,
    string Description,
    decimal GoalAmount,
    DateTime StartDate,
    DateTime EndDate
) : IRequest<CreateCampaignCommandResponse>;
