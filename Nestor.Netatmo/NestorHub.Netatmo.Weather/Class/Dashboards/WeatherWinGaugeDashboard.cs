using System;
using NestorHub.Netatmo.Common.Class;
using NestorHub.Netatmo.Weather.Class.Dashboards.WeatherValue;
using NestorHub.Netatmo.Weather.Enums;
using NestorHub.Netatmo.Weather.Interfaces;
using Newtonsoft.Json.Linq;

namespace NestorHub.Netatmo.Weather.Class.Dashboards
{
    public class WeatherWinGaugeDashboard : IWeatherDashboard
    {
        public Wind WindStrength { get; set; }
        public Angle WindAngle { get; set; }
        public Wind GustStrength { get; }
        public Angle GustAngle { get; }
        public Wind MaxWindStrength { get; }
        public Angle MaxWindAngle { get; }
        public DateTime DateMaxWindStrength { get; }

        public WeatherWinGaugeDashboard(JObject item, IUserUnitsService userUnitsService)
            : this(item["WindStrength"]?.Value<string>(), item["WindAngle"]?.Value<string>(),
                item["GustStrength"]?.Value<string>(), item["GustAngle"]?.Value<string>(), 
                item["max_wind_str"]?.Value<string>(), item["max_wind_angle"]?.Value<string>(), 
                item["date_max_wind_str"]?.Value<string>(), userUnitsService)
        { }

        private WeatherWinGaugeDashboard(string windStrength, string windAngle, string gustStrength,
            string gustAngle, string maxWindStr, string maxWindAngle, string dateMaxWindStr, IUserUnitsService userUnitsService)
        {
            WindStrength = !string.IsNullOrEmpty(windStrength) ? new Wind(windStrength.ConvertToDoubleFromNetatmo(), userUnitsService.GetWindUnit()) : new Wind(-1, WindUnit.Undefined);
            WindAngle = !string.IsNullOrEmpty(windAngle) ? new Angle(windAngle.ConvertToDoubleFromNetatmo()) : new Angle(-1);
            GustStrength = !string.IsNullOrEmpty(gustStrength) ? new Wind(gustStrength.ConvertToDoubleFromNetatmo(), userUnitsService.GetWindUnit()) : new Wind(-1, WindUnit.Undefined);
            GustAngle = !string.IsNullOrEmpty(gustAngle) ? new Angle(gustAngle.ConvertToDoubleFromNetatmo()) : new Angle(-1);
            MaxWindStrength = !string.IsNullOrEmpty(maxWindStr) ? new Wind(maxWindStr.ConvertToDoubleFromNetatmo(), userUnitsService.GetWindUnit()) : new Wind(-1, WindUnit.Undefined);
            MaxWindAngle = !string.IsNullOrEmpty(maxWindAngle) ? new Angle(maxWindAngle.ConvertToDoubleFromNetatmo()) : new Angle(-1);
            DateMaxWindStrength = !string.IsNullOrEmpty(dateMaxWindStr) ? DateTimeOffset.FromUnixTimeSeconds(dateMaxWindStr.ConvertToLongFromNetatmo())
                .LocalDateTime : DateTime.MinValue;
        }
    }
}