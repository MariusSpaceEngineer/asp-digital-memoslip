using AspDigitalMemoSlip.Domain;

namespace AspDigitalMemoSlip.Application.Interfaces.Services.Authentication
{
    public interface IRegisterService : IAuthService
    {
        Task AssignRolesAsync(User user, IList<string> roles);
    }
}
