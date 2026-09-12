namespace ExpenseTracker.Application.DTOs
{
    public class UserProfileDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Currency { get; set; } = "EGP";

        public string Language { get; set; } = "English";
    }
}
