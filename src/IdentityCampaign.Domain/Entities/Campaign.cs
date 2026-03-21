using IdentityCampaign.Domain.Enums;

namespace IdentityCampaign.Domain.Entities;

public class Campaign
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal GoalAmount { get; private set; }
    public decimal AmountRaised { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public CampaignStatus Status { get; private set; }

    private Campaign()
    {
    }

    public Campaign(string title, string description, decimal goalAmount, DateTime startDate, DateTime endDate)
    {
        FieldsValidate(title, goalAmount, startDate, endDate);

        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        GoalAmount = goalAmount;
        AmountRaised = 0;
        StartDate = startDate;
        EndDate = endDate;
        Status = CampaignStatus.Active;
    }

    public void Update(string title, string description, decimal goalAmount, DateTime startDate, DateTime endDate)
    {
        FieldsValidate(title, goalAmount, startDate, endDate);

        Title = title;
        Description = description;
        GoalAmount = goalAmount;
        StartDate = startDate;
        EndDate = endDate;
    }

    private static void FieldsValidate(string title, decimal goalAmount, DateTime startDate, DateTime endDate)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Campaign title is required.");

        if (goalAmount <= 0)
            throw new ArgumentException("Goal amount must be greater than zero.");

        if (endDate <= startDate)
            throw new ArgumentException("End date must be greater than start date.");
    }

    public void Cancel()
    {
        Status = CampaignStatus.Cancelled;
    }

    public void AddRaisedAmount(decimal amount)
    {
        if (amount <= 0)
            throw new Exception("Valor inválido");

        AmountRaised += amount;

        if (AmountRaised >= GoalAmount)
            Status = CampaignStatus.Completed;
    }
}
