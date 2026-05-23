using IdentityCampaign.Application.Abstractions;
using IdentityCampaign.Domain.Entities;
using IdentityCampaign.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IdentityCampaign.Infrastructure.Repositories;

public class DonorRepository : IDonorRepository
{
    private readonly IdentityCampaignDbContext _context;

    public DonorRepository(IdentityCampaignDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Donor donor, CancellationToken cancellationToken = default)
    {
        await _context.Donors.AddAsync(donor, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Donor?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Donors
            .AsNoTracking()
            .FirstOrDefaultAsync(donor => donor.Email == email, cancellationToken);
    }

    public async Task<Donor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Donors.FirstOrDefaultAsync(donor => donor.Id == id, cancellationToken);
    }
}
