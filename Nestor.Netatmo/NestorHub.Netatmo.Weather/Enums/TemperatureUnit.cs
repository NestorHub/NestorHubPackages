using Kopigi.Utils.Class;

namespace NestorHub.Netatmo.Weather.Enums
{
    public enum TemperatureUnit
    {
        [StringEnum("°C")]
        Celsius,

        [StringEnum("°F")]
        Fahrenheit,

        [StringEnum("Undefined")]
        Undefined
    }
}