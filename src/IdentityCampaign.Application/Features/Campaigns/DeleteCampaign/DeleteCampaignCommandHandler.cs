using IdentityCampaign.Application.Abstractions;
using MediatR;

namespace IdentityCampaign.Application.Features.Campaigns.DeleteCampaign;

public sealed class DeleteCampaignCommandHandler : IRequestHandler<DeleteCampaignCommand>
{
    private readonly ICampaignRepository _campaignRepository;

    public DeleteCampaignCommandHandler(ICampaignRepository campaignRepository)
    {
        _campaignRepository = campaignRepository;
    }

    public async Task Handle(DeleteCampaignCommand request, CancellationToken cancellationToken)
    {
        var campaign = await _campaignRepository.GetByIdAsync(request.Id, cancellationToken);

        if (campaign is null)
            throw new KeyNotFoundException($"Campanha com ID {request.Id} não encontrada.");

        await _campaignRepository.DeleteAsync(campaign, cancellationToken);
    }
}
