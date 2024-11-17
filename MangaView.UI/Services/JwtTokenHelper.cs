using MangaView.UI.Utiles;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MangaView.UI.Services
{
    public static class JwtTokenHelper
    {
        public static List<Claim> ValidateDecodeToken(string token, Settings settings)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = settings.Issuer,
                    ValidAudience = settings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        settings.SecretKey))
                }, out SecurityToken validatedToken);
            }
            catch (Exception)
            {
                return new List<Claim>();
            }

            JwtSecurityToken securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken ?? throw new Exception();
            return securityToken.Claims.ToList();
        }
    }
}
