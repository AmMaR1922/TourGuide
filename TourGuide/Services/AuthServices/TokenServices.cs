using ApplicationLayer.Contracts.Auth;
using ApplicationLayer.Models;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace TourGuide.Services.AuthServices
{
    public class TokenServices(UserManager<ApplicationUser> userManager , IOptions<JWT> option):ITokenServices
    {
        private readonly UserManager<ApplicationUser> UserManager = userManager;
        private readonly JWT jWT = option.Value;
        public async Task<string> GetAccessToken(ApplicationUser User)
        {
            var AuthCliams = new List<Claim> { new Claim(ClaimTypes.Email, User.Email!) };

            var roles = await UserManager.GetRolesAsync(User);
            foreach (var role in roles)
            {
                AuthCliams.Add(new Claim(ClaimTypes.Role, role));
            }

            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWT.Key));
            var signingCred = new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256);
            var JwtSecurityToken = new JwtSecurityToken(
                issuer: jWT.Issuer,
                audience: jWT.Audience,
                signingCredentials: signingCred,
                claims: AuthCliams,
                expires: DateTime.UtcNow.AddMinutes(jWT.DurationInMinutes)

                );
            return new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken);

        }

        public RefreshToken GetRefreshToken()
        {
            var refreshToken = new Byte[32];
            using var Genrator = RandomNumberGenerator.Create();
            Genrator.GetBytes(refreshToken);
            return new RefreshToken()
            {
                CreatedOn = DateTime.UtcNow,
                ExpireOn = DateTime.UtcNow.AddDays(30),
                Token = Convert.ToBase64String(refreshToken)
            };
        }

       
    }
}
