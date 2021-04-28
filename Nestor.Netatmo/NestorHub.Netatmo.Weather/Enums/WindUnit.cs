using Kopigi.Utils.Class;

namespace NestorHub.Netatmo.Weather.Enums
{
    public enum WindUnit
    {
        [StringEnum("km/h")]
        Kph,

        [StringEnum("mph")]
        Mph,

        [StringEnum("m/s")]
        Ms,

        [StringEnum("beaufort")]
        Beaufort,

        [StringEnum("knots")]
        Knot,

        [StringEnum("Undefined")]
        Undefined
    }
}