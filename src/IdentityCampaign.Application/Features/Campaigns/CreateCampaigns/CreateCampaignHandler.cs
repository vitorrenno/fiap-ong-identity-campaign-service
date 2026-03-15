using IdentityCampaign.Application.Abstractions;
using IdentityCampaign.Domain.Entities;
using MediatR;

namespace IdentityCampaign.Application.Features.Campaigns.CreateCampaign;

public sealed class CreateCampaignHandler : IRequestHandler<CreateCampaignCommand, CreateCampaignResponse>
{
    private readonly ICampaignRepository _campaignRepository;

    public CreateCampaignHandler(ICampaignRepository campaignRepository)
    {
        _campaignRepository = campaignRepository;
    }

    public async Task<CreateCampaignResponse> Handle(
        CreateCampaignCommand request,
        CancellationToken cancellationToken)
    {
        var campaign = new Campaign(
            request.Title,
            request.Description,
            request.GoalAmount,
            request.StartDate,
            request.EndDate);

        await _campaignRepository.AddAsync(campaign, cancellationToken);

        return new CreateCampaignResponse(
            campaign.Id,
            campaign.Title,
            campaign.Description,
            campaign.GoalAmount,
            campaign.AmountRaised,
            campaign.StartDate,
            campaign.EndDate,
            campaign.IsActive);
    }
}