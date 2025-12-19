namespace Taste_cook.Models
{
    public class IncidentReport
    {
        public int IncidentReportId { get; set; }

        public int BookingId { get; set; }
        public int ReporterId { get; set; }

        public string Type { get; set; } = "";
        // Harassment | Abuse | Unsafe | Payment

        public string Description { get; set; } = "";
        public string Status { get; set; } = "Open";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
