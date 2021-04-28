using NestorHub.Netatmo.Weather.Class;
using NestorHub.Netatmo.Weather.Enums;
using NFluent;
using Xunit;

namespace NestorHub.Netatmo.Weather.Tests
{
    public class UserUnitsServiceShould
    {
        [Fact]
        public void return_temperature_undefined_if_no_user_units_retrieve()
        {
            var userUnitsService = new UserUnitsService();
            Check.That(userUnitsService.GetTemperatureUnit()).IsEqualTo(TemperatureUnit.Undefined);
        }

        [Fact]
        public void return_temperature_celsius_when_user_units_defined()
        {
            var userUnitsService = new UserUnitsService();
            userUnitsService.SetUserUnits(new UserUnits("0", "0", "0"));
            Check.That(userUnitsService.GetTemperatureUnit()).IsEqualTo(TemperatureUnit.Celsius);
        }

        [Fact]
        public void return_pressure_undefined_if_no_user_units_retrieve()
        {
            var userUnitsService = new UserUnitsService();
            Check.That(userUnitsService.GetPressureUnit()).IsEqualTo(PressureUnit.Undefined);
        }

        [Fact]
        public void return_pressure_celsius_when_user_units_defined()
        {
            var userUnitsService = new UserUnitsService();
            userUnitsService.SetUserUnits(new UserUnits("0", "0", "0"));
            Check.That(userUnitsService.GetPressureUnit()).IsEqualTo(PressureUnit.Mbar);
        }

        [Fact]
        public void return_rain_undefined_if_no_user_units_retrieve()
        {
            var userUnitsService = new UserUnitsService();
            Check.That(userUnitsService.GetRainUnit()).IsEqualTo(RainUnit.Undefined);
        }

        [Fact]
        public void return_rain_mm_when_user_units_defined()
        {
            var userUnitsService = new UserUnitsService();
            userUnitsService.SetUserUnits(new UserUnits("0", "0", "0"));
            Check.That(userUnitsService.GetRainUnit()).IsEqualTo(RainUnit.Millimeters);
        }

        [Fact]
        public void return_wind_undefined_if_no_user_units_retrieve()
        {
            var userUnitsService = new UserUnitsService();
            Check.That(userUnitsService.GetWindUnit()).IsEqualTo(WindUnit.Undefined);
        }

        [Fact]
        public void return_wind_kph_when_user_units_defined()
        {
            var userUnitsService = new UserUnitsService();
            userUnitsService.SetUserUnits(new UserUnits("0", "0", "0"));
            Check.That(userUnitsService.GetWindUnit()).IsEqualTo(WindUnit.Kph);
        }
    }
}
