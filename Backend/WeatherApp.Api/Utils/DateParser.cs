using System.Globalization;

namespace WeatherApp.Api.Utils
{
    public static class DateParser
    {
        private static readonly string[] SupportedFormats = new[]
        {
            "MM/dd/yyyy",
            "MMMM d, yyyy",
            "MMM-dd-yyyy"
        };

        public static (DateTime? date, string? error) ParseDate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return (null, "Empty or whitespace date string");

            foreach (var format in SupportedFormats)
            {
                if (DateTime.TryParseExact(input.Trim(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                {
                    return (dt, null);
                }
            }
            // Try general parse as fallback
            if (DateTime.TryParse(input.Trim(), out var generalDt))
            {
                return (generalDt, null);
            }
            return (null, $"Invalid date: '{input}'");
        }
    }
}
