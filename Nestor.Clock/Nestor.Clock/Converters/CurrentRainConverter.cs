
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Nestor.Clock.Converters
{
    public class CurrentRainConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && double.TryParse(value.ToString(), NumberStyles.Any, new CultureInfo("en-us"), out var currentRain))
            {
                if (currentRain > 0)
                {
                    return new BitmapImage(new Uri("ms-appx:///Assets/Icons/Raining.png", UriKind.Absolute));
                }
                return new BitmapImage(new Uri("ms-appx:///Assets/Icons/NoRain.png", UriKind.Absolute));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
