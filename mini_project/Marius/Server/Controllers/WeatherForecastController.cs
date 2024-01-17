using Microsoft.AspNetCore.Mvc;
using MiniProject.Shared;
using System.Globalization;

namespace MiniProject.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static List<WeatherForecast> _forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToList();

        public WeatherForecastController()
        {
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _forecasts;
        }

        [HttpGet("{date}")]
        public ActionResult<WeatherForecast> Get(string date)
        {
            if (!DateOnly.TryParseExact(date, "yyyyMMdd", null, DateTimeStyles.None, out DateOnly parsedDate))
            {
                return BadRequest("Invalid date format. Please use the format yyyyMMdd.");
            }

            string reformattedDate = parsedDate.ToString("dd/MM/yyyy");
            var forecast = _forecasts.FirstOrDefault(f => f.Date.ToString("dd/MM/yyyy") == reformattedDate);
            if (forecast == null)
            {
                return NotFound();
            }
            return forecast;
        }
    }
}
