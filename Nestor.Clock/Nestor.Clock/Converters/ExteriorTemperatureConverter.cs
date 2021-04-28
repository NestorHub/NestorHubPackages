
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
    public class ExteriorTemperatureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && double.TryParse(value.ToString(), NumberStyles.Any, new CultureInfo("en-us"), out var currentTemperature))
            {
                if (currentTemperature <= 0)
                {
                    return new BitmapImage(new Uri("ms-appx:///Assets/Icons/TempLowerZero.png", UriKind.Absolute));
                }

                if (currentTemperature > 0 && currentTemperature <= 10)
                {
                    return new BitmapImage(new Uri("ms-appx:///Assets/Icons/TempZeroTen.png", UriKind.Absolute));
                }

                if (currentTemperature > 10 && currentTemperature <= 20)
                {
                    return new BitmapImage(new Uri("ms-appx:///Assets/Icons/TempTenTwenty.png", UriKind.Absolute));
                }

                if (currentTemperature > 20 && currentTemperature <= 30)
                {
                    return new BitmapImage(new Uri("ms-appx:///Assets/Icons/TempTwentyThirty.png", UriKind.Absolute));
                }

                if (currentTemperature > 30)
                {
                    return new BitmapImage(new Uri("ms-appx:///Assets/Icons/TempUpperThirty.png", UriKind.Absolute));
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
