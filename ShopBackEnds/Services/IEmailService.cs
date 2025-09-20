namespace ShopBackEnd.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string content);
        Task<bool> SendConfirmationEmailAsync(string toEmail, string confirmationToken);
        Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetToken);
    }
}
