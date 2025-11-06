using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Abstractions;

public interface ILinkRepository
{
    Task<ShortLink?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<ShortLink> AddAsync(ShortLink link, CancellationToken ct = default);
    Task IncrementClicksAsync(Guid id, CancellationToken ct = default);
    Task<bool> CodeExistsAsync(string code, CancellationToken ct = default);
}
