using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nestor.Hue.Lights.Enums;
using Nestor.Hue.Lights.Interfaces;
using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.HSB;
using Q42.HueApi.Interfaces;

namespace Nestor.Hue.Lights.Class
{
    public class LightsManager
    {
        private readonly IHueClient _hueClient;

        public IEnumerable<Light> Lights { get; private set; }

        public LightsManager(IClient hueClient)
        {
            _hueClient = hueClient.GetClient();
        }

        public async Task RetrieveLights()
        {
            try
            {
                var lights = await _hueClient.GetLightsAsync();
                Lights = lights.ToList().Where(l => l.Capabilities.Control.ColorGamut != null);
            }
            catch (Exception e)
            {
                Lights = new List<Light>();
            }
        }

        public async Task<StateLight> StartLight(Light light)
        {
            await ChangeStateLights(true, new List<string>() {light.Id});
            return await GetState(light);
        }

        public async Task<StateLight> StopLight(Light light)
        {
            await ChangeStateLights(false, new List<string>() {light.Id});
            return await GetState(light);
        }

        public async Task<StateLight> StartAllLights()
        {
            var result = await ChangeStateLights(true, Lights.Select(l => l.Id).ToList());
            return result ? StateLight.On : StateLight.Off;
        }

        public async Task<StateLight> StopAllLights()
        {
            var result = await ChangeStateLights(false, Lights.Select(l => l.Id).ToList());
            return result ? StateLight.Off : StateLight.On;
        }

        public async Task<StateLight> SetColorOnLight(Light light, string hexColor)
        {
            var command = new LightCommand();
            command.TurnOn().SetColor(new RGBColor(hexColor)); 
            var result = await _hueClient.SendCommandAsync(command, new List<string>() { light.Id });
            return await GetState(light);
        }

        public async Task<StateLight> GetStateOfLight(Light light)
        {
            return await GetState(light);
        }

        private async Task<StateLight> GetState(Light light)
        {
            try
            {
                var state = await _hueClient.GetLightAsync(light.Id);
                return state.State.On ? StateLight.On : StateLight.Off;
            }
            catch (Exception e)
            {
                return StateLight.Off;
            }
        }

        private async Task<bool> ChangeStateLights(bool stateOfLights, List<string> lightsIds)
        {
            try
            {
                if (lightsIds.Any())
                {
                    var command = new LightCommand();
                    if (stateOfLights)
                    {
                        command.TurnOn();
                    }
                    else
                    {
                        command.TurnOff();
                    }

                    var result = await _hueClient.SendCommandAsync(command, lightsIds);
                    return !result.HasErrors();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}