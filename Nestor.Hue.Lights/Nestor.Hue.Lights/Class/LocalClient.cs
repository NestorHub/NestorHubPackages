using System;
using System.Collections.Generic;
using System.Text;
using Nestor.Hue.Lights.Interfaces;
using Q42.HueApi;
using Q42.HueApi.Interfaces;

namespace Nestor.Hue.Lights.Class
{
    public class LocalClient : IClient
    {
        private readonly LocalHueClient _client;

        public LocalClient(string ipAddress, string applicationKey)
        {
            _client = new LocalHueClient(ipAddress);
            _client.Initialize(applicationKey);
        }

        public IHueClient GetClient()
        {
            return _client;
        }
    }
}
