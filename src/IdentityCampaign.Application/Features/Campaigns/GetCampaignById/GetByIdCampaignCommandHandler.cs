using AutoMapper;
using IdentityCampaign.Application.Abstractions;
using MediatR;

namespace IdentityCampaign.Application.Features.Campaigns.GetCampaignById
{
    public class GetByIdCampaignCommandHandler : IRequestHandler<GetByIdCampaignCommand, GetByIdCampaignCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICampaignRepository _campaignRepository;

        public GetByIdCampaignCommandHandler(IMapper mapper, ICampaignRepository campaignRepository)
        {
            _mapper = mapper;
            _campaignRepository = campaignRepository;
        }

        public async Task<GetByIdCampaignCommandResponse> Handle(GetByIdCampaignCommand request, CancellationToken cancellationToken)
        {
            var campaign = await _campaignRepository.GetByIdAsync(request.Id);

            if (campaign == null)
                throw new KeyNotFoundException($"Jogo com ID {request.Id} não encontrado.");

            return _mapper.Map<GetByIdCampaignCommandResponse> (campaign);
        }
    }
}
