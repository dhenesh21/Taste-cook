using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Taste_cook.Data;
using Taste_cook.DTOs;
using Taste_cook.Models;
using Taste_cook.Services;

namespace Taste_cook.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly JwtService _jwt;

        public AuthController(ApplicationDbContext db, JwtService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(x => x.MobileNumber == dto.MobileNumber))
                return BadRequest("User already exists");

            var user = new User
            {
            FullName = dto.FullName,
            MobileNumber = dto.MobileNumber,
            PasswordHash = HashPassword(dto.Password),
            Role = dto.Role
    };

    _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok("Registered successfully");
}

[HttpPost("login")]
public async Task<IActionResult> Login(LoginDto dto)
{
    var user = await _db.Users
        .FirstOrDefaultAsync(x => x.MobileNumber == dto.MobileNumber);

    if (user == null || user.PasswordHash != HashPassword(dto.Password))
        return Unauthorized("Invalid credentials");

    var token = _jwt.GenerateToken(user);

    return Ok(new { token, role = user.Role });
}

private static string HashPassword(string password)
{
    using var sha = SHA256.Create();
    var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
    return Convert.ToBase64String(bytes);
}
    }
}
