using StackExchange.Redis;
using System.Text.Json;
using UrlShortener.Core.Entities;
using UrlShortener.Application.Abstractions;

namespace UrlShortener.Infrastructure;

public class RedisLinkCache : ILinkCache
{
    private readonly IDatabase _db;

    public RedisLinkCache(IConnectionMultiplexer multiplexer)
    {
        _db = multiplexer.GetDatabase();
    }

    public async Task<ShortLink?> GetAsync(string code)
    {
        var value = await _db.StringGetAsync(code);
        if (value.IsNullOrEmpty)
            return null;

        return JsonSerializer.Deserialize<ShortLink>(value!)!;
    }

    public async Task SetAsync(ShortLink link)
    {
        await _db.StringSetAsync(link.Code, JsonSerializer.Serialize(link));
    }
}
