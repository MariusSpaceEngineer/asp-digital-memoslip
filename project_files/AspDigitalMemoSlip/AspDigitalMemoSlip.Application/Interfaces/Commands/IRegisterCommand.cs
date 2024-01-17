using AspDigitalMemoSlip.Application.CQRS.Authentication;
using AspDigitalMemoSlip.Domain;

namespace AspDigitalMemoSlip.Application.Interfaces.Commands
{
    public interface IRegisterCommand
    {
            Task<User> CreateUserAsync(RegisterCommand command);
    }
}
