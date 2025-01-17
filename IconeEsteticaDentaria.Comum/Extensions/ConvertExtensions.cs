using System;

namespace IconeEsteticaDentaria.Comum.Extensions
{
    public static class ConvertExtensions
    {
        public static bool ToBool(this object value, bool minvalue = false)
        {
            var valor = minvalue;
            if (value != null)
                Boolean.TryParse(value.ToString(), out valor);
            return valor;
        }

        public static double ToDouble(this object value, double minvalue = 0)
        {
            var number = minvalue;
            if (value != null)
                Double.TryParse(value.ToString(), out number);
            return number;
        }

        public static int BitToInt(this object value, int minvalue = 0)
        {
            var number = minvalue;
            if (value != null)
            {
                var boleano = value.ToBool();
                number = boleano == true ? 1 : 0;
            }
            return number;
        }

        public static int ToInt(this object value, int minvalue = 0)
        {
            var number = minvalue;
            if (value != null)
                Int32.TryParse(value.ToString(), out number);
            return number;
        }

        public static Int16 ToInt16(this object value, Int16 minvalue = 0)
        {
            var number = minvalue;
            if (value != null)
                Int16.TryParse(value.ToString(), out number);
            return number;
        }

        public static Int64 ToInt64(this object value, Int64 minvalue = 0)
        {
            var number = minvalue;
            if (value != null)
                Int64.TryParse(value.ToString(), out number);
            return number;
        }

        public static float ToFloat(this object value, float minvalue = 0)
        {
            var number = minvalue;
            if (value != null)
                float.TryParse(value.ToString(), out number);
            return number;
        }

        public static Byte ToByte(this object value, byte minvalue = 0)
        {
            var number = minvalue;
            if (value != null)
                Byte.TryParse(value.ToString(), out number);
            return number;
        }

        public static decimal ToDecimal(this object value)
        {
            decimal number = 0;
            if (value != null)
                Decimal.TryParse(value.ToString().Replace("R$", "").Replace(" ", ""), out number);
            return number;
        }

        public static DateTime ToDateTime(this object value)
        {
            var date = DateTime.MinValue;
            if (value != null)
                DateTime.TryParse(value.ToString(), out date);
            return date;
        }

        public static char ToChar(this object value, char minvalue = ' ')
        {
            var ch = minvalue;
            if (value != null)
                Char.TryParse(value.ToString(), out ch);
            return ch;
        }
    }
}
