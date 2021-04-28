using Kopigi.Utils.Helpers;
using NestorHub.Netatmo.Weather.Enums;

namespace NestorHub.Netatmo.Weather.Class.Dashboards.WeatherValue
{
    public class Wind : WeatherValue
    {
        public Wind(double value, WindUnit unit) 
            : base(value, StringEnum.GetStringValue(unit))
        {}
    }
}