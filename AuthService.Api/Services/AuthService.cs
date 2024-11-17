using AuthService.Api.Interfaces;
using AuthService.Api.Models.Settings;
using MangaScraper.Data.Data;
using MangaScraper.Data.Models.Auth;
using Microsoft.AspNetCore.Identity;
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
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly UserIdentityDbContext _context;
        private readonly Settings _settings;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, UserIdentityDbContext context, Settings settings, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _settings = settings;
            _logger = logger;
        }

        public async Task<bool> RegisterAsync(RegistrationUser userData)
        {
            User user = new User(userData.UserName, userData.Email);

            var result = await _userManager.CreateAsync(user, userData.Password);
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

                LoginResponse response = new LoginResponse(await GenerateJwtTokenAsync(user), GenerateRefreshTokenAsync())
                                         {
                                             IsLoggedIn = true
                                         };

                user.RefreshToken = response.RefreshToken;
                user.RefreshTokenExpiration = DateTime.Now.AddHours(24);

                await _userManager.UpdateAsync(user);

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

                // Per ogni role dello user trovo i claims e li aggiungo alla lista di claim
                foreach (string role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));

                    Role? identityRole = await _roleManager.FindByNameAsync(role);
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
        
        private string GenerateRefreshTokenAsync()
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

        public async Task<LoginResponse> SetUserRefreshToken(RefreshTokenModel refreshTokenModel)
        {
            ClaimsPrincipal principal = GetTokenPrincipal(refreshTokenModel.JwtToken);

            if(principal?.Identity?.Name is null)
            {
                return new LoginResponse();
            }

            User? user = await _userManager.FindByNameAsync(principal.Identity.Name);

            if (user is null || user.RefreshToken != refreshTokenModel.RefreshToken || user.RefreshTokenExpiration > DateTime.Now)
            {
                return new LoginResponse();
            }

            LoginResponse response = new LoginResponse(await GenerateJwtTokenAsync(user), GenerateRefreshTokenAsync())
            {
                IsLoggedIn = true
            };

            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpiration = DateTime.Now.AddHours(24);

            await _userManager.UpdateAsync(user);

            return response;
        }

        private ClaimsPrincipal GetTokenPrincipal(string jwtToken)
        {
            TokenValidationParameters validation = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _settings.Issuer,
                ValidAudience = _settings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey))
            };

            return new JwtSecurityTokenHandler().ValidateToken(jwtToken, validation, out _);
        }
    }
}
