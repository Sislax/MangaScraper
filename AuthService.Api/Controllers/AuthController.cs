using AuthService.Api.Interfaces;
using AuthService.Api.Models.Settings;
using MangaScraper.Data.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly Settings _settings;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, Settings settings, ILogger<AuthController> logger)
        {
            _authService = authService;
            _settings = settings;
            _logger = logger;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(RegistrationUser credentials)
        {
            if(await _authService.RegisterAsync(credentials))
            {
                return Ok("Registrazione completata.");
            }

            return BadRequest("Ops... Qualcosa è andato storto.");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUser credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            LoginResponse loginResponse = await _authService.LoginAsync(credentials);

            if (loginResponse.IsLoggedIn)
            {
                return Ok(loginResponse);
            }

            return Unauthorized();
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel refreshTokenModel)
        {
            LoginResponse loginResult = await _authService.SetUserRefreshToken(refreshTokenModel);

            if (loginResult.IsLoggedIn)
            {
                return Ok(loginResult);
            }

            return Unauthorized();
        }
    }
}
