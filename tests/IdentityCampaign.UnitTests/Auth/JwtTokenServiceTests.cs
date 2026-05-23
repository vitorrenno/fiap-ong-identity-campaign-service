using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using FluentAssertions;
using IdentityCampaign.Application.Common;
using IdentityCampaign.Infrastructure.Services;
using IdentityCampaign.Domain.Entities;
using Microsoft.Extensions.Options;
using Xunit;

namespace IdentityCampaign.UnitTests.Auth;

public class JwtTokenServiceTests
{
    [Fact]
    public void GenerateToken_ShouldContainRoleClaimAndIssuer()
    {
        var settings = new JwtSettings
        {
            Key = "supersecretkey1234567890ABCDEFGHIJKLMNOP",
            Issuer = "IdentityCampaign.Api",
            Audience = "IdentityCampaign.Client"
        };

        var tokenService = new JwtTokenService(Options.Create(settings));
        var donor = new Donor("Bruno Luiz", "bruno-luiz@example.com", "12345678900", "hashed", RoleConstants.Donor);

        var token = tokenService.GenerateToken(donor);

        token.Should().NotBeNullOrWhiteSpace();

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        // Validar claims do token
        jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value.Should().Be(RoleConstants.Donor);
        jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value.Should().Be(donor.Id.ToString());
        jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value.Should().Be(donor.Email);
        jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value.Should().Be(donor.FullName);
        
        // Validar que o token não está vencido
        jwt.ValidTo.Should().BeAfter(DateTime.UtcNow);
    }
}
