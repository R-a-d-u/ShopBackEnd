using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using ShopBackEnd.Services;
using System;
using System.Threading.Tasks;

namespace ShopBackEnd.Service
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _apiKey = configuration["SendGrid:ApiKey"];
            _logger = logger;
        }

        public async Task<bool> SendConfirmationEmailAsync(string toEmail, string confirmationToken)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress("luxuryandgold111@gmail.com", "LuxuryAndGold");
            var to = new EmailAddress(toEmail);
            var subject = "Confirm Your Email Address";

            var plainTextContent = $"Please confirm your email by using this token: {confirmationToken}";
            var confirmationLink = $"http://localhost:4200/confirm-email?token={confirmationToken}";
            var htmlContent = $@"
  <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
      <h2 style='color: #333;'>Welcome to <span style='color: #c19a6b;'>Luxury & Gold</span>!</h2>
      <p style='font-size: 16px;'>Thank you for registering. Please confirm your email address to complete your registration.</p>
      <p style='margin: 20px 0;'>
          <a href='{confirmationLink}' style='display: inline-block; padding: 10px 20px; background-color: #c19a6b; color: white; text-decoration: none; border-radius: 5px;'>Confirm Email</a>
      </p>
      <p>If the button doesn't work, copy and paste this link into your browser:</p>
      <p><a href='{confirmationLink}'>{confirmationLink}</a></p>
      <p>Best regards,<br><strong>The Luxury&Gold Team</strong></p>
  </div>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            try
            {
                _logger.LogInformation("Attempting to send email to {Email}", toEmail);
                var response = await client.SendEmailAsync(msg);
                _logger.LogInformation("Email response status code: {StatusCode}", response.StatusCode);
                return response.StatusCode == System.Net.HttpStatusCode.Accepted ||
                       response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", toEmail);
                return false;
            }
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string content)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress("luxuryandgold111@gmail.com", "LuxuryAndGold");
            var to = new EmailAddress(toEmail);

            var htmlContent = $@"
    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #ffffff;'>
        <h2 style='color: #333;'>{subject}</h2>
        <div style='font-size: 16px; color: #444;'>{content}</div>
        <br/>
        <p style='font-size: 14px; color: #888;'>This email was sent by LuxuryAndGold.</p>
    </div>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, htmlContent);

            try
            {
                _logger.LogInformation("Sending email to {Email} with subject: {Subject}", toEmail, subject);
                var response = await client.SendEmailAsync(msg);
                _logger.LogInformation("Email response status code: {StatusCode}", response.StatusCode);

                return response.StatusCode == System.Net.HttpStatusCode.Accepted ||
                       response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", toEmail);
                return false;
            }
        }

        public async Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetToken)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress("luxuryandgold111@gmail.com", "LuxuryAndGold");
            var to = new EmailAddress(toEmail);
            var subject = "Reset Your Password";

            var plainTextContent = $"To reset your password, please use this token: {resetToken}";

            var resetLink = $"http://localhost:4200/profile/edit-password?token={resetToken}";
            var htmlContent = $@"
    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #ffffff;'>
        <h2 style='color: #c0392b;'>Password Reset Request</h2>
        <p style='font-size: 16px;'>We received a request to reset your password for your <strong>LuxuryAndGold</strong> account.</p>
        <p style='margin: 20px 0;'>
            <a href='{resetLink}' style='display: inline-block; padding: 10px 20px; background-color: #c0392b; color: white; text-decoration: none; border-radius: 5px;'>Reset Password</a>
        </p>
        <p>If the button doesn't work, copy and paste this link into your browser:</p>
        <p><a href='{resetLink}'>{resetLink}</a></p>
        <p>If you didn’t request this, you can safely ignore this email.</p>
        <p>Best regards,<br><strong>The LuxuryAndGold Team</strong></p>
    </div>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            try
            {
                _logger.LogInformation("Sending password reset email to {Email}", toEmail);
                var response = await client.SendEmailAsync(msg);
                _logger.LogInformation("Password reset email response status code: {StatusCode}", response.StatusCode);

                return response.StatusCode == System.Net.HttpStatusCode.Accepted ||
                       response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password reset email to {Email}", toEmail);
                return false;
            }
        }
    }
}