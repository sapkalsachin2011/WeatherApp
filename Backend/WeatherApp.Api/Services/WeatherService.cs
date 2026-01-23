using System.Net.Http;
using System.Text.Json;
using WeatherApp.Api.Models;

namespace WeatherApp.Api.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _weatherDataDir;
        private readonly string _baseUrl;
        private readonly string _latitude;
        private readonly string _longitude;
        private readonly string _dailyFields;
        private readonly string _timezone;

        public WeatherService(HttpClient httpClient, string weatherDataDir, IConfiguration config)
        {
            _httpClient = httpClient;
            _weatherDataDir = weatherDataDir;
            var section = config.GetSection("OpenMeteo");
            _baseUrl = section["BaseUrl"] ?? "https://archive-api.open-meteo.com/v1/archive";
            _latitude = section["Latitude"] ?? "32.78";
            _longitude = section["Longitude"] ?? "-96.8";
            _dailyFields = section["DailyFields"] ?? "temperature_2m_max,temperature_2m_min,precipitation_sum";
            _timezone = section["Timezone"] ?? "auto";
        }

        public async Task<WeatherResult> GetWeatherForDateAsync(string isoDate)
        {
            var filePath = Path.Combine(_weatherDataDir, $"{isoDate}.json");
            if (File.Exists(filePath))
            {
                var json = await File.ReadAllTextAsync(filePath);
                return JsonSerializer.Deserialize<WeatherResult>(json) ?? new WeatherResult { Date = isoDate, Error = "Corrupt local data" };
            }

            var url = $"{_baseUrl}?latitude={_latitude}&longitude={_longitude}&start_date={isoDate}&end_date={isoDate}&daily={_dailyFields}&timezone={_timezone}";
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return new WeatherResult { Date = isoDate, Error = $"API error: {response.StatusCode}" };
                }
                var content = await response.Content.ReadAsStringAsync();
                var apiResult = JsonSerializer.Deserialize<OpenMeteoResponse>(content);
                var result = new WeatherResult { Date = isoDate };
                if (apiResult?.Daily?.Time != null && apiResult.Daily.Time.Count > 0)
                {
                    result.MinTemperature = apiResult.Daily.MinTemp?.FirstOrDefault();
                    result.MaxTemperature = apiResult.Daily.MaxTemp?.FirstOrDefault();
                    result.Precipitation = apiResult.Daily.Precipitation?.FirstOrDefault();
                }
                else
                {
                    result.Error = "No data returned for this date";
                }
                await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(result));
                return result;
            }
            catch (Exception ex)
            {
                return new WeatherResult { Date = isoDate, Error = $"Exception: {ex.Message}" };
            }
        }
    }
}
