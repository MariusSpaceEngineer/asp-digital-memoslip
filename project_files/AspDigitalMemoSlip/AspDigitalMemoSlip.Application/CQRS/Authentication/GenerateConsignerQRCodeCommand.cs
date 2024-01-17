using AspDigitalMemoSlip.Application.Exceptions;
using AspDigitalMemoSlip.Application.Exceptions.Consigner;
using AspDigitalMemoSlip.Application.Exceptions.Email;
using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Application.Interfaces.Services;
using DTOClassLibrary.DTO.Consigner;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace AspDigitalMemoSlip.Application.CQRS.Authentication
{
    public class GenerateConsignerQRCodeCommand : IRequest<QRCodeResult>
    {
        public string ConsignerId { get; set; }

        public GenerateConsignerQRCodeCommand(string consignerId)
        {
            ConsignerId = consignerId;
        }
    }

    public class GenerateConsignerQRCodeCommandHandler : IRequestHandler<GenerateConsignerQRCodeCommand, QRCodeResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly TokenHelper _tokenHelper;
        private readonly IEmailService _emailService;


        public GenerateConsignerQRCodeCommandHandler(IUnitOfWork uow, IConfiguration configuration, TokenHelper tokenHelper, IEmailService emailService)
        {
            _uow = uow;
            _configuration = configuration;
            _tokenHelper = tokenHelper;
            _emailService = emailService;
        }

        public async Task<QRCodeResult> Handle(GenerateConsignerQRCodeCommand command, CancellationToken cancellationToken)
        {
            var consigner = await GetConsigner(command.ConsignerId);
            var blazorUrl = GetBlazorUrl();

            var url = $"{blazorUrl}/{consigner.UserName}/register";
            var qrCode = _tokenHelper.GenerateQrCode(url);

            if (qrCode == null)
            {
                throw new QrCodeGenerationException("Error on the server side.");
            }
            await SendQrCodeAndUrlThroughMail(consigner.Email, consigner.Name, qrCode, url);
            return new QRCodeResult(url, qrCode);
        }

        private async Task<Domain.Consigner> GetConsigner(string consignerId)
        {
            var consigner = await _uow.ConsignerRepository.GetById(consignerId);
            if (consigner == null)
            {
                throw new ConsignerNotFoundException("Consigner not found! Please check the consigner username and try again.");
            }

            return consigner;
        }

        private string GetBlazorUrl()
        {
            var blazorUrl = _configuration["ClientUrls:Blazor"];
            if (string.IsNullOrEmpty(blazorUrl))
            {
                throw new ConfigurationNotFoundException("Blazor URL not found in app settings! Please check your configuration and try again.");
            }

            return blazorUrl;
        }

        private async Task SendQrCodeAndUrlThroughMail(string to, string recipientName, string qrCode, string url)
        {
            try
            {
                string subject = "Your QR Code";
                string body = $"Attached is the QR code that you can distribute to your consignees. This will enable them to create an account and commence trading with you! You can also click on the following link to access the registration page: <a href='{url}'>{url}</a>";

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
