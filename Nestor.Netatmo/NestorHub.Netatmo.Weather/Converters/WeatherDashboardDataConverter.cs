using System;
using NestorHub.Netatmo.Weather.Class.Dashboards;
using NestorHub.Netatmo.Weather.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NestorHub.Netatmo.Weather.Converters
{
    public class WeatherDashboardDataConverter : JsonConverter
    {
        private readonly IUserUnitsService _userUnitsService;

        public WeatherDashboardDataConverter(IUserUnitsService userUnitsService)
        {
            _userUnitsService = userUnitsService;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {}

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var item = JObject.Load(reader);
            if (objectType == typeof(WeatherBaseStationDashboard))
            {
                return ToBaseStationDashboard(item);
            }

            if (objectType == typeof(WeatherOutdoorDashboard))
            {
                return ToOutdoorDashboard(item);
            }

            if (objectType == typeof(WeatherIndoorDashboard))
            {
                return ToIndoorDashboard(item);
            }

            if (objectType == typeof(WeatherRainGaugeDashboard))
            {
                return ToRainGaugeDashboard(item);
            }

            if (objectType == typeof(WeatherWinGaugeDashboard))
            {
                return ToWindGaugeDashboard(item);
            }
            return new object();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IWeatherDashboard).IsAssignableFrom(objectType);
        }

        private IWeatherDashboard ToBaseStationDashboard(JObject item)
        {
            return new WeatherBaseStationDashboard(item, _userUnitsService);
        }

        private IWeatherDashboard ToOutdoorDashboard(JObject item)
        {
            return new WeatherOutdoorDashboard(item, _userUnitsService);
        }

        private IWeatherDashboard ToIndoorDashboard(JObject item)
        {
            return new WeatherIndoorDashboard(item, _userUnitsService);
        }

        private IWeatherDashboard ToRainGaugeDashboard(JObject item)
        {
            return new WeatherRainGaugeDashboard(item, _userUnitsService);
        }

        private IWeatherDashboard ToWindGaugeDashboard(JObject item)
        {
            return new WeatherWinGaugeDashboard(item, _userUnitsService);
        }
    }
}
