using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityCampaign.Application.Abstractions;
using IdentityCampaign.Domain.Entities;
using IdentityCampaign.Infrastructure.Persistence;

namespace IdentityCampaign.Infrastructure.Repositories
{
    public class DonationRepository : IDonationRepository
    {
        private readonly IdentityCampaignDbContext _context;
        public DonationRepository(IdentityCampaignDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Donation donation, CancellationToken cancellationToken = default)
        {
            await _context.Donations.AddAsync(donation, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
