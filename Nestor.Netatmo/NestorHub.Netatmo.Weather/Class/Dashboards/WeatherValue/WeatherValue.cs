namespace NestorHub.Netatmo.Weather.Class.Dashboards.WeatherValue
{
    public class WeatherValue
    {
        public double Value { get; }
        public string Unit { get; }

        protected WeatherValue(double value, string unit)
        {
            Value = value;
            Unit = unit;
        }
    }
}