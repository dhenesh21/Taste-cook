namespace Taste_cook.DTOs
{
    public class CreateBookingDto
    {
        public int CookId { get; set; }
        public DateTime BookingDate { get; set; }
        public string MealType { get; set; } = "";
        public int PersonsCount { get; set; }
        public bool CleaningRequired { get; set; }
    }
}
