using AspDigitalMemoSlip.Application.Interfaces.Services.Authentication;
using AspDigitalMemoSlip.Domain;
using Microsoft.AspNetCore.Identity;

namespace AspDigitalMemoSlip.Application.Services.Authentication
{
    public class AuthService : IAuthService
    {
        protected readonly UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager)
        {
            _userManager = userManager;

        }

        public async Task<bool> DoesUserExistAsync(string username, string email)
        {
            var userExists = await _userManager.FindByNameAsync(username) ?? await _userManager.FindByEmailAsync(email);
            return userExists != null;
        }

    }
}
