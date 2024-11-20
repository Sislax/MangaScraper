using AuthService.Api.Interfaces;
using MangaScraper.Data.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
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

            return Ok(loginResponse);
        }

        [HttpPost("RefreshTokenAndLogin")]
        public async Task<IActionResult> RefreshTokenAndLogin(string refreshTokenModel)
        {
            LoginResponse response = await _authService.RefreshTokenExist(refreshTokenModel);

            if(response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }
    }
}
