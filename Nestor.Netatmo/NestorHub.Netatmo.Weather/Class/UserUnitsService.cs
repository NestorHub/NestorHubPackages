using NestorHub.Netatmo.Weather.Enums;
using NestorHub.Netatmo.Weather.Interfaces;

namespace NestorHub.Netatmo.Weather.Class
{
    public class UserUnitsService : IUserUnitsService
    {
        private UserUnits _userUnits;

        public void SetUserUnits(UserUnits userUnits)
        {
            _userUnits = userUnits;
        }

        public TemperatureUnit GetTemperatureUnit()
        {
            if (_userUnits != null)
            {
                return _userUnits.Temperature;
            }

            return TemperatureUnit.Undefined;
        }

        public PressureUnit GetPressureUnit()
        {
            if (_userUnits != null)
            {
                return _userUnits.Pressure;
            }

            return PressureUnit.Undefined;
        }

        public RainUnit GetRainUnit()
        {
            if (_userUnits != null)
            {
                return _userUnits.Rain;
            }

            return RainUnit.Undefined;
        }

        public WindUnit GetWindUnit()
        {
            if (_userUnits != null)
            {
                return _userUnits.Wind;
            }

            return WindUnit.Undefined;
        }
    }
}