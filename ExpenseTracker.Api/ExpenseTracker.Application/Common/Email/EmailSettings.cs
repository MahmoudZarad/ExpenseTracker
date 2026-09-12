namespace ExpenseTracker.Application.Common.Email
{
    public class EmailSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string SmtpServer { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; }
        public int Port { get; set; }
        public string DomainEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
