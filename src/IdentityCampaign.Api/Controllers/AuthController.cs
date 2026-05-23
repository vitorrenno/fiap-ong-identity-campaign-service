using IdentityCampaign.Application.Abstractions;
using IdentityCampaign.Application.Common;
using IdentityCampaign.Api.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityCampaign.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IDonorRepository _donorRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(
        IDonorRepository donorRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _donorRepository = donorRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDonorRequest request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email?.Trim().ToLowerInvariant() ?? string.Empty;
        var existingDonor = await _donorRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (existingDonor is not null)
            return Conflict(new { message = "Email already registered." });

        var role = string.IsNullOrWhiteSpace(request.Role)
            ? RoleConstants.Donor
            : request.Role.Trim();

        if (!RoleConstants.All.Contains(role, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new { message = "Invalid role. Use Admin or Donor." });

        var donor = new Domain.Entities.Donor(
            request.FullName,
            normalizedEmail,
            request.Cpf,
            _passwordHasher.Hash(request.Password),
            role);

        await _donorRepository.AddAsync(donor, cancellationToken);

        return Created(string.Empty, new
        {
            donor.Id,
            donor.FullName,
            donor.Email,
            donor.Role
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email?.Trim().ToLowerInvariant() ?? string.Empty;
        var donor = await _donorRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (donor is null || !_passwordHasher.Verify(request.Password, donor.PasswordHash))
            return Unauthorized(new { message = "Invalid credentials." });

        var token = _jwtTokenService.GenerateToken(donor);

        return Ok(new AuthResponse(token, donor.Role, donor.Email));
    }
}
