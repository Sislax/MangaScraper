using AuthService.Api.Interfaces;
using AuthService.Api.Models.Settings;
using MangaScraper.Data.Data;
using MangaScraper.Data.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserIdentityDbContext _context;
        private readonly Settings _settings;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, UserIdentityDbContext context, Settings settings, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _settings = settings;
            _logger = logger;
        }

        public async Task<bool> RegisterAsync(RegistrationUser userData)
        {
            User user = new User(userData.UserName, userData.Email);

            var result = await _userManager.CreateAsync(user, userData.Password);

            await _context.SaveChangesAsync();

            return result.Succeeded;
        }

        public async Task<LoginResponse> LoginAsync(LoginUser credentials)
        {
            try
            {
                User? user = await _userManager.FindByEmailAsync(credentials.Email);

                if (user == null || await _userManager.CheckPasswordAsync(user, credentials.Password) == false)
                {
                    return new LoginResponse();
                }

                string newRefreshToken = GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiration = DateTime.Now.AddHours(24);

                LoginResponse response = new LoginResponse(await GenerateJwtTokenAsync(user), newRefreshToken, user.RefreshTokenExpiration);

                response.TokenExpired = user.RefreshTokenExpiration;

                await _userManager.UpdateAsync(user);

                await _context.SaveChangesAsync();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile effettuare il login per lo user.... . {ex}", ex);
                throw;
            }
        }

        private async Task<List<Claim>> GetClaimsAsync(string username)
        {
            
            try
            {
                User? user = await _userManager.FindByNameAsync(username);

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, username),
                };

                claims.AddRange(await _userManager.GetClaimsAsync(user!));
                IList<string> roles = await _userManager.GetRolesAsync(user!);

                foreach (string role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));

                    IdentityRole? identityRole = await _roleManager.FindByNameAsync(role);
                    claims.AddRange(await _roleManager.GetClaimsAsync(identityRole!));
                }

                return claims;
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile ottenere i claims per lo user {username}. {ex}", username, ex);
                throw;
            }
        }

        private async Task<string> GenerateJwtTokenAsync(User user)
        {
            try
            {
                List<Claim> claims = await GetClaimsAsync(user.UserName!);
        
                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
                SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _settings.Issuer,
                    audience: _settings.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(_settings.ExpiryMinutes),
                    signingCredentials: credentials
                    );
        
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile generare il JwtToken. {ex}", ex);
                throw;
            }
        }
        
        private string GenerateRefreshToken()
        {
            try
            {
                var randomNumber = new byte[32];

                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomNumber);
                }

                return Convert.ToBase64String(randomNumber);
            }
            catch(Exception ex)
            {
                _logger.LogError("ERRORE: impossibile generare il RefreshToken. {ex}", ex);
                throw;
            }
        }

        public async Task<LoginResponse> RefreshTokenExist(string refreshToken)
        {
            User? user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

            if(user == null)
            {
                return new LoginResponse();
            }

            string newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiration = DateTime.Now;

            await _userManager.UpdateAsync(user);

            await _context.SaveChangesAsync();

            return new LoginResponse(await GenerateJwtTokenAsync(user), newRefreshToken, user.RefreshTokenExpiration);
        }
    }
}
