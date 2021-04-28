using System;
using NestorHub.Netatmo.Common.Class;
using NestorHub.Netatmo.Weather.Class.Dashboards.WeatherValue;
using NestorHub.Netatmo.Weather.Enums;
using NestorHub.Netatmo.Weather.Interfaces;
using Newtonsoft.Json.Linq;

namespace NestorHub.Netatmo.Weather.Class.Dashboards
{
    public class WeatherBaseStationDashboard : IWeatherDashboard
    {
        public Temperature Temperature { get; }
        public Humidity Humidity { get; }
        public Trend PressureTrend { get; }
        public Trend TemperatureTrend { get; }
        public Temperature MinTemp { get; }
        public Temperature MaxTemp { get; }
        public DateTime DateMinTemp { get; }
        public DateTime DateMaxTemp { get; }
        public Pressure Pressure { get; }
        public Noise Noise { get; }
        public Co2 Co2 { get; }

        public WeatherBaseStationDashboard(JObject item, IUserUnitsService userUnitsService)
            : this(item["Temperature"]?.Value<string>(), item["Humidity"]?.Value<string>(), item["Pressure"]?.Value<string>(), 
                item["Noise"]?.Value<string>(), item["CO2"]?.Value<string>(), item["min_temp"]?.Value<string>(), item["max_temp"]?.Value<string>(),
                item["temp_trend"]?.Value<string>(), item["pressure_trend"]?.Value<string>(), item["date_min_temp"]?.Value<string>(),
                item["date_max_temp"]?.Value<string>(), userUnitsService)
        {}

        private WeatherBaseStationDashboard(string temperature, string humidity, string pressure, 
            string noise, string co2, string minTemp, string maxTemp, string tempTrend, 
            string pressureTrend, string dateMinTemp, string dateMaxTemp, IUserUnitsService userUnitsService)
        {
            PressureTrend = !string.IsNullOrEmpty(pressureTrend) ? pressureTrend.GetTrend() : Trend.Stable;
            TemperatureTrend = !string.IsNullOrEmpty(tempTrend) ? tempTrend.GetTrend() : Trend.Stable;
            MinTemp = !string.IsNullOrEmpty(minTemp) ? new Temperature(minTemp.ConvertToDoubleFromNetatmo(), userUnitsService.GetTemperatureUnit()) : new Temperature(0, TemperatureUnit.Undefined);
            MaxTemp = !string.IsNullOrEmpty(maxTemp) ? new Temperature(maxTemp.ConvertToDoubleFromNetatmo(), userUnitsService.GetTemperatureUnit()) : new Temperature(0, TemperatureUnit.Undefined);
            Pressure = !string.IsNullOrEmpty(pressure) ? new Pressure(pressure.ConvertToDoubleFromNetatmo(), userUnitsService.GetPressureUnit()) : new Pressure(0, PressureUnit.Undefined);
            Noise = !string.IsNullOrEmpty(noise) ? new Noise(noise.ConvertToDoubleFromNetatmo()) : new Noise(-1);
            Co2 = !string.IsNullOrEmpty(co2) ? new Co2(co2.ConvertToDoubleFromNetatmo()) : new Co2(-1);
            Temperature = !string.IsNullOrEmpty(temperature) ? new Temperature(temperature.ConvertToDoubleFromNetatmo(), userUnitsService.GetTemperatureUnit()) : new Temperature(0, TemperatureUnit.Undefined);
            Humidity = !string.IsNullOrEmpty(humidity) ? new Humidity(humidity.ConvertToDoubleFromNetatmo()) : new Humidity(-1);
            DateMinTemp = !string.IsNullOrEmpty(dateMinTemp) ? DateTimeOffset.FromUnixTimeSeconds(dateMinTemp.ConvertToLongFromNetatmo()).LocalDateTime : DateTime.MinValue;
            DateMaxTemp = !string.IsNullOrEmpty(dateMaxTemp) ? DateTimeOffset.FromUnixTimeSeconds(dateMaxTemp.ConvertToLongFromNetatmo()).LocalDateTime : DateTime.MinValue;
        }
    }
}