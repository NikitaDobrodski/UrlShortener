namespace UrlShortener.Application.Abstractions;

public interface ISecurityService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}
