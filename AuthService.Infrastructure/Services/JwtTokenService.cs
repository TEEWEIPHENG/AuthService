using AuthService.Application.Interfaces.Services;
using AuthService.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace AuthService.Infrastructure.Services
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string Secret { get; set; } = null!;
        public int ExpiryMinutes { get; set; }
    }
    public class JwtTokenService : ITokenService
    {
        private readonly JwtOptions _options;
        ILogger<JwtTokenService> _logger;

        public JwtTokenService(IOptions<JwtOptions> options, ILogger<JwtTokenService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public TokenResult Generate(User user)
        {
            _logger.LogInformation("======== Start Generate Token for {Email} ========", user.Email);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
                new Claim("username", user.Username),
                new Claim("role", user.Role.Value),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenResult(accessToken, DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes));
        }

        public string RefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

}
