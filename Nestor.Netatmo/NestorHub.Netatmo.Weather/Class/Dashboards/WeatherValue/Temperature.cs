using System;
using Kopigi.Utils.Helpers;
using NestorHub.Netatmo.Weather.Enums;

namespace NestorHub.Netatmo.Weather.Class.Dashboards.WeatherValue
{
    [Serializable]
    public class Temperature : WeatherValue
    {
        public Temperature(double value, TemperatureUnit unit)
            : base(value, StringEnum.GetStringValue(unit))
        {}
    }
}