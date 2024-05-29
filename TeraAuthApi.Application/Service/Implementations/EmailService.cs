using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using TeraAuthApi.Application.Service.Interfaces;
using TeraAuthApi.Application.Settings;

namespace TeraAuthApi.Application.Service.Implementations;

public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task<bool> SendNewPasswordEmailAsync(string toEmail, string newPassword, 
            CancellationToken cancellationToken = default)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.Username),
                Subject = "Your New Password",
                Body = $"Your new password is: {newPassword}",
                IsBodyHtml = false
            };

            mailMessage.To.Add(toEmail);

            return await SendEmailAsync(mailMessage, cancellationToken: cancellationToken);
        }

        private async Task<bool> SendEmailAsync(MailMessage mailMessage, 
            CancellationToken cancellationToken)
        {
            try
            {
                using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = true
                };

                await client.SendMailAsync(mailMessage, cancellationToken: cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return false;
            }
        }
    }