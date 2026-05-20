namespace IdentityCampaign.Application.Messaging.Events
{
    public record DonationReceivedEvent(
        Guid DonationId,
        Guid campaignId,
        decimal DonationValue,
        DateTime ConfirmedAt
    );
}
