using AspDigitalMemoSlip.Domain;
using DTOClassLibrary.DTO.Authentication;

namespace AspDigitalMemoSlip.Application.Interfaces.Services.Authentication
{
    public interface ILoginService : IAuthService
    {
        Task<User> ValidateUser(LoginDTO dto);
    }
}
