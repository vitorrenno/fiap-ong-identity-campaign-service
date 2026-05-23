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
            DonationReceivedEvent evento = context.Message;

            Console.WriteLine(
                $"[RabbitMQ] DonationReceivedEvent consumido | " +
                $"CampaignId: {evento.CampaignId} | " +
                $"Valor: {evento.DonationValue:C} | " +
                $"Timestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");

            await _campaignRepository.IncrementRaisedAmountAsync(evento.CampaignId, evento.DonationValue);
        }
    }
}
