using System.Collections.Generic;
using NestorHub.Netatmo.Common.Class;
using NestorHub.Netatmo.Weather.Class.Dashboards;
using NestorHub.Netatmo.Weather.Enums;
using NestorHub.Netatmo.Weather.Interfaces;
using Newtonsoft.Json;

namespace NestorHub.Netatmo.Weather.Class
{
    public class WeatherDevice
    {
        public string MacAddress { get; }
        public DeviceType Type { get; }
        public string ModuleName { get; }
        public string StationName { get; }
        public IEnumerable<WeatherDevice> Modules { get; set; }
        public IWeatherDashboard DashboardData { get; }
        public int BatteryPercent { get; }
        public bool ReadOnly { get; }

        [JsonConstructor]
        public WeatherDevice(string _id, string type, string module_name, string station_name, string read_only, object dashboard_data, string battery_percent)
        {
            ReadOnly = read_only != null;
            MacAddress = _id;
            Type = type.GetTypeOfDevice();
            ModuleName = module_name;
            StationName = station_name;
            BatteryPercent = battery_percent?.ConvertToIntFromNetatmo() ?? -1;
            DashboardData = WeatherDashboardDataFactory.GetDashboardData(Type, dashboard_data.ToString());
        }
    }
}