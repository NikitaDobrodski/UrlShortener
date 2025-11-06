using System.Security.Cryptography;
using UrlShortener.Application.Abstractions;

namespace UrlShortener.Application.Services;

public class SecurityService : ISecurityService
{
    public string HashPassword(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyPassword(string password, string hash) =>
        BCrypt.Net.BCrypt.Verify(password, hash);
}
