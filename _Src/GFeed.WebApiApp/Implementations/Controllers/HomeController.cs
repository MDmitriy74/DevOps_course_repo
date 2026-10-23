using GFeed.WebApiApp.Implementations.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GFeed.WebApiApp.Implementations.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public AppHome GetHome()
    {
        var message = Environment.GetEnvironmentVariable("APP_MESSAGE")
            ?? GetMessage.GetJsonHomeMessage();
        var tokenConfigured = !string.IsNullOrWhiteSpace(
            Environment.GetEnvironmentVariable("APP_TOKEN"));
        var author = Environment.GetEnvironmentVariable("APP_AUTHOR") ?? "Dima";

        return new AppHome(message, author, tokenConfigured);
    }

    [HttpGet("webapi/home")]
    public JsonHome GetLegacyHome()
    {
        var message = GetMessage.GetJsonHomeMessage();
        return new JsonHome(message);
    }

    public record AppHome(string Message, string Author, bool TokenConfigured);

    public record JsonHome(string Message);
}