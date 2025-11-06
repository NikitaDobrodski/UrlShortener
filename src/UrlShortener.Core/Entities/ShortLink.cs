namespace UrlShortener.Core.Entities;

public class ShortLink
{
    public Guid Id { get; set; }
    public string Code { get; set; } = default!;
    public string OriginalUrl { get; set; } = default!;
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = default!;
    public long Clicks { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAtUtc { get; set; }
}
