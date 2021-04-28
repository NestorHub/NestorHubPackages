using NestorHub.Netatmo.Common.Class;
using NestorHub.Netatmo.Weather.Enums;
using Newtonsoft.Json;

namespace NestorHub.Netatmo.Weather.Class
{
    public class UserUnits
    {
        public TemperatureUnit Temperature { get; set; }
        public RainUnit Rain { get; set; }
        public WindUnit Wind { get; set; }
        public PressureUnit Pressure { get; set; }

        [JsonConstructor]
        public UserUnits(string unit, string windunit, string pressureunit)
        {
            Temperature = unit.ConvertToIntFromNetatmo().GetTemperatureUnit();
            Rain = unit.ConvertToIntFromNetatmo().GetRainUnit();
            Wind = windunit.ConvertToIntFromNetatmo().GetWindUnit();
            Pressure = pressureunit.ConvertToIntFromNetatmo().GetPressureUnit();
        }
    }
}