using System.Globalization;

namespace NestorHub.Netatmo.Common.Class
{
    public static class StringExtension
    {
        public static double ConvertToDoubleFromNetatmo(this string data)
        {
            if (double.TryParse(data, NumberStyles.Any, new CultureInfo("en-us"), out double result))
            {
                return result;
            }
            return 0;
        }

        public static long ConvertToLongFromNetatmo(this string data)
        {
            if (long.TryParse(data, out long result))
            {
                return result;
            }
            return 0;
        }

        public static int ConvertToIntFromNetatmo(this string data)
        {
            if (int.TryParse(data, out int result))
            {
                return result;
            }
            return 0;
        }
    }
}
