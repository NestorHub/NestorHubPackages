using NestorHub.Netatmo.Weather.Enums;
using NestorHub.Netatmo.Weather.Interfaces;
using Newtonsoft.Json;

namespace NestorHub.Netatmo.Weather.Class.Dashboards
{
    public static class WeatherDashboardDataFactory
    {
        public static IWeatherDashboard GetDashboardData(DeviceType deviceType, string data)
        {
            switch (deviceType)
            {
                case DeviceType.BaseStation:
                    return JsonConvert.DeserializeObject<WeatherBaseStationDashboard>(data);
                case DeviceType.IndoorModule:
                    return JsonConvert.DeserializeObject<WeatherIndoorDashboard>(data);
                case DeviceType.OutdoorModule:
                    return JsonConvert.DeserializeObject<WeatherOutdoorDashboard>(data);
                case DeviceType.RainGauge:
                    return JsonConvert.DeserializeObject<WeatherRainGaugeDashboard>(data);
                case DeviceType.WinGauge:
                    return JsonConvert.DeserializeObject<WeatherWinGaugeDashboard>(data);
                default:
                    return new WeatherUnknownDashboard();
            }
        }
    }
}
