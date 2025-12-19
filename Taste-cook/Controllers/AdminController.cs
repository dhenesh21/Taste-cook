using Taste_cook.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Taste_cook.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public AdminController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("sos")]
    public IActionResult GetActiveSOS()
    {
        var alerts = _db.SOSAlerts
            .Where(x => !x.IsResolved)
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

        return Ok(alerts);
    }

    [HttpPost("resolve-sos/{id}")]
    public async Task<IActionResult> ResolveSOS(int id)
    {
        var sos = await _db.SOSAlerts.FindAsync(id);
        if (sos == null) return NotFound();

        sos.IsResolved = true;
        await _db.SaveChangesAsync();

        return Ok("SOS resolved");
    }

    [HttpGet("incidents")]
    public IActionResult GetIncidents()
    {
        return Ok(_db.IncidentReports.ToList());
    }
}
