using Kopigi.Utils.Class;

namespace NestorHub.Netatmo.Weather.Enums
{
    public enum RainUnit
    {
        [StringEnum("mm")]
        Millimeters,

        [StringEnum("inch")]
        Inch,

        [StringEnum("Undefined")]
        Undefined
    }
}