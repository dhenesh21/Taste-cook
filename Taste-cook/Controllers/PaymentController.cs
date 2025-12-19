using Taste_cook.Data;
using Taste_cook.DTOs;
using Taste_cook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace Taste_cook.Controllers;

[ApiController]
[Route("api/payment")]
[Authorize(Roles = "Customer")]
public class PaymentController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ApplicationDbContext _db;

    public PaymentController(IConfiguration config, ApplicationDbContext db)
    {
        _config = config;
        _db = db;
    }

    [HttpPost("create-order")]
    public IActionResult CreateOrder(CreatePaymentDto dto)
    {
        var booking = _db.Bookings.Find(dto.BookingId);
        if (booking == null) return NotFound();

        RazorpayClient client = new(
            _config["Razorpay:Key"],
            _config["Razorpay:Secret"]
        );

        var options = new Dictionary<string, object>
        {
            { "amount", booking.TotalAmount * 100 }, // paise
            { "currency", "INR" },
            { "receipt", $"booking_{booking.BookingId}" }
        };

        Order order = client.Order.Create(options);

        var payment = new Taste_cook.Models.Payment
        {
            BookingId = booking.BookingId,
            RazorpayOrderId = order["id"].ToString(),
            Amount = booking.TotalAmount
        };

        _db.Payments.Add(payment);
        _db.SaveChanges();

        return Ok(new
        {
            orderId = order["id"],
            amount = booking.TotalAmount,
            key = _config["Razorpay:Key"]
        });
    }

    [HttpPost("verify")]
    public IActionResult VerifyPayment(VerifyPaymentDto dto)
    {
        var payment = _db.Payments
            .FirstOrDefault(x => x.RazorpayOrderId == dto.RazorpayOrderId);

        if (payment == null) return BadRequest("Payment not found");

        string payload = dto.RazorpayOrderId + "|" + dto.RazorpayPaymentId;
        string secret = _config["Razorpay:Secret"]!;

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        var signature = BitConverter.ToString(hash).Replace("-", "").ToLower();

        if (signature != dto.RazorpaySignature)
            return BadRequest("Invalid signature");

        payment.RazorpayPaymentId = dto.RazorpayPaymentId;
        payment.RazorpaySignature = dto.RazorpaySignature;
        payment.Status = "Paid";

        var booking = _db.Bookings.Find(payment.BookingId);
        booking!.Status = "Paid";

        _db.SaveChanges();

        return Ok("Payment verified successfully");
    }
}
