using DotNet8Auth.Shared.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet8Auth.API.Controllers.WeatherForecast;

[Authorize(Roles = UserRoles.Admin)]
[ApiController]
[Route("[controller]")]
public class WeatherForecastAdminController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastAdminController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecastAdmin")]
    public IEnumerable<Shared.Models.WeatherForecast.WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new Shared.Models.WeatherForecast.WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}