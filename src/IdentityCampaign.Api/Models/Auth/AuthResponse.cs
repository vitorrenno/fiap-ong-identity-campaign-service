namespace IdentityCampaign.Api.Models.Auth;

public sealed record AuthResponse(
    string Token,
    string Role,
    string Email);
