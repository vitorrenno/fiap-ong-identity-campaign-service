namespace IdentityCampaign.Application.Common;

public static class RoleConstants
{
    public const string Admin = "admin";
    public const string Donor = "donor";

    public static IReadOnlyCollection<string> All { get; } = new[] { Admin, Donor };
}
