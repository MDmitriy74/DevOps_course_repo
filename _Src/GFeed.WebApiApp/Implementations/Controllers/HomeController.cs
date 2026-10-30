using GFeed.WebApiApp.Implementations.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GFeed.WebApiApp.Implementations.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    private static readonly DateTimeOffset StartedAt = DateTimeOffset.UtcNow;
    private static int _livenessFailed;
    private static int _readinessFailed;

    [HttpGet]
    public AppHome GetHome()
    {
        var message = Environment.GetEnvironmentVariable("APP_MESSAGE")
            ?? GetMessage.GetJsonHomeMessage();
        var tokenConfigured = !string.IsNullOrWhiteSpace(
            Environment.GetEnvironmentVariable("APP_TOKEN"));
        var author = Environment.GetEnvironmentVariable("APP_AUTHOR") ?? "Dima";
        var podName = Environment.GetEnvironmentVariable("POD_NAME")
            ?? Environment.MachineName;

        return new AppHome(message, author, tokenConfigured, podName);
    }

    [HttpGet("webapi/home")]
    public JsonHome GetLegacyHome()
    {
        var message = GetMessage.GetJsonHomeMessage();
        return new JsonHome(message);
    }

    [HttpGet("live")]
    public IActionResult GetLive()
    {
        if (IsWithinStartupPeriod() || Volatile.Read(ref _livenessFailed) == 1)
            return StatusCode(StatusCodes.Status503ServiceUnavailable);

        return Ok();
    }

    [HttpGet("ready")]
    public IActionResult GetReady()
    {
        if (IsWithinStartupPeriod() || Volatile.Read(ref _readinessFailed) == 1)
            return StatusCode(StatusCodes.Status503ServiceUnavailable);

        return Ok();
    }

    [HttpGet("you-are-death")]
    public IActionResult FailLiveness()
    {
        Interlocked.Exchange(ref _livenessFailed, 1);
        return Ok();
    }

    [HttpGet("not-ready")]
    public IActionResult FailReadiness()
    {
        Interlocked.Exchange(ref _readinessFailed, 1);
        return Ok();
    }

    [HttpGet("ready-again")]
    public IActionResult RestoreReadiness()
    {
        Interlocked.Exchange(ref _readinessFailed, 0);
        return Ok();
    }

    private static bool IsWithinStartupPeriod()
    {
        return DateTimeOffset.UtcNow - StartedAt < TimeSpan.FromSeconds(10);
    }

    public record AppHome(string Message, string Author, bool TokenConfigured, string PodName);
    public record JsonHome(string Message);
}