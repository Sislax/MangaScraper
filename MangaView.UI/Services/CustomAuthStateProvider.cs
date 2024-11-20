using MangaScraper.Data.Models.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MangaView.UI.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _localStorage;

        public CustomAuthStateProvider(ProtectedLocalStorage protectedLocalStorage)
        {
            _localStorage = protectedLocalStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //Blazor viene renderizzato una volta sul server e una sul client. Dato che sul server non è presente il local storage, inserirsco il try-catch
            //per evitare che venga lanciata eccezione quando si renderizza sul server
            try
            {
                LoginResponse sessionModel = (await _localStorage.GetAsync<LoginResponse>("SessionState")).Value!;

                ClaimsIdentity identity;

                if(sessionModel == null)
                {
                    identity = new ClaimsIdentity();
                }
                else
                {
                    identity = GetClaimsIdentity(sessionModel?.JwtToken ?? throw new Exception());
                }

                ClaimsPrincipal user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch (InvalidOperationException)
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }
        }

        private ClaimsIdentity GetClaimsIdentity(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

            List<Claim> claims = jwtToken.Claims.ToList();

            return new ClaimsIdentity(claims, "Jwt");
        }

        public async Task NotifyUserLogin(LoginResponse response)
        {
            await _localStorage.SetAsync("SessionState", response);

            ClaimsIdentity identity = GetClaimsIdentity(response.JwtToken!);

            ClaimsPrincipal user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task NotifyUserLogout()
        {
            await _localStorage.DeleteAsync("SessionState");

            ClaimsIdentity identity = new ClaimsIdentity();

            ClaimsPrincipal user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
