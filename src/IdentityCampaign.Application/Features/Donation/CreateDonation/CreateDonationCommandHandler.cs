using AutoMapper;
using IdentityCampaign.Application.Abstractions;
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
            Domain.Entities.Donation donation = new Domain.Entities.Donation(request.vAmount,request.IdCampaign,request.IdUser);

            //Verifica se a campanha existe antes de doar para a mesma.
            Domain.Entities.Campaign? campaign = await _campaignRepository.GetByIdAsync(request.IdCampaign, cancellationToken)
                ?? throw new KeyNotFoundException($"Campanha com ID {request.IdCampaign} não encontrado.");

            //Aplicar chamada para o service aqui
            await _donationRepository.AddAsync(donation, cancellationToken);
            return _mapper.Map<CreateDonationResponse>(donation);

        }
    }
}
