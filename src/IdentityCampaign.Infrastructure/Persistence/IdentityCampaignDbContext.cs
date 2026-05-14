using IdentityCampaign.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityCampaign.Infrastructure.Persistence;

public class IdentityCampaignDbContext : DbContext
{
    public IdentityCampaignDbContext(DbContextOptions<IdentityCampaignDbContext> options)
        : base(options)
    {
    }
    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<Donor> Donors => Set<Donor>();
    public DbSet<Donation> Donations => Set<Donation>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityCampaignDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
