using System;
using Kopigi.Utils.Helpers;
using NestorHub.Netatmo.Weather.Enums;

namespace NestorHub.Netatmo.Weather.Class.Dashboards.WeatherValue
{
    [Serializable]
    public class Pressure : WeatherValue
    {
        public Pressure(double value, PressureUnit unit)
            : base(value, StringEnum.GetStringValue(unit))
        {}
    }
}