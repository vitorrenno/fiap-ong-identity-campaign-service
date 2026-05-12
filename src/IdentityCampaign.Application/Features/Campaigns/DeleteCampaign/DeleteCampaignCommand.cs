using MediatR;

namespace IdentityCampaign.Application.Features.Campaigns.DeleteCampaign;

public sealed record DeleteCampaignCommand(Guid Id) : IRequest;
