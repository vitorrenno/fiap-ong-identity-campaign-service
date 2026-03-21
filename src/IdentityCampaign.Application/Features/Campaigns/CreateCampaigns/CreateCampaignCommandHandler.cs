using AutoMapper;
using IdentityCampaign.Application.Abstractions;
using IdentityCampaign.Domain.Entities;
using MediatR;

namespace IdentityCampaign.Application.Features.Campaigns.CreateCampaign;

public sealed class CreateCampaignCommandHandler : IRequestHandler<CreateCampaignCommand, CreateCampaignCommandResponse>
{
    private readonly ICampaignRepository _campaignRepository;
    private readonly IMapper _mapper;

    public CreateCampaignCommandHandler(ICampaignRepository campaignRepository, IMapper mapper)
    {
        _campaignRepository = campaignRepository;
        _mapper = mapper;
    }

    public async Task<CreateCampaignCommandResponse> Handle(CreateCampaignCommand request, CancellationToken cancellationToken)
    {
        var campaign = new Campaign(
            title: request.Title,
            description: request.Description,
            goalAmount: request.GoalAmount,
            startDate: request.StartDate,
            endDate: request.EndDate
        );

        await _campaignRepository.AddAsync(campaign, cancellationToken);

        return _mapper.Map<CreateCampaignCommandResponse>(campaign);
    }
}
