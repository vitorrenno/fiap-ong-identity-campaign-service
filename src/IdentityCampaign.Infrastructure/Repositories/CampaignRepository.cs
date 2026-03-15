using IdentityCampaign.Application.Abstractions;
using IdentityCampaign.Domain.Entities;
using IdentityCampaign.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IdentityCampaign.Infrastructure.Repositories;

public class CampaignRepository : ICampaignRepository
{
    private readonly IdentityCampaignDbContext _context;

    public CampaignRepository(IdentityCampaignDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Campaign campaign, CancellationToken cancellationToken = default)
    {
        await _context.Campaigns.AddAsync(campaign, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Campaign?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Campaigns.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Campaign>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Campaigns
            .AsNoTracking()
            .OrderBy(x => x.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Campaign campaign, CancellationToken cancellationToken = default)
    {
        _context.Campaigns.Update(campaign);
        await _context.SaveChangesAsync(cancellationToken);
    }
}