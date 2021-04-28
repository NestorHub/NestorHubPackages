using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NestorHub.Netatmo.Security.Class
{
    public static class HomeDataService
    {
        public static async Task<IEnumerable<Home>> GetData(string homeId, int numberOfEvents, string accessToken)
        {
            var response = await GetHomeDataOnline(homeId, numberOfEvents, accessToken);
            return GetHomesData(response);
        }

        private static IEnumerable<Home> GetHomesData(HttpResponseMessage response)
        {
            var homesContent = GetHomesContent(response);
            var homesData = JsonConvert.DeserializeObject<IEnumerable<Home>>(homesContent);
            return homesData;
        }

        private static string GetHomesContent(HttpResponseMessage response)
        {
            var fullJToken = (JObject) JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
            var homesData = JsonConvert.SerializeObject(fullJToken.First.First.First.First);
            return homesData;
        }

        private static async Task<HttpResponseMessage> GetHomeDataOnline(string homeId, int numberOfEvents, string accessToken)
        {
            var client = HttpClientFactory.Create();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var parameters = new List<string>();
            if(!string.IsNullOrEmpty(homeId) && !string.IsNullOrWhiteSpace(homeId))
            {
                parameters.Add($"home_id={homeId}");
            }

            if(numberOfEvents >= 0)
            {
                parameters.Add($"size={numberOfEvents}");
            }

            var response = await client.GetAsync($"https://api.netatmo.com/api/gethomedata{UrlParameters(parameters)}");
            return response;
        }

        private static string UrlParameters(List<string> parameters)
        {
            if(!parameters.Any()) return string.Empty;

            var urlParameters = "";
            foreach (var parameter in parameters)
            {
                urlParameters += !string.IsNullOrEmpty(urlParameters) ? "&" : "";
                urlParameters += parameter;
            }
            return $"?{urlParameters}";
        }
    }
}