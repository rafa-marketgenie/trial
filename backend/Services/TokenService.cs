using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using trial.Models;

namespace trial.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        private readonly string _secretKey;
        private readonly int _accessTokenExpiryMinutes;

        public TokenService(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _secretKey = _config["ApiSettings:Secret"] ?? throw new ArgumentNullException("ApiSettings:Secret configuration is missing.");
            _accessTokenExpiryMinutes = _config.GetValue<int>("ApiSettings:AccessTokenExpiryMinutes", 60);
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
                // new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["ApiSettings:Issuer"],
                audience: _config["ApiSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_accessTokenExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}