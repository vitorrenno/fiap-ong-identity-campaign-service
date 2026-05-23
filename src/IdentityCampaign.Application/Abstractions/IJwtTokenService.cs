using IdentityCampaign.Domain.Entities;

namespace IdentityCampaign.Application.Abstractions;

public interface IJwtTokenService
{
    string GenerateToken(Donor donor);
}
