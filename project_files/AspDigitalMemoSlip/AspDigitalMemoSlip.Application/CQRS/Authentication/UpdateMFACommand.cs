using AspDigitalMemoSlip.Application.Exceptions;
using AspDigitalMemoSlip.Application.Exceptions.Authentication;
using AspDigitalMemoSlip.Application.Exceptions.Email;
using AspDigitalMemoSlip.Application.Exceptions.Multi_Factor;
using AspDigitalMemoSlip.Application.Interfaces.Services;
using AspDigitalMemoSlip.Domain;
using DTOClassLibrary.DTO.Authentication;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace AspDigitalMemoSlip.Application.CQRS.Authentication
{
    public class UpdateMFACommand : IRequest<AuthResult>
    {
        public string UserId { get; set; }

        public UpdateMFACommand(string userId)
        {

            UserId = userId;
        }
    }

    public class UpdateMFACommandHandler : IRequestHandler<UpdateMFACommand, AuthResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenHelper _tokenHelper;
        private readonly IUserTwoFactorTokenProvider<User> _tokenProvider;
        private readonly IEmailService _emailService;

        public UpdateMFACommandHandler(UserManager<User> userManager, TokenHelper tokenHelper, IUserTwoFactorTokenProvider<User> tokenProvider, IEmailService emailService) // Add IMapper mapper to the constructor parameters
        {
            _userManager= userManager;
            _tokenHelper= tokenHelper;
            _tokenProvider = tokenProvider;
            _emailService = emailService;
        }

        public async Task<AuthResult> Handle(UpdateMFACommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new UserNotFoundException("User does not exist");

            // Toggle the TwoFactorEnabled property
            user.TwoFactorEnabled = !user.TwoFactorEnabled;

            string qrCode = null;
            if (user.TwoFactorEnabled)
            {
                var totp = await GenerateTotp(user);
                qrCode = _tokenHelper.GenerateQrCode(totp);

                if (qrCode == null)
                {
                    throw new QrCodeGenerationException("Error on the server side.");
                }
            }
            else
            {
                user.TwoFactorSecretKey = null;
            }

            // Update the user in the database with the value for the TwoFactorSecretKey (either a value or null)
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new UpdateFailedException("Failed to update Multi-Factor Authentication status.");
            }
            else
            {
                if (user.TwoFactorEnabled)
                {
                    await SendQrCodeThroughMail(user.Email, user.Name, qrCode);
                    return new AuthResult(StatusCodes.Status200OK, qrCode);
                }
                else
                {
                    await NotifyMFADeactivation(user.Email, user.Name);
                    return new AuthResult(StatusCodes.Status200OK, "Multi-Factor Authentication is disabled.");
                }
            } 
        }


        private async Task<string> GenerateTotp(User user)
        {
            try
            {
                return await _tokenProvider.GenerateAsync("TwoFactor", _userManager, user);
            }
            catch (Exception ex)
            {
                throw new TotpGenerationException("An error occurred while generating the TOTP", ex);
            }
        }

        private async Task NotifyMFADeactivation(string to, string recipientName)
        {
            try
            {
                string subject = "You have deactivated Multi-Factor Authentication";
                string body = "You have just deactivated Multi-Factor Authentication on your account.";

                await _emailService.SendEmailAsync(to, recipientName, subject, body);
            }
            catch (Exception ex)
            {
                throw new EmailSendingException("An error occurred while sending the Multi-Factor Authentication deactivation email", ex);
            }
        }

        private async Task SendQrCodeThroughMail(string to, string recipientName, string qrCode)
        {
            try
            {
                string subject = "You have activated Multi-Factor Authentication";
                string body = "You have just activated Multi-Factor Authentication on your account, in the attachments you find the qr code needed to link your account with Microsoft Authenticator.";

                // Convert the Base64 string to a byte array
                byte[] qrCodeBytes = Convert.FromBase64String(qrCode);

                await _emailService.SendEmailWithAttachmentAsync(to, recipientName, subject, body, qrCodeBytes, "image/png", "qrcode.png");
            }
            catch (Exception ex)
            {
                throw new EmailSendingException("An error occurred while sending the QR code email", ex);
            }
        }

    }

}
