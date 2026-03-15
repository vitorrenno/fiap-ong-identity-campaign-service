namespace IdentityCampaign.Domain.Entities;

public class Donor
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Cpf { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;

    private Donor()
    {
    }

    public Donor(
        string fullName,
        string email,
        string cpf,
        string passwordHash,
        string role)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF is required.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.");

        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role is required.");

        Id = Guid.NewGuid();
        FullName = fullName;
        Email = email;
        Cpf = cpf;
        PasswordHash = passwordHash;
        Role = role;
    }
}