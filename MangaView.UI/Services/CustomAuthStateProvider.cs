using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace MangaView.UI.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly LoginService _loginService;

        public CustomAuthStateProvider(LoginService loginService)
        {
            _loginService = loginService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            List<Claim> claims = await _loginService.GetLoginInfoAsync();

            ClaimsIdentity claimsIdentity = claims.Count != 0 
                ? new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme, "name", "role")
                : new ClaimsIdentity();

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return new AuthenticationState(claimsPrincipal);
        }
    }
}
