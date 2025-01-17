using System;
using System.Text.RegularExpressions;

namespace IconeEsteticaDentaria.Comum.Extensions
{
    public static class FormatExtension
    {
        public static String GetOnlyNumbers(this object value)
        {
            if (string.IsNullOrEmpty(value.ToString()))
                return "";

            var result = Regex.Replace(value.ToString(), "[^0-9]", "");
            if (string.IsNullOrEmpty(result))
                return "";

            return result;
        }
    }
}
