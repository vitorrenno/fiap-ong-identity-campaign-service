using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityCampaign.Domain.Entities;

namespace IdentityCampaign.Application.Abstractions
{
    public interface IDonationRepository
    {
        Task AddAsync(Donation donation, CancellationToken cancellationToken = default);

    }
}
