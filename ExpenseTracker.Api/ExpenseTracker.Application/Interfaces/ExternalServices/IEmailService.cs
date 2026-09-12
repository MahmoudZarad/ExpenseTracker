using ExpenseTracker.Application.Common.Email;

namespace ExpenseTracker.Application.Interfaces.ExternalServices
{
    public interface IEmailService
    {
        public Task SendEmailAsync(EmailDto emailDto);
        public Task SendEmailverificationAsync(string email, string verificationToken, string EndPoint, string subject, string FullName = "Dear customer");
        public Task SendCodeverificationAsync(string email, string Codeverification, string subject, string FullName = "Dear customer");
    }
}
