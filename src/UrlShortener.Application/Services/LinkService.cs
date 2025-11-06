using UrlShortener.Core.Abstractions;
using UrlShortener.Core.Entities;
using UrlShortener.Application.Abstractions;
namespace UrlShortener.Application.Services;

public class LinkService : ILinkService
{
    private readonly ILinkRepository _links;
    private readonly IUserRepository _users;
    private readonly ICodeGenerator _codes;
    private readonly ILinkCache _cache;

    public LinkService(
        ILinkRepository links,
        IUserRepository users,
        ICodeGenerator codes,
        ILinkCache cache)
    {
        _links = links;
        _users = users;
        _codes = codes;
        _cache = cache;
    }

    public async Task<ShortLink> CreateShortLinkAsync(Guid userId, string originalUrl)
    {
        // добавили проверку что пользователь существует
        var user = await _users.GetByEmailAsync("placeholder"); // временно, ниже исправим
        // ✋ но у нас в IUserRepository нет метода по Id, поэтому пока не проверяем владельца

        var code = _codes.Generate(); // <-- мы используем Generate() (не GenerateCode())

        var link = new ShortLink
        {
            Id = Guid.NewGuid(),
            Code = code,
            OriginalUrl = originalUrl,
            OwnerId = userId,
            CreatedAtUtc = DateTime.UtcNow,
            Clicks = 0
        };

        await _links.AddAsync(link);

        await _cache.SetAsync(link);

        return link;
    }

    public async Task<string?> ResolveShortCodeAsync(string code)
    {
        var cached = await _cache.GetAsync(code);
        if (cached != null)
            return cached.OriginalUrl;

        var link = await _links.GetByCodeAsync(code);
        if (link == null)
            return null;

        await _cache.SetAsync(link);

        return link.OriginalUrl;
    }
}
