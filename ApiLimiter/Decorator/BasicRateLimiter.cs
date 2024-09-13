using StackExchange.Redis;

namespace ApiLimiter.Decorator;
public class BasicRateLimiter : IRateLimiter
{
    private readonly IConnectionMultiplexer _redis;
    private readonly int _requestLimit;   
    private readonly TimeSpan _timeSpan;   

    public BasicRateLimiter(IConnectionMultiplexer redis, int requestLimit, TimeSpan timeSpan)
    {
        _redis = redis;
        _requestLimit = requestLimit;
        _timeSpan = timeSpan;
    }

    public async Task<bool> IsLimitReached(string key)
    {
        var db = _redis.GetDatabase();
        var requestCount = await db.StringGetAsync(key);

        if (requestCount.IsNullOrEmpty)
        {
            await db.StringSetAsync(key, "1", _timeSpan);
            return false;
        }
        else if (int.Parse(requestCount) >= _requestLimit)
        {
            return true;
        }
        else
        {
            await db.StringIncrementAsync(key);
            return false;
        }
    }
}