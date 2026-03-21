using IdentityCampaign.Domain.Enums;

namespace IdentityCampaign.Application.Features.Campaigns.GetCampaignById
{
    public sealed class GetByIdCampaignCommandResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal GoalAmount { get; set; }
        public decimal AmountRaised { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CampaignStatus Status { get; set; }
    }
}
