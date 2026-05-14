using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LauncherHero.Starter.Data.Repositories;
using LauncherHero.Starter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace LauncherHero.Starter.Services;

public class AuthService
{
    private readonly UserRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public AuthService(UserRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        if (await _repository.GetByEmailAsync(request.Email) is not null)
            return null;

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(null!, request.Password),
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(user);
        return CreateAuthResponse(created);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _repository.GetByEmailAsync(request.Email);
        if (user is null)
            return null;

        var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verify == PasswordVerificationResult.Failed)
            return null;

        return CreateAuthResponse(user);
    }

    private AuthResponse CreateAuthResponse(User user)
    {
        var jwt = GenerateJwt(user);
        return new AuthResponse
        {
            AccessToken = jwt.Token,
            ExpiresAt = jwt.ExpiresAt,
            User = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            }
        };
    }

    private (string Token, DateTime ExpiresAt) GenerateJwt(User user)
    {
        var issuer = _configuration["Jwt:Issuer"] ?? "LauncherHero.Starter";
        var audience = _configuration["Jwt:Audience"] ?? "LauncherHero.Starter";
        var secret = _configuration["Jwt:Secret"] ?? "ChangeThisSecretToAStrongValue";
        var expiresAt = DateTime.UtcNow.AddHours(2);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
