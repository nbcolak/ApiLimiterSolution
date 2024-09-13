namespace ApiLimiter.Decorator;

public class WithIPRateLimiterDecorator:IRateLimiter
{

    private readonly IRateLimiter _rateLimiter;

    public WithIPRateLimiterDecorator(IRateLimiter rateLimiter)
    {
        _rateLimiter = rateLimiter;
    }
    public async Task<bool> IsLimitReached(string key)
    {
        // IP bazlı rate limiting kontrolü
        var ipBasedKey = "IP_" + key;
        return await _rateLimiter.IsLimitReached(ipBasedKey);
    }
}