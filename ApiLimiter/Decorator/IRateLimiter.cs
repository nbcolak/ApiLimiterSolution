namespace ApiLimiter.Decorator;

public interface IRateLimiter
{
    Task<bool> IsLimitReached(string key);
}