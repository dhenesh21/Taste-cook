namespace Taste_cook.Models
{
    public class SOSAlert
    {
        public int SOSAlertId { get; set; }

        public int BookingId { get; set; }
        public int CookId { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool IsResolved { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
