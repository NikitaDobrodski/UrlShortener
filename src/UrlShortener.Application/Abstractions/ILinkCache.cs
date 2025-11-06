using UrlShortener.Core.Entities;

namespace UrlShortener.Application.Abstractions;

public interface ILinkCache
{
    Task<ShortLink?> GetAsync(string code);
    Task SetAsync(ShortLink link);
}
