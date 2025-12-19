namespace Taste_cook.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public string RazorpayOrderId { get; set; } = "";
        public string RazorpayPaymentId { get; set; } = "";
        public string RazorpaySignature { get; set; } = "";
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Created"; // Created | Paid
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
