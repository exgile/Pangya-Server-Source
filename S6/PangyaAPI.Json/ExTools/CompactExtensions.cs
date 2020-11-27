using System;
using System.Globalization;
namespace PangyaAPI.Json.ExTools
{
    public static class CompactExtensions
    {
        private const NumberStyles JsonNumbers = NumberStyles.Float;

        public static bool TryParse(this string input, out double result)
        {
            try
            {
                result = double.Parse(input, JsonNumbers, CultureInfo.InvariantCulture);
                return true;
            }
            catch (System.Exception)
            {
                result = 0;
                return false;
            }
        }
    }
}
