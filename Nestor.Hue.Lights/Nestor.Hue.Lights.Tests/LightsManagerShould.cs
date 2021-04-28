using System;
using System.Linq;
using Nestor.Hue.Lights.Class;
using Nestor.Hue.Lights.Enums;
using Nestor.Hue.Lights.Interfaces;
using NFluent;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using Xunit;

namespace Nestor.Hue.Lights.Tests
{
    public class LightsManagerShould
    {
        private readonly IClient _client;

        public LightsManagerShould()
        {
            _client = new LocalClient("192.168.1.26", "Ta519Bn0t3JvVLORifBtAQhk09RqI6ki2DUBuNQi");
        }

        [Fact]
        public async void return_two_lights()
        {
            var lightsManager = new LightsManager(_client);
            await lightsManager.RetrieveLights();
            Check.That(lightsManager.Lights.ToList().Count).IsEqualTo(2);
        }

        [Fact]
        public async void return_true_on_start_all_light()
        {
            var lightsManager = new LightsManager(_client);
            await lightsManager.RetrieveLights();
            var result = await lightsManager.StartAllLights();
            Check.That(result).IsEqualTo(StateLight.On);
        }

        [Fact]
        public async void return_true_on_stop_all_light()
        {
            var lightsManager = new LightsManager(_client);
            await lightsManager.RetrieveLights();
            var result = await lightsManager.StopAllLights();
            Check.That(result).IsEqualTo(StateLight.Off);
        }

        [Fact]
        public async void return_true_on_stop_one_light()
        {
            var lightsManager = new LightsManager(_client);
            await lightsManager.RetrieveLights();
            var lights = lightsManager.Lights;
            var result = await lightsManager.StopLight(lights.First());
            Check.That(result).IsEqualTo(StateLight.Off);
        }

        [Fact]
        public async void return_true_on_start_one_light()
        {
            var lightsManager = new LightsManager(_client);
            await lightsManager.RetrieveLights();
            var lights = lightsManager.Lights;
            var result = await lightsManager.StartLight(lights.First());
            Check.That(result).IsEqualTo(StateLight.On);
        }

        [Fact]
        public async void return_true_on_set_color_one_light()
        {
            var lightsManager = new LightsManager(_client);
            await lightsManager.RetrieveLights();
            var lights = lightsManager.Lights;
            var result = await lightsManager.SetColorOnLight(lights.First(), "#FF00AA");
            Check.That(result).IsEqualTo(StateLight.On);
        }

        [Fact]
        public async void return_on_or_state_on_one_light()
        {
            var lightsManager = new LightsManager(_client);
            await lightsManager.RetrieveLights();
            var lights = lightsManager.Lights;
            await lightsManager.StartLight(lights.First());
            var state = await lightsManager.GetStateOfLight(lights.First());
            Check.That(state).IsEqualTo(StateLight.On);
        }
    }
}
