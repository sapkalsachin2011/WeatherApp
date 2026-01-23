using System.Text.Json.Serialization;

namespace WeatherApp.Api.Models
{
    public class WeatherResult
    {
        public string Date { get; set; } = string.Empty;
        public double? MinTemperature { get; set; }
        public double? MaxTemperature { get; set; }
        public double? Precipitation { get; set; }
        public string? Error { get; set; }
    }

    public class OpenMeteoResponse
    {
        [JsonPropertyName("daily")]
        public DailyWeather? Daily { get; set; }
    }

    public class DailyWeather
    {
        [JsonPropertyName("time")]
        public List<string>? Time { get; set; }
        [JsonPropertyName("temperature_2m_min")]
        public List<double>? MinTemp { get; set; }
        [JsonPropertyName("temperature_2m_max")]
        public List<double>? MaxTemp { get; set; }
        [JsonPropertyName("precipitation_sum")]
        public List<double>? Precipitation { get; set; }
    }
}
