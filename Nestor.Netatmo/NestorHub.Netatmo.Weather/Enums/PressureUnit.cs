using Kopigi.Utils.Class;

namespace NestorHub.Netatmo.Weather.Enums
{
    public enum PressureUnit
    {
        [StringEnum("mbar")]
        Mbar,

        [StringEnum("inHg")]
        InHg,

        [StringEnum("mmHg")]
        MmHg,

        [StringEnum("Undefined")]
        Undefined
    }
}