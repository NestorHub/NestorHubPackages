using System;

namespace NestorHub.Netatmo.Weather.Class.Dashboards.WeatherValue
{
    [Serializable]
    public class Humidity : WeatherValue
    {
        public Humidity(double value)
            : base (value, "%")
        {}
    }
}