using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace Nestor.Clock
{
    public sealed partial class MainView : Page
    {
        public MainView()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchViewSize = new Size(800, 480);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            this.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ApplicationView.GetForCurrentView().TryResizeView(new Size(800, 480));
        }

        private async void OnFlipSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((FlipView)sender).SelectedIndex != 1)
            {
                await ApplyBlurEffectOnBackground(10);
            }
            else
            {
                await ApplyBlurEffectOnBackground(0);
            }
        }

        private async Task ApplyBlurEffectOnBackground(int blurValue)
        {
            await BackgroundBrush.Blur(blurValue, 500, 0, EasingType.Cubic, EasingMode.EaseInOut).StartAsync();
        }
    }
}
