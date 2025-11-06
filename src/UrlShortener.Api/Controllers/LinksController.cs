using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrlShortener.Application.Abstractions;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/links")]
public class LinksController : ControllerBase
{
    private readonly ILinkService _links;

    public LinksController(ILinkService links)
    {
        _links = links;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLinkRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized("Invalid user token"); // Пользователь не найден

        var link = await _links.CreateShortLinkAsync(userId, request.Url);

        return Ok(new { url = $"{Request.Scheme}://{Request.Host}/r/{link.Code}" });
    }
}

public record CreateLinkRequest(string Url);
