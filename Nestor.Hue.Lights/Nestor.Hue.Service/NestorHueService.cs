using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using HomeController.Common.Api.Mef;
using HomeController.Sentinels.Api;
using HomeController.Sentinels.Api.Class;
using HomeController.Sentinels.Api.Domain;
using Nestor.Hue.Lights.Class;
using Q42.HueApi;

namespace Nestor.Hue.Service
{
    [Sentinel("NestorHue")]
    public class NestorHueService : SentinelHost, ISentinel
    {
        private const int MinimalIntervalToRetrieveData = 5;
        private StatesValuesHost _statesValuesHost;
        private Timer _timerToSendStateValues;
        private Timer _timerToHoursOff;
        private LightsManager _lightsManager;

        private Dictionary<string, bool> _stateOnOffForLights = new Dictionary<string, bool>();
        private Dictionary<string, int> _stateDimmerPercentForLights = new Dictionary<string, int>();
        private List<HourOff> _hoursOff;
        private TimeSpan _restartHourOff;

        public async Task<bool> Run(string sentinelName, string packageName, string homeControllerUrl, int homeControllerPort, bool useSsl,
            object parameter)
        {
            base.Run(sentinelName, packageName);

            SetParameters(parameter, out var hueKey, out var hueServerIp, out _hoursOff, out _restartHourOff);
            _lightsManager = new LightsManager(new LocalClient(hueServerIp, hueKey));

            _statesValuesHost = new StatesValuesHost(this, HomeConnectionServer.CreateConnection(homeControllerUrl, homeControllerPort, useSsl));

            SetTimeToSendValue(parameter);

            if (_hoursOff.Any())
            {
                SetTimeToHoursOff();
            }

            await FirstRetrieveData();

            return true;
        }

        public async Task<bool> Stop()
        {
            _timerToSendStateValues.Stop();
            _timerToSendStateValues = null;
            _timerToHoursOff.Stop();
            _timerToHoursOff = null;
            return true;
        }

        private void SetTimeToHoursOff()
        {
            _timerToHoursOff = new Timer(1000);
            _timerToHoursOff.Elapsed += OffLights;
            _timerToHoursOff.Start();
        }

        private void SetTimeToSendValue(object parameter)
        {
            _timerToSendStateValues = new Timer(GetIntervalInMilliseconds(parameter));
            _timerToSendStateValues.Elapsed += RetrieveLightsStatut;
            _timerToSendStateValues.Start();
        }

        private async void OffLights(object sender, ElapsedEventArgs e)
        {
            var nowTime = DateTime.Now.TimeOfDay;
            foreach (var hourOff in _hoursOff)
            {
                if (hourOff.Hour.Hours == nowTime.Hours && hourOff.Hour.Minutes == nowTime.Minutes)
                {
                    await _lightsManager.RetrieveLights();
                    await _lightsManager.StopAllLights();
                    _timerToHoursOff.Stop();
                }
            }
        }

        private static int GetIntervalInMilliseconds(object parameter)
        {
            var intervalToSend = parameter.GetPropertyValue<int>("interval");
            if (intervalToSend < MinimalIntervalToRetrieveData)
            {
                intervalToSend = MinimalIntervalToRetrieveData;
            }
            return intervalToSend * 1000;
        }

        private void SetParameters(object parameter, out string hueKey, out string hueServerIp,
            out List<HourOff> hoursOff, out TimeSpan restartHourOff)
        {
            hueKey = parameter.GetPropertyValue<string>("hueKey");
            hueServerIp = parameter.GetPropertyValue<string>("hueServerIp");
            hoursOff = parameter.GetListPropertyValue<HourOff>("hoursoff");
            TimeSpan.TryParse(parameter.GetPropertyValue<string>("restarthoursoff"), out restartHourOff);
        }

        private async Task FirstRetrieveData()
        {
            await SendStatesValue();
        }

        private async void RetrieveLightsStatut(object sender, ElapsedEventArgs e)
        {
            await SendStatesValue();
            RestartTimerHoursOff();
        }

        private void RestartTimerHoursOff()
        {
            if (DateTime.Now.TimeOfDay >= _restartHourOff)
            {
                _timerToHoursOff.Start();
            }
        }

        private async Task SendStatesValue()
        {
            await _lightsManager.RetrieveLights();
            foreach (var light in _lightsManager.Lights)
            {
                var stateNameFromLight = FormatModuleNameForStateValueKeyRules(light.Name);

                await _statesValuesHost.Send(stateNameFromLight, light.State.On);
                await _statesValuesHost.Send($"{stateNameFromLight}DimmerPercent", GetBrightness(light));
            }
        }

        private static int GetBrightness(Light light)
        {
            return ((light.State.Brightness * 100)/255);
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

    internal class HourOff
    {
        public TimeSpan Hour { get; set; }
    }
}
