using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email.DataProvider;
using GalaSoft.MvvmLight.Ioc;
using Navegar.Libs.Interfaces;
using Navegar.Plateformes.NetCore.UWP.Win10;
using Nestor.Clock.Class;
using Nestor.Clock.Interfaces;

namespace Nestor.Clock.ViewModel
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModelInstance => SimpleIoc.Default.GetInstance<MainViewModel>();
        public MainControlViewModel MainControlViewModelInstance => SimpleIoc.Default.GetInstance<MainControlViewModel>();

        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MainControlViewModel>();
            
            RegisterServices();
            AssociateViewModelsToViews();
            SetClockSettings();
        }

        private void SetClockSettings()
        {
            SimpleIoc.Default.GetInstance<IClockSettings>().SaveCompositeSetting("HueSettings", 
                new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("HueKey", "Ta519Bn0t3JvVLORifBtAQhk09RqI6ki2DUBuNQi"),
                    new KeyValuePair<string, object>("HueServerIp", "192.168.1.26"),
                });

            SimpleIoc.Default.GetInstance<IClockSettings>().SaveCompositeSetting("WeatherSettings",
                new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("SentinelWeatherName", "MaisonWeather"),
                    new KeyValuePair<string, object>("PackageWeatherName", "NestorWeather"),
                    new KeyValuePair<string, object>("RoomStateValueName", "Interieur"),
                    new KeyValuePair<string, object>("ExteriorStateValueName", "Exterieur"),
                    new KeyValuePair<string, object>("RainStateValueName", "Pluviometre"),
                });

            SimpleIoc.Default.GetInstance<IClockSettings>().SaveSetting("LightUniqueId", "00:17:88:01:04:3d:08:24-0b");
            SimpleIoc.Default.GetInstance<IClockSettings>().SaveSetting("LightStateValueOnOff", "Salon1");
            SimpleIoc.Default.GetInstance<IClockSettings>().SaveSetting("LightStateValueDimmerPercent", "Salon1DimmerPercent");
        }

        private void AssociateViewModelsToViews()
        {
            SimpleIoc.Default.GetInstance<INavigationUwp>().RegisterView<MainViewModel, MainView>();
        }

        private void RegisterServices()
        {
            if (!SimpleIoc.Default.IsRegistered<INavigationUwp>())
            {
                SimpleIoc.Default.Register<INavigationUwp, Navigation>();
            }

            if (!SimpleIoc.Default.IsRegistered<INestorService>())
            {
                SimpleIoc.Default.Register<INestorService, NestorService>();
            }

            if (!SimpleIoc.Default.IsRegistered<IClockSettings>())
            {
                SimpleIoc.Default.Register<IClockSettings, ClockSettings>();
            }
        }
    }
}
