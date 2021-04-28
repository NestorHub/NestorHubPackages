using System;

namespace NestorHub.Netatmo.Weather.Class.Dashboards.WeatherValue
{
    [Serializable]
    public class Co2 : WeatherValue
    {
        public Co2(double value) 
            : base(value, "ppm")
        {}
    }
}