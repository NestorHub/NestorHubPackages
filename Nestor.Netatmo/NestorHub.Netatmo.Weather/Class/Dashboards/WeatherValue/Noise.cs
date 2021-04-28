using System;

namespace NestorHub.Netatmo.Weather.Class.Dashboards.WeatherValue
{
    [Serializable]
    public class Noise : WeatherValue
    {
        public Noise(double value)
            : base (value, "db")
        {}
    }
}