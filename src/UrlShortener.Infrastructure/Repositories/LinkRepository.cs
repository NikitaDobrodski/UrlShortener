using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Abstractions;
using UrlShortener.Core.Entities;

namespace UrlShortener.Infrastructure.Repositories;

public class LinkRepository : ILinkRepository
{
    private readonly AppDbContext _db;
    public LinkRepository(AppDbContext db) => _db = db;

    public Task<ShortLink?> GetByCodeAsync(string code, CancellationToken ct = default) =>
        _db.ShortLinks.Include(x => x.Owner).FirstOrDefaultAsync(x => x.Code == code, ct);

    public async Task<ShortLink> AddAsync(ShortLink link, CancellationToken ct = default)
    {
        _db.ShortLinks.Add(link);
        await _db.SaveChangesAsync(ct);
        return link;
    }

    public async Task IncrementClicksAsync(Guid id, CancellationToken ct = default)
    {
        await _db.ShortLinks.Where(x => x.Id == id)
            .ExecuteUpdateAsync(u => u.SetProperty(l => l.Clicks, l => l.Clicks + 1), ct);
    }

    public Task<bool> CodeExistsAsync(string code, CancellationToken ct = default) =>
        _db.ShortLinks.AnyAsync(x => x.Code == code, ct);
}
