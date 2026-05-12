using AutoMapper;
using IdentityCampaign.Application.Abstractions;
using IdentityCampaign.Domain.Entities;
using MediatR;

namespace IdentityCampaign.Application.Features.Campaigns.UpdateCampaign;

public sealed class UpdateCampaignCommandHandler(ICampaignRepository campaignRepository, IMapper mapper) : IRequestHandler<UpdateCampaignCommand, UpdateCampaignCommandResponse>
{
    private readonly ICampaignRepository _campaignRepository = campaignRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<UpdateCampaignCommandResponse> Handle(UpdateCampaignCommand request, CancellationToken cancellationToken)
    {
        Campaign? campaign = await _campaignRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException($"Campanha com ID {request.Id} não encontrada.");

        campaign.Update(
            request.Title,
            request.Description,
            request.GoalAmount,
            request.StartDate,
            request.EndDate
        );

        await _campaignRepository.UpdateAsync(campaign, cancellationToken);

        return _mapper.Map<UpdateCampaignCommandResponse>(campaign);
    }
}
