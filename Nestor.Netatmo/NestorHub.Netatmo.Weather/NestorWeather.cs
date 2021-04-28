using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.DependencyInjection;
using NestorHub.Common.Api.Mef;
using NestorHub.Netatmo.Common;
using NestorHub.Netatmo.Weather.Class;
using NestorHub.Netatmo.Weather.Converters;
using NestorHub.Netatmo.Weather.Interfaces;
using NestorHub.Sentinels.Api;
using NestorHub.Sentinels.Api.Class;
using NestorHub.Sentinels.Api.Domain;
using Newtonsoft.Json;

namespace NestorHub.Netatmo.Weather
{
    [Sentinel("NestorWeather")]
    public class NestorWeather : SentinelHost, ISentinel
    {
        private StatesValuesHost _statesValuesHost;
        private Timer _timerToSendStateValues;
        private string _deviceMacAddress;
        private WeatherDevicesManager _weatherDeviceManager;
        private ServiceProvider _servicesProvider;

        public async Task<bool> Run(string sentinelName, string packageName, string homeControllerUrl, int homeControllerPort, bool useSsl,
            object parameter)
        {
            base.Run(sentinelName, packageName);

            var clientId = SetParameters(parameter, out var clientSecret, out var username, out var password);
            var weatherTokensFactory = new WeatherTokensFactory(username, password, clientId, clientSecret);

            _servicesProvider = ConfigureServices();

            _statesValuesHost = new StatesValuesHost(this, HomeConnectionServer.CreateConnection(homeControllerUrl, homeControllerPort, useSsl));
            _weatherDeviceManager = new WeatherDevicesManager(weatherTokensFactory, _servicesProvider.GetService<IUserUnitsService>());

            SetTimeToSendValue(parameter);

            await FirstRetrieveData();

            return true;
        }

        public async Task<bool> Stop()
        {
            _timerToSendStateValues.Stop();
            _timerToSendStateValues = null;
            return true;
        }

        private async Task FirstRetrieveData()
        {
            await SendStatesValue();
        }

        private void SetTimeToSendValue(object parameter)
        {
            _timerToSendStateValues = new Timer(GetIntervalInMilliseconds(parameter));
            _timerToSendStateValues.Elapsed += RetrieveWeatherData;
            _timerToSendStateValues.Start();
        }

        private static int GetIntervalInMilliseconds(object parameter)
        {
            var intervalToSend = parameter.GetPropertyValue<int>("interval");
            if (intervalToSend<10)
            {
                intervalToSend = 10;
            }
            return (intervalToSend*60)*1000;
        }

        private async void RetrieveWeatherData(object sender, ElapsedEventArgs e)
        {
            await SendStatesValue();
        }

        private async Task SendStatesValue()
        {
            SetJsonSerializSettingsWithConverter();

            var weatherDeviceAndUnits = await _weatherDeviceManager.ReadDataFromDevice(_deviceMacAddress);

            SetJsonSerializSettingsWithoutConverter();

            await SendStateValueForDevice(weatherDeviceAndUnits);

            foreach (var weatherDataModule in weatherDeviceAndUnits.Modules)
            {
                await SendStateValueForDevice(weatherDataModule);
            }
        }

        private async Task SendStateValueForDevice(WeatherDevice weatherDevice)
        {
            await _statesValuesHost.Send(FormatModuleNameForStateValueKeyRules(weatherDevice.ModuleName), weatherDevice.DashboardData);
        }

        private string SetParameters(object parameter, out string clientSecret, out string username, out string password)
        {
            var clientId = parameter.GetPropertyValue<string>("clientId");
            clientSecret = parameter.GetPropertyValue<string>("clientSecret");
            _deviceMacAddress = parameter.GetPropertyValue<string>("deviceMacAddress");
            username = parameter.GetPropertyValue<string>("username");
            password = parameter.GetPropertyValue<string>("password");
            return clientId;
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IUserUnitsService, UserUnitsService>();
            ServiceProvider servicesProvider = services.BuildServiceProvider();
            return servicesProvider;
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

        private void SetJsonSerializSettingsWithoutConverter()
        {
            JsonConvert.DefaultSettings = () => NoSettingsFactory();
        }

        private void SetJsonSerializSettingsWithConverter()
        {
            JsonConvert.DefaultSettings = () => SettingsFactory(_servicesProvider.GetService<IUserUnitsService>());
        }

        private string FormatModuleNameForStateValueKeyRules(string moduleName)
        {
            return RemoveDiacritics(moduleName.Replace(" ", ""));
        }

        static string RemoveDiacritics(string moduleName)
        {
            var moduleNameNormalized = moduleName.Normalize(NormalizationForm.FormD);
            var moduleNameBuilder = new StringBuilder();

            foreach (var moduleNameChar in moduleNameNormalized)
            {
                var unicodeChar = CharUnicodeInfo.GetUnicodeCategory(moduleNameChar);
                if (unicodeChar != UnicodeCategory.NonSpacingMark)
                {
                    moduleNameBuilder.Append(moduleNameChar);
                }
            }
            return (moduleNameBuilder.ToString().Normalize(NormalizationForm.FormC));
        }
    }
}
