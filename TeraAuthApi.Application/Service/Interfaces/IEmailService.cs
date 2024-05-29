namespace TeraAuthApi.Application.Service.Interfaces;

public interface IEmailService
{
    Task<bool> SendNewPasswordEmailAsync(string toEmail, string newPassword,
        CancellationToken cancellationToken = default);
}