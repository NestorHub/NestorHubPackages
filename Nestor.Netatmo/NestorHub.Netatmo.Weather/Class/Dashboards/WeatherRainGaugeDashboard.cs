using Kopigi.Utils.Helpers;
using NestorHub.Netatmo.Common.Class;
using NestorHub.Netatmo.Weather.Class.Dashboards.WeatherValue;
using NestorHub.Netatmo.Weather.Enums;
using NestorHub.Netatmo.Weather.Interfaces;
using Newtonsoft.Json.Linq;

namespace NestorHub.Netatmo.Weather.Class.Dashboards
{
    public class WeatherRainGaugeDashboard : IWeatherDashboard
    {
        public Rain CurrentRain { get; }
        public Rain SumRainLastHour { get; }
        public Rain SumRainLast24Hour { get; }

        public WeatherRainGaugeDashboard(JObject item, IUserUnitsService userUnitsService)
            : this(item["Rain"]?.Value<string>(), item["sum_rain_1"]?.Value<string>(),
                item["sum_rain_24"]?.Value<string>(), userUnitsService)
        { }

        private WeatherRainGaugeDashboard(string currentRain, string sumRain1, string sumRain24, IUserUnitsService userUnitsService)
        {
            SumRainLastHour = !string.IsNullOrEmpty(currentRain) ? new Rain(sumRain1.ConvertToDoubleFromNetatmo(), userUnitsService.GetRainUnit()) : new Rain(-1, RainUnit.Undefined);
            SumRainLast24Hour = !string.IsNullOrEmpty(sumRain24) ? new Rain(sumRain24.ConvertToDoubleFromNetatmo(), userUnitsService.GetRainUnit()) : new Rain(-1, RainUnit.Undefined);
            CurrentRain = !string.IsNullOrEmpty(currentRain) ? new Rain(currentRain.ConvertToDoubleFromNetatmo(), $"{StringEnum.GetStringValue(userUnitsService.GetRainUnit())}/h") : new Rain(-1, RainUnit.Undefined);
        }
    }
}