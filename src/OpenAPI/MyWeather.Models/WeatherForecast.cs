using System;

namespace MyWeather.Models
{
    /// <summary>
    /// Contains the weather forecast
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// Date and time the results were generated
        /// </summary>
        /// <example>2021-02-15T09:00:00.000Z</example>
        public DateTime Date { get; set; }

        /// <summary>
        /// The temperature in celsius
        /// </summary>
        /// <example>31</example>
        public int TemperatureC { get; set; }

        /// <summary>
        /// The temperature in fahrenheit
        /// </summary>
        /// <example>991</example>
        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);

        /// <summary>
        /// a summary of the weather
        /// </summary>
        /// <example>muggy</example>
        public string Summary { get; set; }
    }
}