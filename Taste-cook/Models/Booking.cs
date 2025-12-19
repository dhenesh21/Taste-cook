namespace Taste_cook.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int CookId { get; set; }

        public DateTime BookingDate { get; set; }
        public string MealType { get; set; } = "";
        public int PersonsCount { get; set; }
        public bool CleaningRequired { get; set; }

        public string Status { get; set; } = "Pending";
        // Pending | Accepted | Completed | Cancelled

        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
