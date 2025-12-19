
using Microsoft.AspNetCore.Http;
using Taste_cook.Data;
using Taste_cook.DTOs;
using Taste_cook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Taste_cook.Controllers;

[ApiController]
[Route("api/booking")]
[Authorize(Roles = "Customer")]
public class BookingController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public BookingController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking(CreateBookingDto dto)
    {
        int customerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var cook = await _db.CookProfiles.FindAsync(dto.CookId);
        if (cook == null || !cook.IsAvailable)
            return BadRequest("Cook not available");

        var amount = cook.ChargesPerMeal * dto.PersonsCount;

        var booking = new Booking
        {
            CustomerId = customerId,
            CookId = dto.CookId,
            BookingDate = dto.BookingDate,
            MealType = dto.MealType,
            PersonsCount = dto.PersonsCount,
            CleaningRequired = dto.CleaningRequired,
            TotalAmount = amount
        };

        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();

        return Ok(booking);
    }

    [HttpGet("my")]
    public IActionResult MyBookings()
    {
        int customerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var bookings = _db.Bookings
            .Where(x => x.CustomerId == customerId)
            .ToList();

        return Ok(bookings);
    }
}
