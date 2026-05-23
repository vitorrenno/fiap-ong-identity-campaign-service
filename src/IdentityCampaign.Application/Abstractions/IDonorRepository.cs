using IdentityCampaign.Domain.Entities;

namespace IdentityCampaign.Application.Abstractions;

public interface IDonorRepository
{
    Task AddAsync(Donor donor, CancellationToken cancellationToken = default);
    Task<Donor?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Donor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
