using System.Linq;
using NestorHub.Netatmo.Common;
using NestorHub.Netatmo.Weather.Class;
using NestorHub.Netatmo.Weather.Class.Dashboards;
using NestorHub.Netatmo.Weather.Converters;
using NestorHub.Netatmo.Weather.Interfaces;
using Newtonsoft.Json;
using NFluent;
using Xunit;

namespace NestorHub.Netatmo.Weather.Tests
{
    public class WeatherDevicesManagerShould
    {
        private WeatherTokensFactory _weatherTokensFactory;
        private UserUnitsService _userUnitsService;

        public WeatherDevicesManagerShould()
        {
            var clientId = "587790346d1dbd5e1c8ba445";
            var clientSecret = "zvUT8N2M6yeARhswSYhCn0THSRM2KsKMQP9afV";
            var username = "mplessis@hotmail.com";
            var password = "zru*p387";

            _weatherTokensFactory = new WeatherTokensFactory(username, password, clientId, clientSecret);
            _userUnitsService = new UserUnitsService();
            _userUnitsService.SetUserUnits(new UserUnits("0", "0", "0"));
            JsonConvert.DefaultSettings = () => SettingsFactory(_userUnitsService);
        }

        [Fact]
        public async void return_data_device_from_marc_station()
        {
            var deviceManager = new WeatherDevicesManager(_weatherTokensFactory, _userUnitsService);
            var deviceData = await deviceManager.ReadDataFromDevice("70:ee:50:04:65:4c");
            Check.That(deviceData.Modules.Count()).IsEqualTo(2);
            Check.That(((WeatherBaseStationDashboard)deviceData.DashboardData).Pressure.Value).IsStrictlyGreaterThan(0);
            Check.That(((WeatherBaseStationDashboard)deviceData.DashboardData).Noise.Value).IsStrictlyGreaterThan(0);
            Check.That(((WeatherBaseStationDashboard)deviceData.DashboardData).Temperature.Unit).IsEqualTo("°C");

            var rainModules = deviceData.Modules.Where(m => m.DashboardData is WeatherRainGaugeDashboard);
            if (rainModules.Any())
            {
                Check.That(((WeatherRainGaugeDashboard)rainModules.First().DashboardData).CurrentRain.Unit).IsEqualTo("mm/h");
            }
        }

        [Fact]
        public async void return_data_device_from_serge_station()
        {
            var deviceManager = new WeatherDevicesManager(_weatherTokensFactory, _userUnitsService);
            var deviceData = await deviceManager.ReadDataFromDevice("70:ee:50:29:4a:4a");
            Check.That(deviceData.Modules.Count()).IsEqualTo(3);
            Check.That(((WeatherBaseStationDashboard)deviceData.DashboardData).Pressure.Value).IsStrictlyGreaterThan(0);
            Check.That(((WeatherBaseStationDashboard)deviceData.DashboardData).Noise.Value).IsStrictlyGreaterThan(0);
            Check.That(((WeatherBaseStationDashboard) deviceData.DashboardData).Noise.Unit).IsEqualTo("db");
            Check.That(((WeatherBaseStationDashboard)deviceData.DashboardData).Temperature.Unit).IsEqualTo("°C");
        }

        [Fact]
        public async void serialize_dashboard()
        {
            var deviceManager = new WeatherDevicesManager(_weatherTokensFactory, _userUnitsService);
            var deviceData = await deviceManager.ReadDataFromDevice("70:ee:50:29:4a:4a");

            JsonConvert.DefaultSettings = () => NoSettingsFactory();

            var jsonData = JsonConvert.SerializeObject(deviceData.DashboardData);
            Check.That(jsonData).IsNotEmpty();
        }

        private JsonSerializerSettings SettingsFactory(IUserUnitsService userUnitsService)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new WeatherDashboardDataConverter(userUnitsService));

            return settings;
        }

        private JsonSerializerSettings NoSettingsFactory()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Clear();

            return settings;
        }
    }
}
