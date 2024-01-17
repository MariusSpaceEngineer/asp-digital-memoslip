namespace AspDigitalMemoSlip.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string recipientName, string subject, string body);
        Task SendEmailWithAttachmentAsync(string to, string recipientName, string subject, string body, byte[] attachmentData, string contentType, string attachmentName);

    }
}
