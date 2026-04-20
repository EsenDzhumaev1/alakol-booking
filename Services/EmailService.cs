using Alakol.Models;
using Alakol.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Alakol.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendBookingNotificationAsync(string name, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(_settings.SenderEmail) ||
            string.IsNullOrWhiteSpace(_settings.SenderPassword) ||
            string.IsNullOrWhiteSpace(_settings.AdminEmail) ||
            string.IsNullOrWhiteSpace(_settings.SmtpServer))
        {
            return;
        }

        try
        {
            using var message = new MailMessage(_settings.SenderEmail, _settings.AdminEmail)
            {
                Subject = "New Booking Request",
                Body = $"New booking request received:{Environment.NewLine}{Environment.NewLine}Name: {name}{Environment.NewLine}Email: {email}{Environment.NewLine}Phone: {phone}"
            };

            using var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_settings.SenderEmail, _settings.SenderPassword)
            };

            await client.SendMailAsync(message);
        }
        catch
        {
        }
    }
}
