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
        public DateTime Date { get; set; }

        /// <summary>
        /// The temperature in celsius
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// The temperature in fahrenheit
        /// </summary>
        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);

        /// <summary>
        /// a summary of the weather
        /// </summary>
        public string Summary { get; set; }
    }
}