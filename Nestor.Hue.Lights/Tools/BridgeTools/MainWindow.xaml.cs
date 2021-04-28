using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Bridge;

namespace BridgeTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IEnumerable<LocatedBridge> _bridgeIPs;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void GetIpOnClick(object sender, RoutedEventArgs e)
        {
            IBridgeLocator locator = new HttpBridgeLocator();
            _bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(10));
            if (_bridgeIPs.Any())
            {
                IP.Text = _bridgeIPs.First().IpAddress;
            }
            else
            {
                IP.Text = "Aucun bridge trouvé";
            }
        }

        private async void GetKeyOnClick(object sender, RoutedEventArgs e)
        {
            if (_bridgeIPs.Any())
            {
                ILocalHueClient client = new LocalHueClient(_bridgeIPs.First().IpAddress);
                var appKey = await client.RegisterAsync("Nestor.Lights.Hue", "Maison");
                if (!string.IsNullOrEmpty(appKey))
                {
                    Key.Text = appKey;
                }
                else
                {
                    Key.Text = "Aucune clé trouvée";
                }
            }
        }
    }
}
