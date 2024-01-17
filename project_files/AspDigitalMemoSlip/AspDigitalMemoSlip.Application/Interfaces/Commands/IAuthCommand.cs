using AspDigitalMemoSlip.Domain;
using System.Security.Claims;

namespace AspDigitalMemoSlip.Application.Interfaces.Commands
{
    public interface IAuthCommand
    {
        Task<List<Claim>> GenerateClaimsAsync(User user);
    }

}
