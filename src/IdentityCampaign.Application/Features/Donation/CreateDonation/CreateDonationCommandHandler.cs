using AutoMapper;
using IdentityCampaign.Application.Abstractions;
using IdentityCampaign.Application.Features.Campaigns.CreateCampaign;
using IdentityCampaign.Domain.Entities;
using MediatR;

namespace IdentityCampaign.Application.Features.Donation.CreateDonation
{
    public class CreateDonationCommandHandler : IRequestHandler<CreateDonationCommand, CreateDonationResponse>
    {
        private readonly IDonationRepository _donationRepository;
        private readonly ICampaignRepository _campaignRepository;
        private readonly IMapper _mapper;
        public CreateDonationCommandHandler(IDonationRepository donationRepository, ICampaignRepository campaignRepository, IMapper mapper)
        {
            _donationRepository = donationRepository;
            _campaignRepository = campaignRepository;
            _mapper = mapper;
        }

        public async Task<CreateDonationResponse> Handle(CreateDonationCommand request, CancellationToken cancellationToken)
        {
            var donation = new Domain.Entities.Donation(request.vAmount,request.IdCampaign,request.IdUser);

            //Verifica se a campanha existe antes de doar para a mesma.
            var campaign = await _campaignRepository.GetByIdAsync(request.IdCampaign, cancellationToken);
            if (campaign == null)
                throw new KeyNotFoundException($"Campanha com ID {request.IdCampaign} não encontrado.");

            //Aplicar chamada para o service aqui
            await _donationRepository.AddAsync(donation, cancellationToken);
            return _mapper.Map<CreateDonationResponse>(donation);

        }
    }
}
