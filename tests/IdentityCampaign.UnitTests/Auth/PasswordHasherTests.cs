using FluentAssertions;
using IdentityCampaign.Infrastructure.Services;
using Xunit;

namespace IdentityCampaign.UnitTests.Auth;

public class PasswordHasherTests
{
    [Fact]
    public void Hash_ShouldCreateNonEmptyHash_AndVerifyPassword()
    {
        var hasher = new BcryptPasswordHasher();
        var password = "MyS3cretP@ssw0rd";

        var hashed = hasher.Hash(password);

        hashed.Should().NotBeNullOrWhiteSpace();
        hashed.Should().NotBe(password);
        hasher.Verify(password, hashed).Should().BeTrue();
        hasher.Verify("wrong-password", hashed).Should().BeFalse();
    }
}
