using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NestorHub.Netatmo.Common;
using NestorHub.Netatmo.Weather.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NestorHub.Netatmo.Weather.Class
{
    public class WeatherDevicesManager
    {
        private readonly WeatherTokensFactory _weatherTokensFactory;
        private readonly IUserUnitsService _userUnitsService;

        public WeatherDevicesManager(WeatherTokensFactory weatherTokensFactory, IUserUnitsService userUnitsService)
        {
            _weatherTokensFactory = weatherTokensFactory;
            _userUnitsService = userUnitsService;
        }

        public async Task<WeatherDevice> ReadDataFromDevice(string macAddress)
        {
            var accessToken = await _weatherTokensFactory.GetToken();
            var response = await GetDeviceDataOnline(macAddress, accessToken);
            var units = GetUnits(response);
            _userUnitsService.SetUserUnits(units);

            return GetDeviceData(response);
        }

        private WeatherDevice GetDeviceData(HttpResponseMessage response)
        {
            var devicesContent = GetDevicesContent(response);
            var weatherDevices = JsonConvert.DeserializeObject<IEnumerable<WeatherDevice>>(devicesContent);
            return weatherDevices.First();
        }

        private static string GetDevicesContent(HttpResponseMessage response)
        {
            var fullJToken = (JObject) JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
            var devicesData = JsonConvert.SerializeObject(fullJToken.First.First.First.First);
            return devicesData;
        }

        private UserUnits GetUnits(HttpResponseMessage response)
        {
            var userContent = GetUserContent(response);
            return JsonConvert.DeserializeObject<UserUnits>(userContent);
        }

        private static string GetUserContent(HttpResponseMessage response)
        {
            var fullJToken = (JObject) JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
            var userContent = JsonConvert.SerializeObject(fullJToken.First.First.Last.First.Last.First);
            return userContent;
        }

        private static async Task<HttpResponseMessage> GetDeviceDataOnline(string deviceMacAddress, string accessToken)
        {
            var client = HttpClientFactory.Create();
            var response = await client.GetAsync($"https://api.netatmo.com/api/getstationsdata?access_token={accessToken}&device_id={deviceMacAddress}");
            return response;
        }
    }
}