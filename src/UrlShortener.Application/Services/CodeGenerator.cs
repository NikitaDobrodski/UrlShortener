using System.Security.Cryptography;
using UrlShortener.Application.Abstractions;

namespace UrlShortener.Application.Services;

public class CodeGenerator : ICodeGenerator
{
    public string Generate()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Span<char> buffer = stackalloc char[8];

        for (int i = 0; i < buffer.Length; i++)
            buffer[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];

        return new string(buffer);
    }
}
