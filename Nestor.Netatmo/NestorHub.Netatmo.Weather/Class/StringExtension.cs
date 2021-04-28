using NestorHub.Netatmo.Weather.Enums;

namespace NestorHub.Netatmo.Weather.Class
{
    public static class StringExtension
    {
        public static DeviceType GetTypeOfDevice(this string type)
        {
            switch (type.ToUpper())
            {
                case "NAMAIN":
                    return DeviceType.BaseStation;
                case "NAMODULE1":
                    return DeviceType.OutdoorModule;
                case "NAMODULE2":
                    return DeviceType.WinGauge;
                case "NAMODULE3":
                    return DeviceType.RainGauge;
                case "NAMODULE4":
                    return DeviceType.IndoorModule;
                default:
                    return DeviceType.UnknownModule;
            }
        }

        public static Trend GetTrend(this string type)
        {
            switch (type.ToUpper())
            {
                case "UP":
                    return Trend.Up;
                case "DOWN":
                    return Trend.Down;
                case "STABLE":
                    return Trend.Stable;
                default:
                    return Trend.Stable;
            }
        }

    }
}
