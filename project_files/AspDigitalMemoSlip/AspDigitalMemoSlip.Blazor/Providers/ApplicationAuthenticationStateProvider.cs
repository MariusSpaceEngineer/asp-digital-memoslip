using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace AspDigitalMemoSlip.Blazor.Providers
{
    public class ApplicationAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;

        public ApplicationAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            ClaimsIdentity identity;

            if (!string.IsNullOrEmpty(token))
            {
                // If the token exists, create a ClaimsIdentity
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, token)
                }, "jwt");
            }
            else
            {
                // If the token doesn't exist, create an empty ClaimsIdentity
                identity = new ClaimsIdentity();
            }

            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        public void NotifyUserAuthentication(string username)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, "apiauth_type"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }


    }
}
