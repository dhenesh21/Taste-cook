using Taste_cook.Data;
using Taste_cook.DTOs;
using Taste_cook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Taste_cook.Controllers;

[ApiController]
[Route("api/incident")]
[Authorize]
public class IncidentController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public IncidentController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> ReportIncident(IncidentReportDto dto)
    {
        int reporterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var report = new IncidentReport
        {
            BookingId = dto.BookingId,
            ReporterId = reporterId,
            Type = dto.Type,
            Description = dto.Description
        };

        _db.IncidentReports.Add(report);
        await _db.SaveChangesAsync();

        return Ok("Incident reported successfully");
    }
}
