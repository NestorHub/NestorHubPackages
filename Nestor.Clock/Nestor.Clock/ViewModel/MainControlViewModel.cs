using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Windows.UI.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HomeController.Common.Api;
using Kopigi.Utils.Helpers;
using Nestor.Clock.Class;
using Nestor.Clock.Common.Class;
using Nestor.Clock.Common.Enums;
using Nestor.Clock.Interfaces;
using Nestor.Clock.Messages;
using Nestor.Hue.Lights.Class;
using Q42.HueApi;
using GalaSoft.MvvmLight.Ioc;

namespace Nestor.Clock.ViewModel
{
    public class MainControlViewModel : ViewModelBase
    {
        private readonly Timer _displayHourTimer;
        private bool _lightToSwitchStarted;

        private string _displayHour;

        public string DisplayHour
        {
            get => _displayHour;
            set
            {
                _displayHour = value;
                RaisePropertyChanged(() => DisplayHour);
            }
        }

        private string _interiorTemperature;

        public string InteriorTemperature
        {
            get => _interiorTemperature;
            set
            {
                _interiorTemperature = value;
                RaisePropertyChanged(() => InteriorTemperature);
            }
        }

        private string _interiorTemperatureUnit;

        public string InteriorTemperatureUnit
        {
            get => _interiorTemperatureUnit;
            set
            {
                _interiorTemperatureUnit = value;
                RaisePropertyChanged(() => InteriorTemperatureUnit);
            }
        }

        private string _interiorHumidity;

        public string InteriorHumidity
        {
            get => _interiorHumidity;
            set
            {
                _interiorHumidity = value;
                RaisePropertyChanged(() => InteriorHumidity);
            }
        }

        private string _interiorHumidityUnit;

        public string InteriorHumidityUnit
        {
            get => _interiorHumidityUnit;
            set
            {
                _interiorHumidityUnit = value;
                RaisePropertyChanged(() => InteriorHumidityUnit);
            }
        }

        private string _interiorAirQuality;

        public string InteriorAirQuality
        {
            get => _interiorAirQuality;
            set
            {
                _interiorAirQuality = value;
                RaisePropertyChanged(() => InteriorAirQuality);
            }
        }

        private string _interiorAirQualityUnit;

        public string InteriorAirQualityUnit
        {
            get => _interiorAirQualityUnit;
            set
            {
                _interiorAirQualityUnit = value;
                RaisePropertyChanged(() => InteriorAirQualityUnit);
            }
        }

        private string _exteriorTemperature;

        public string ExteriorTemperature
        {
            get => _exteriorTemperature;
            set
            {
                _exteriorTemperature = value;
                RaisePropertyChanged(() => ExteriorTemperature);
            }
        }

        private string _exteriorTemperatureUnit;

        public string ExteriorTemperatureUnit
        {
            get => _exteriorTemperatureUnit;
            set
            {
                _exteriorTemperatureUnit = value;
                RaisePropertyChanged(() => ExteriorTemperatureUnit);
            }
        }

        private string _currentRain;

        public string CurrentRain
        {
            get => _currentRain;
            set
            {
                _currentRain = value;
                RaisePropertyChanged(() => CurrentRain);
            }
        }

        private string _currentRainUnit;

        public string CurrentRainUnit
        {
            get => _currentRainUnit;
            set
            {
                _currentRainUnit = value;
                RaisePropertyChanged(() => CurrentRainUnit);
            }
        }

        private string _message;

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        private long _lightPercent;

        public long LightPercent
        {
            get => _lightPercent;
            set
            {
                _lightPercent = value;
                RaisePropertyChanged(() => LightPercent);
            }
        }

        private string _lightState;

        public string LightState
        {
            get => _lightState;
            set
            {
                _lightState = value;
                RaisePropertyChanged(() => LightState);
            }
        }

        private string _emoticon;
        private static string _sentinelWeatherName;
        private static string _packageWeatherName;
        private static string _roomStateValueName;
        private static string _exteriorStateValueName;
        private static string _rainStateValueName;

        public string Emoticon
        {
            get => _emoticon;
            set
            {
                _emoticon = value;
                RaisePropertyChanged(() => Emoticon);
            }
        }

        public RelayCommand LightCommand { get; set; }

        public MainControlViewModel()
        {
            GetWeatherSettings();
            SubscribeOnStateValues();

            Messenger.Default.Register<SubscriptionToMainControlChanged>(this, OnSubscriptionChanged);

            _displayHourTimer = new Timer {Interval = 1000};
            _displayHourTimer.Elapsed += DisplayHourTimerElapsed;
            _displayHourTimer.Start();

            LightCommand = new RelayCommand(StartStopLight);
        }

        private static void SubscribeOnStateValues()
        {
            SubscribeOnOneWeatherStateValue(_roomStateValueName);
            SubscribeOnOneWeatherStateValue(_exteriorStateValueName);
            SubscribeOnOneWeatherStateValue(_rainStateValueName);
            SubscribeOnOneHueStateValue(SimpleIoc.Default.GetInstance<IClockSettings>().GetSetting<string>("LightStateValueOnOff"));
            SubscribeOnOneHueStateValue(SimpleIoc.Default.GetInstance<IClockSettings>().GetSetting<string>("LightStateValueDimmerPercent"));
        }

        private static void SubscribeOnOneWeatherStateValue(string stateValueName)
        {
            SimpleIoc.Default.GetInstance<INestorService>().SubscribeOnStateValue(_sentinelWeatherName,
                _packageWeatherName, stateValueName, ControlType.Main);
        }
        
        private static void SubscribeOnOneHueStateValue(string stateValueName)
        {
            SimpleIoc.Default.GetInstance<INestorService>().SubscribeOnStateValue("MaisonHue",
                "NestorHue", stateValueName, ControlType.Main);
        }

        private static void GetWeatherSettings()
        {
            var (sentinelWeatherName, packageWeatherName, roomStateValueName, exteriorStateValueName, rainStateValueName) =
                (ValueTuple<object, object, object, object, object>) SimpleIoc.Default
                    .GetInstance<IClockSettings>()
                    .GetCompositeSetting("WeatherSettings");
            _sentinelWeatherName = sentinelWeatherName.ToString();
            _packageWeatherName = packageWeatherName.ToString();
            _roomStateValueName = roomStateValueName.ToString();
            _exteriorStateValueName = exteriorStateValueName.ToString();
            _rainStateValueName = rainStateValueName.ToString();
        }

        private async void StartStopLight()
        {
            var lightsManager = CreateLightsManager();
            var lightToSwitch = await GetLightToSwitch(lightsManager);
            if (lightToSwitch != null)
            {
                if (!_lightToSwitchStarted)
                {
                    await lightsManager.StartLight(lightToSwitch);
                    _lightToSwitchStarted = true;
                }
                else
                {
                    await lightsManager.StopLight(lightToSwitch);
                    _lightToSwitchStarted = false;
                }
            }
        }

        private static LightsManager CreateLightsManager()
        {
            var (hueKey, hueServerIp) = (ValueTuple<object, object>) SimpleIoc.Default.GetInstance<IClockSettings>()
                .GetCompositeSetting("HueSettings");

            var client = new LocalClient(hueServerIp.ToString(), hueKey.ToString());
            return new LightsManager(client);
        }

        private async Task<Light> GetLightToSwitch(LightsManager lightsManager)
        {
            await lightsManager.RetrieveLights();
            var lightUniqueId = SimpleIoc.Default.GetInstance<IClockSettings>()
                .GetSetting<string>("LightUniqueId");
            var lights = lightsManager.Lights.Where(l => l.UniqueId == lightUniqueId);
            if (lights.Any())
            {
                return lights.First();
            }
            return null;
        }

        private async void DisplayHourTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var time = DateTime.Now;
            var diffMs = time.Millisecond - 100;
            _displayHourTimer.Interval = 1000 - diffMs / 2;
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                DisplayHour = time.ToShortTimeString();
            });

            ShowMessageIfNecessary(time);
        }

        private async void ShowMessageIfNecessary(DateTime time)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                if (time.TimeOfDay >= System.TimeSpan.Parse("22:30:00") || time.TimeOfDay <= TimeSpan.Parse("07:00:00"))
                {
                    var message = NestorMessageService.GetMessageAccordingToMessageType(NestorMessageType.Sleeping);
                    Message = message.Message;
                    Emoticon = message.Emoticon;
                }
                else
                {
                    Message = "";
                    Emoticon = "";
                }
            });
        }

        private async void OnSubscriptionChanged(SubscriptionToMainControlChanged message)
        {
            dynamic value = message.StateValue.Value;
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                ShowRoomStateValues(message, value);
                ShowExteriorStateValue(message, value);
                ShowRainStateValue(message, value);
                ShowLightStateValue(message, value);
            });
        }

        private void ShowLightStateValue(SubscriptionToMainControlChanged message, dynamic value)
        {
            if (message.StateValueKey.GetComponents().Item3 == SimpleIoc.Default.GetInstance<IClockSettings>().GetSetting<string>("LightStateValueOnOff"))
            {
                LightState = (bool)value ? @"/Assets/Icons/LightOn.png" : @"/Assets/Icons/LightOff.png";
            }
            
            if (message.StateValueKey.GetComponents().Item3 == SimpleIoc.Default.GetInstance<IClockSettings>().GetSetting<string>("LightStateValueDimmerPercent"))
            {
                LightPercent = value;
            }
        }

        private void ShowRainStateValue(SubscriptionToMainControlChanged message, dynamic value)
        {
            if (message.StateValueKey.GetComponents().Item3 == _rainStateValueName)
            {
                CurrentRain = value.CurrentRain.Value;
                CurrentRainUnit = value.CurrentRain.Unit;
            }
        }

        private void ShowExteriorStateValue(SubscriptionToMainControlChanged message, dynamic value)
        {
            if (message.StateValueKey.GetComponents().Item3 == _exteriorStateValueName)
            {
                ExteriorTemperature = value.Temperature.Value;
                ExteriorTemperatureUnit = value.Temperature.Unit;
            }
        }

        private void ShowRoomStateValues(SubscriptionToMainControlChanged message, dynamic value)
        {
            if (message.StateValueKey.GetComponents().Item3 == _roomStateValueName)
            {
                InteriorTemperature = value.Temperature.Value;
                InteriorTemperatureUnit = value.Temperature.Unit;
                InteriorHumidity = value.Humidity.Value;
                InteriorHumidityUnit = value.Humidity.Unit;
                InteriorAirQuality = value.Co2.Value;
                InteriorAirQualityUnit = value.Co2.Unit;
            }
        }
    }
}
