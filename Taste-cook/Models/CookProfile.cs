using System.ComponentModel.DataAnnotations;

namespace Taste_cook.Models
{
    public class CookProfile
    {
        [Key]
        public int CookId { get; set; }
        public int UserId { get; set; }

        public string Cuisines { get; set; } = "";
        public int ExperienceYears { get; set; }
        public decimal ChargesPerMeal { get; set; }
        public bool IsAvailable { get; set; }

        public User User { get; set; } = null!;
    }
}
