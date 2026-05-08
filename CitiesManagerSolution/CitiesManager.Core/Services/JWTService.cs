using CitiesManager.Core.DTO;
using CitiesManager.Core.Entities.IdentityUser;
using CitiesManager.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CitiesManager.Core.Services
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _configuration;

        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public AuthResponseDTO CreateJWTToken(ApplicationUser user)
        {
            double? expiryMins = Convert.ToDouble(_configuration["JWT:ExpiryMinutes"]);
            DateTime expiryTime = DateTime.UtcNow.AddMinutes(expiryMins ?? 0);
            double? expiryRefreshMins = Convert.ToDouble(_configuration["JWT:ExpiryRefreshMinutes"]);
            DateTime expiryRefreshTime = DateTime.UtcNow.AddMinutes(expiryRefreshMins ?? 0);
            string? issuer = _configuration["JWT:Issuer"];
            string? audience = _configuration["JWT:Audience"];

            Claim[] claims  = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
          new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
          ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Name, user.PersonName ?? string.Empty)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
        
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
            JwtSecurityToken tokenData = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiryTime,
                signingCredentials: signingCredentials
            );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string token = tokenHandler.WriteToken(tokenData);

            return new AuthResponseDTO() { Token = token, Email = user.Email, Expiration = expiryTime, PersonName = user.PersonName, RefreshToken=CreateRefreshToken(), RefreshTokenExpiration = expiryRefreshTime };
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidAudience = _configuration["JWT:Audience"],
                ValidateIssuer = false,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"])),
                ValidateLifetime = false  //should be false, because we are validating an expired token
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            ClaimsPrincipal? principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        private string CreateRefreshToken()
        {
            byte[] randomNumber = new byte[64];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
