using ApiLimiter.Decorator;

namespace ApiLimiter.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RateLimitingController : ControllerBase
{
    private readonly IRateLimiter _rateLimiter;

    public RateLimitingController(IRateLimiter rateLimiter)
    {
        _rateLimiter = rateLimiter;
    }

    [HttpGet("limited-resource")]
    public async Task<IActionResult> GetLimitedResource()
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        
        if (ipAddress != null && await _rateLimiter.IsLimitReached(ipAddress))
        {
            return StatusCode(429, "Rate limit exceeded. Try again later.");
        }

        return Ok("Here is your limited resource. Access allowed!");
    }

    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        return Ok("Rate limiting system is up and running.");
    }
}