using AspDigitalMemoSlip.Application.Exceptions.Email;
using AspDigitalMemoSlip.Application.Interfaces.Services;
using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Extensions.Configuration;

public class MailjetEmailService : IEmailService
{
    private readonly MailjetClient _client;
    private readonly string _fromName;
    private readonly string _fromEmail;

    public MailjetEmailService(IConfiguration configuration)
    {
        var apiKey = configuration["Mailjet:ApiKey"];
        var apiSecret = configuration["Mailjet:ApiSecret"];
        _fromEmail = configuration["Mailjet:E-mail"];
        _fromName = configuration["Mailjet:Name"];

        _client = new MailjetClient(apiKey, apiSecret);
    }

    public async Task SendEmailAsync(string to, string recipientName, string subject, string body)
    {
        try
        {
            var email = new TransactionalEmailBuilder()
                .WithFrom(new SendContact(_fromEmail, _fromName))
                .WithTo(new SendContact(to, recipientName))
                .WithSubject(subject)
                .WithHtmlPart($"{body}")
                .Build();

            var response = await _client.SendTransactionalEmailAsync(email);
        }
        catch (Exception ex)
        {
            throw new EmailSendingException("An error occurred while sending the email", ex);
        }
    }

    public async Task SendEmailWithAttachmentAsync(string to, string recipientName, string subject, string body, byte[] attachmentData, string contentType, string attachmentName)
    {
        try
        {
            var attachment = new Attachment(attachmentName, contentType, Convert.ToBase64String(attachmentData));
            var attachments = new List<Attachment> { attachment };

            var email = new TransactionalEmailBuilder()
                .WithFrom(new SendContact(_fromEmail, _fromName))
                .WithTo(new SendContact(to, recipientName))
                .WithSubject(subject)
                .WithHtmlPart($"{body}")
                .WithAttachments(attachments)
                .Build();

            var response = await _client.SendTransactionalEmailAsync(email);
        }
        catch (Exception ex)
        {
            throw new EmailSendingException("An error occurred while sending the email", ex);
        }
    }

}

