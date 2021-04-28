using NestorHub.Netatmo.Weather.Class;
using NestorHub.Netatmo.Weather.Enums;

namespace NestorHub.Netatmo.Weather.Interfaces
{
    public interface IUserUnitsService
    {
        void SetUserUnits(UserUnits userUnits);
        TemperatureUnit GetTemperatureUnit();
        PressureUnit GetPressureUnit();
        RainUnit GetRainUnit();
        WindUnit GetWindUnit();
    }
}