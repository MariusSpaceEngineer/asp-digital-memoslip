namespace AspDigitalMemoSlip.Application.Interfaces.Services.Authentication
{
    public interface IAuthService
    {
        Task<bool> DoesUserExistAsync(string username, string email);
    }

}
