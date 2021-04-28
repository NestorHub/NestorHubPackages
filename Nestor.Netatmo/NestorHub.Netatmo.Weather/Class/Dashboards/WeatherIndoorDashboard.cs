using NestorHub.Netatmo.Common.Class;
using NestorHub.Netatmo.Weather.Class.Dashboards.WeatherValue;
using NestorHub.Netatmo.Weather.Enums;
using NestorHub.Netatmo.Weather.Interfaces;
using Newtonsoft.Json.Linq;

namespace NestorHub.Netatmo.Weather.Class.Dashboards
{
    public class WeatherIndoorDashboard : IWeatherDashboard
    {
        public Co2 Co2 { get; }
        public Temperature Temperature { get; }
        public Humidity Humidity { get; }

        public WeatherIndoorDashboard(JObject item, IUserUnitsService userUnitsService)
            : this(item["Temperature"]?.Value<string>(), item["Humidity"]?.Value<string>(), 
                item["CO2"]?.Value<string>(), userUnitsService)
        { }

        private WeatherIndoorDashboard(string temperature, string humidity, string co2, IUserUnitsService userUnitsService)
        {
            Co2 = !string.IsNullOrEmpty(co2) ? new Co2(co2.ConvertToDoubleFromNetatmo()) : new Co2(-1);
            Temperature = !string.IsNullOrEmpty(temperature) ? new Temperature(temperature.ConvertToDoubleFromNetatmo(), userUnitsService.GetTemperatureUnit()) : new Temperature(0, TemperatureUnit.Undefined);
            Humidity = !string.IsNullOrEmpty(humidity) ? new Humidity(humidity.ConvertToDoubleFromNetatmo()) : new Humidity(-1);
        }
    }
}