using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email is required.", nameof(email));
        if (string.IsNullOrEmpty(subject)) throw new ArgumentException("Subject is required.", nameof(subject));
        if (string.IsNullOrEmpty(htmlMessage)) throw new ArgumentException("Message body is required.", nameof(htmlMessage));

        if (!int.TryParse(_configuration["EmailSettings:SMTPPort"], out int port))
        {
            throw new InvalidOperationException("Invalid SMTP port configuration.");
        }

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(
            _configuration["EmailSettings:SenderName"],
            _configuration["EmailSettings:SenderEmail"]));
        emailMessage.To.Add(MailboxAddress.Parse(email));
        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlMessage,
            TextBody = "Your email client does not support HTML messages."
        };
        emailMessage.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_configuration["EmailSettings:SMTPHost"], port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(
                _configuration["EmailSettings:SenderEmail"],
                _configuration["EmailSettings:SenderPassword"]);
            await client.SendAsync(emailMessage);
        }
        catch (Exception ex)
        {
            // Log error
            Console.WriteLine($"Error sending email: {ex.Message}");
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}
