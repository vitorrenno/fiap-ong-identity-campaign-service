namespace IdentityCampaign.Api.Models.Auth;

public sealed record RegisterDonorRequest(
    string FullName,
    string Email,
    string Cpf,
    string Password,
    string Role);
