using IdentityCampaign.Application.Abstractions;
using MassTransit;

namespace IdentityCampaign.Application.Messaging.Events
{
    public class DonationReceivedConsumer : IConsumer<DonationReceivedEvent>
    {
        private readonly ICampaignRepository _campaignRepository;

        public DonationReceivedConsumer(ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository;
        }

        public async Task Consume(ConsumeContext<DonationReceivedEvent> context)
        {
            var evento = context.Message;

            await _campaignRepository.IncrementRaisedAmountAsync(evento.campaignId, evento.DonationValue);
        }
    }
}
