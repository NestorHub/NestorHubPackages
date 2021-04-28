using System;
using NestorHub.Netatmo.Common.Class;
using NestorHub.Netatmo.Weather.Class.Dashboards.WeatherValue;
using NestorHub.Netatmo.Weather.Enums;
using NestorHub.Netatmo.Weather.Interfaces;
using Newtonsoft.Json.Linq;

namespace NestorHub.Netatmo.Weather.Class.Dashboards
{
    public class WeatherOutdoorDashboard : IWeatherDashboard
    {
        public Temperature Temperature { get; set; }
        public Humidity Humidity { get; set; }
        public Trend TemperatureTrend { get; }
        public Temperature MinTemp { get; }
        public Temperature MaxTemp { get; }
        public DateTime DateMinTemp { get; }
        public DateTime DateMaxTemp { get; }

        public WeatherOutdoorDashboard(JObject item, IUserUnitsService userUnitsService)
            : this(item["Temperature"]?.Value<string>(), item["Humidity"]?.Value<string>(), item["min_temp"]?.Value<string>(), item["max_temp"]?.Value<string>(),
                item["temp_trend"]?.Value<string>(), item["date_min_temp"]?.Value<string>(),
                item["date_max_temp"]?.Value<string>(), userUnitsService)
        { }

        private WeatherOutdoorDashboard(string temperature, string humidity, string minTemp, 
            string maxTemp, string tempTrend, string dateMinTemp, string dateMaxTemp, IUserUnitsService userUnitsService)
        {
            Temperature = !string.IsNullOrEmpty(temperature) ? new Temperature(temperature.ConvertToDoubleFromNetatmo(), userUnitsService.GetTemperatureUnit()) : new Temperature(0, TemperatureUnit.Undefined);
            Humidity = !string.IsNullOrEmpty(humidity) ? new Humidity(humidity.ConvertToDoubleFromNetatmo()) : new Humidity(-1);
            MinTemp = !string.IsNullOrEmpty(minTemp) ? new Temperature(minTemp.ConvertToDoubleFromNetatmo(), userUnitsService.GetTemperatureUnit()) : new Temperature(0, TemperatureUnit.Undefined);
            MaxTemp = !string.IsNullOrEmpty(maxTemp) ? new Temperature(maxTemp.ConvertToDoubleFromNetatmo(), userUnitsService.GetTemperatureUnit()) : new Temperature(0, TemperatureUnit.Undefined);
            TemperatureTrend = !string.IsNullOrEmpty(tempTrend) ? tempTrend.GetTrend() : Trend.Stable;
            DateMinTemp = !string.IsNullOrEmpty(dateMinTemp) ? DateTimeOffset.FromUnixTimeSeconds(dateMinTemp.ConvertToLongFromNetatmo()).LocalDateTime : DateTime.MinValue;
            DateMaxTemp = !string.IsNullOrEmpty(dateMaxTemp) ? DateTimeOffset.FromUnixTimeSeconds(dateMaxTemp.ConvertToLongFromNetatmo()).LocalDateTime : DateTime.MinValue;
        }
    }
}