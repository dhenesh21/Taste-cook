
using Taste_cook.Data;
using Taste_cook.DTOs;
using Taste_cook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Taste_cook.Controllers;

[ApiController]
[Route("api/cook")]
[Authorize(Roles = "Cook")]
public class CookController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public CookController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpPost("profile")]
    public async Task<IActionResult> CreateProfile(CookProfileDto dto)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (await _db.CookProfiles.AnyAsync(x => x.UserId == userId))
            return BadRequest("Profile already exists");

        var profile = new CookProfile
        {
            UserId = userId,
            Cuisines = dto.Cuisines,
            ExperienceYears = dto.ExperienceYears,
            ChargesPerMeal = dto.ChargesPerMeal,
            IsAvailable = true
        };

        _db.CookProfiles.Add(profile);
        await _db.SaveChangesAsync();

        return Ok("Cook profile created");
    }

    [HttpGet("bookings")]
    public async Task<IActionResult> MyBookings()
    {
        int cookId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var bookings = await _db.Bookings
            .Where(x => x.CookId == cookId)
            .ToListAsync();

        return Ok(bookings);
    }

    [HttpPost("accept/{bookingId}")]
    public async Task<IActionResult> AcceptBooking(int bookingId)
    {
        var booking = await _db.Bookings.FindAsync(bookingId);
        if (booking == null) return NotFound();

        booking.Status = "Accepted";
        await _db.SaveChangesAsync();

        return Ok("Booking accepted");
    }

    [HttpPost("complete/{bookingId}")]
    public async Task<IActionResult> CompleteBooking(int bookingId)
    {
        var booking = await _db.Bookings.FindAsync(bookingId);
        if (booking == null) return NotFound();

        booking.Status = "Completed";
        await _db.SaveChangesAsync();

        return Ok("Booking completed");
    }
}
