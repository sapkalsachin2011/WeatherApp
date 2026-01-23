using Microsoft.AspNetCore.Mvc;
using WeatherApp.Api.Utils;

namespace WeatherApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly string _datesFilePath = Path.Combine("..", "..", "Shared", "dates.txt");
        private readonly Services.WeatherService _weatherService;

        public WeatherController(Services.WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("dates")]
        public IActionResult GetParsedDates()
        {
            if (!System.IO.File.Exists(_datesFilePath))
                return NotFound("dates.txt not found");

            var lines = System.IO.File.ReadAllLines(_datesFilePath);
            var results = new List<object>();
            foreach (var line in lines)
            {
                var (date, error) = Utils.DateParser.ParseDate(line);
                results.Add(new
                {
                    Original = line,
                    Normalized = date?.ToString("yyyy-MM-dd"),
                    Error = error
                });
            }
            return Ok(results);
        }

        [HttpGet]
        public async Task<IActionResult> GetWeather()
        {
            if (!System.IO.File.Exists(_datesFilePath))
                return NotFound("dates.txt not found");

            var lines = System.IO.File.ReadAllLines(_datesFilePath);
            var tasks = new List<Task<Models.WeatherResult>>();
            foreach (var line in lines)
            {
                var (date, error) = Utils.DateParser.ParseDate(line);
                if (date != null && error == null)
                {
                    var isoDate = date.Value.ToString("yyyy-MM-dd");
                    tasks.Add(_weatherService.GetWeatherForDateAsync(isoDate));
                }
                else
                {
                    tasks.Add(Task.FromResult(new Models.WeatherResult
                    {
                        Date = line,
                        Error = error
                    }));
                }
            }
            var results = await Task.WhenAll(tasks);
            return Ok(results);
        }
    }
}
