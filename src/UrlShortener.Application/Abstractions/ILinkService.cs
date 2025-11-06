using UrlShortener.Core.Entities;

namespace UrlShortener.Application.Abstractions;

public interface ILinkService
{
    Task<ShortLink> CreateShortLinkAsync(Guid userId, string originalUrl);
    Task<string?> ResolveShortCodeAsync(string code);
}
