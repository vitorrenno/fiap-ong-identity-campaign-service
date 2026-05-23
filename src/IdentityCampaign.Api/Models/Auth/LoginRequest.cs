namespace IdentityCampaign.Api.Models.Auth;

public sealed record LoginRequest(
    string Email,
    string Password);
