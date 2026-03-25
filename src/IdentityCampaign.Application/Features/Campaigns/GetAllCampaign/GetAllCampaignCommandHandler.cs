using AutoMapper;
using IdentityCampaign.Application.Abstractions;
using MediatR;

namespace IdentityCampaign.Application.Features.Campaigns.GetAllCampaign
{
    public sealed class GetAllCampaignCommandHandler : IRequestHandler<GetAllCampaignCommand, IEnumerable<GetAllCampaignCommandResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ICampaignRepository _campaignRepository;

        public GetAllCampaignCommandHandler(IMapper mapper, ICampaignRepository campaignRepository)
        {
            _mapper = mapper;
            _campaignRepository = campaignRepository;
        }

        public async Task<IEnumerable<GetAllCampaignCommandResponse>> Handle(GetAllCampaignCommand request, CancellationToken cancellationToken)
        {
            var campaign = await _campaignRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GetAllCampaignCommandResponse>>(campaign);
        }
    }
}
