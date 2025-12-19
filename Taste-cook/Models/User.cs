namespace Taste_cook.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = "";
        public string MobileNumber { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string Role { get; set; } = "Customer"; // Customer | Cook | Admin
        public bool IsVerified { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
