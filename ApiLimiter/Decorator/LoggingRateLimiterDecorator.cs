namespace ApiLimiter.Decorator;

public class LoggingRateLimiterDecorator : IRateLimiter
{
    private readonly IRateLimiter _rateLimiter;

    public LoggingRateLimiterDecorator(IRateLimiter rateLimiter)
    {
        _rateLimiter = rateLimiter;
    }

    public async Task<bool> IsLimitReached(string key)
    {
        var limitReached = await _rateLimiter.IsLimitReached(key);
        if (limitReached)
        {
            Console.WriteLine($"Rate limit reached for {key} at {DateTime.Now}");
        }
        return limitReached;
    }
}