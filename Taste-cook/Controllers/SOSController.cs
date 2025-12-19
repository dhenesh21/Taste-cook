

using Taste_cook.Data;
using Taste_cook.DTOs;
using Taste_cook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Taste_cook.Controllers;

[ApiController]
[Route("api/sos")]
[Authorize(Roles = "Cook")]
public class SOSController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public SOSController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpPost("trigger")]
    public async Task<IActionResult> TriggerSOS(SOSRequestDto dto)
    {
        int cookId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var sos = new SOSAlert
        {
            BookingId = dto.BookingId,
            CookId = cookId,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude
        };

        _db.SOSAlerts.Add(sos);
        await _db.SaveChangesAsync();

        // 🔔 Later: SignalR + Firebase alert to admin

        return Ok("SOS triggered. Help is on the way.");
    }
}
