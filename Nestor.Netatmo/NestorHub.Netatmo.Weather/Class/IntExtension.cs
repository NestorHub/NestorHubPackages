using NestorHub.Netatmo.Weather.Enums;

namespace NestorHub.Netatmo.Weather.Class
{
    public static class IntExtension
    {
        public static TemperatureUnit GetTemperatureUnit(this int type)
        {
            switch (type)
            {
                case 0:
                    return TemperatureUnit.Celsius;
                case 1:
                    return TemperatureUnit.Fahrenheit;
                default:
                    return TemperatureUnit.Celsius;
            }
        }

        public static RainUnit GetRainUnit(this int type)
        {
            switch (type)
            {
                case 0:
                    return RainUnit.Millimeters;
                case 1:
                    return RainUnit.Inch;
                default:
                    return RainUnit.Millimeters;
            }
        }

        public static WindUnit GetWindUnit(this int type)
        {
            switch (type)
            {
                case 0:
                    return WindUnit.Kph;
                case 1:
                    return WindUnit.Mph;
                case 2:
                    return WindUnit.Ms;
                case 3:
                    return WindUnit.Beaufort;
                case 4:
                    return WindUnit.Knot;
                default:
                    return WindUnit.Kph;
            }
        }

        public static PressureUnit GetPressureUnit(this int type)
        {
            switch (type)
            {
                case 0:
                    return PressureUnit.Mbar;
                case 1:
                    return PressureUnit.InHg;
                case 2:
                    return PressureUnit.MmHg;
                default:
                    return PressureUnit.Mbar;
            }
        }
    }
}
