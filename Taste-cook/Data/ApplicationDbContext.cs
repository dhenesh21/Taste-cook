using Microsoft.EntityFrameworkCore;
using Taste_cook.Models;
namespace Taste_cook.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<CookProfile> CookProfiles => Set<CookProfile>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<SOSAlert> SOSAlerts => Set<SOSAlert>();
        public DbSet<IncidentReport> IncidentReports => Set<IncidentReport>();
        public DbSet<Payment> Payments => Set<Payment>();
    }
}
