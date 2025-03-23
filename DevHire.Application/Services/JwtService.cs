using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DevHire.Application.DTO;
using DevHire.Application.ServiceContracts;
using DevHire.Domain.IdentityEntities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace DevHire.Application.Services
{
    public class JwtService : IJwtService
    {

        private readonly IConfiguration _configuration;
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _expiration;
        private readonly string _refreshTokenExpiration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _key = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");
            _issuer = _configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer");
            _audience = _configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience");
            _expiration = _configuration["Jwt:Expiration_Minutes"] ?? throw new ArgumentNullException("Jwt:Expiration_Minutes");
            _refreshTokenExpiration = _configuration["RefreshToken:Expiration_Minutes"] ?? throw new ArgumentNullException("RefreshToken:Expiration_Minutes");
        }

        public AuthenticationResponse GenerateJwtToken(User user, string role)
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("Configuration is not provided.");
            }

            DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_expiration));// Add the expiration time to Current time
            DateTime refreshTokenExpiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_refreshTokenExpiration));// Add the expiration time to Current time


            //Create an array of claim object representing the user's claims, such as ID, Email, etc.,
            Claim[] claim = new Claim[] {

                            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), //Subject (User) ID 
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT unique ID
                            new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString()), // Issued At (Unix timestamp)
                            new Claim(ClaimTypes.Email, user.Email ?? string.Empty) // Unique name Identifer of the user (Email)
                           // new Claim(ClaimTypes.Role, user.ro ?? string.Empty) // Unique name Identifer of the user (Email)
                        };

            if (string.IsNullOrEmpty(_key))
            {
                throw new InvalidOperationException("JWT Key is not provided.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));  //

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claim,
                expires: expiration,
                signingCredentials: signingCredentials
            );

           string jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return new AuthenticationResponse()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Expiration = expiration,
                JwtToken = jwtToken,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpiration = refreshTokenExpiration

            };                
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public ClaimsPrincipal GetClaimsPrincipalFromAccessToken(string? accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {           
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),                             
                ValidateLifetime = false // Ignore token expiration and should be false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
       
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }
                   
                return principal; // Successfully extracted user claims          
        }

        private string GenerateRefreshToken()
        {
            Byte[] bytes = new Byte[64];

            RandomNumberGenerator.Create().GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
    }
}
