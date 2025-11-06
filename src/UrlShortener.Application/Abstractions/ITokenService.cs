namespace UrlShortener.Application.Abstractions;

public interface ITokenService
{
    string GenerateToken(Guid userId, string email);
}
