using AspDigitalMemoSlip.Application.Interfaces.Services.Authentication;
using AspDigitalMemoSlip.Domain;
using DTOClassLibrary.DTO.Authentication;
using Microsoft.AspNetCore.Identity;

namespace AspDigitalMemoSlip.Application.Services.Authentication
{
    public class LoginService : AuthService, ILoginService
    {
        protected readonly IUserTwoFactorTokenProvider<User> _tokenProvider;

        public LoginService(UserManager<User> userManager, IUserTwoFactorTokenProvider<User> tokenProvider) : base(userManager)
        {
            _tokenProvider = tokenProvider;
        }

        public async Task<User> ValidateUser(LoginDTO dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null)
                return null;

            if (dto.Password == null && dto.OTCode == null)
            {
                return null;

            }
            else if (dto.Password != null && dto.OTCode == null)
            {
                if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                    return null;
            }
            else if (dto.OTCode != null && dto.Password == null)
            {
                if (!await _tokenProvider.ValidateAsync("TwoFactor", dto.OTCode, _userManager, user))
                    return null;
            }
            else if (dto.Password != null && dto.OTCode != null)
            {
                if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                {
                    if (!await _tokenProvider.ValidateAsync("TwoFactor", dto.OTCode, _userManager, user))
                        return null;
                }
            }

            return user;
        }
    }
}
