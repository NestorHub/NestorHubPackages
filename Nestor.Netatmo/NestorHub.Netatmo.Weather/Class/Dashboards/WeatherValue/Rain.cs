using System;
using Kopigi.Utils.Helpers;
using NestorHub.Netatmo.Weather.Enums;

namespace NestorHub.Netatmo.Weather.Class.Dashboards.WeatherValue
{
    [Serializable]
    public class Rain : WeatherValue
    {
        public Rain(double value, RainUnit unit) 
            : base(value, StringEnum.GetStringValue(unit))
        {}

        public Rain(double value, string unit)
            : base(value, unit)
        {}
    }
}