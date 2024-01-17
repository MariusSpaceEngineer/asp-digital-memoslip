using AspDigitalMemoSlip.Application.CQRS.Authentication;
using AspDigitalMemoSlip.Application.Interfaces.Services.Authentication;
using AspDigitalMemoSlip.Domain;
using Microsoft.AspNetCore.Identity;

namespace AspDigitalMemoSlip.Application.Services.Authentication
{
    public class RegisterService : AuthService, IRegisterService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : base(userManager)
        {
            _roleManager = roleManager;

        }

        public async Task AssignRolesAsync(User user, IList<string> roles)
        {
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));

                if (await _roleManager.RoleExistsAsync(role))
                    await _userManager.AddToRoleAsync(user, role);
            }
        }

    }
}
