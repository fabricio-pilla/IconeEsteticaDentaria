using System;

namespace IconeEsteticaDentaria.Comum.Validator
{
    public static class Validator
    {
        public static bool ValidaCPF(string cpf)
        {
            bool Retorno = true;
            cpf = cpf.Replace(".", "").Replace("-", "").Replace(" ", "").Replace("_", "");
            if (cpf.Length != 11) return false;

            switch (cpf)
            {
                case "11111111111": Retorno = false;
                    break;
                case "22222222222": Retorno = false;
                    break;
                case "33333333333": Retorno = false;
                    break;
                case "44444444444": Retorno = false;
                    break;
                case "55555555555": Retorno = false;
                    break;
                case "66666666666": Retorno = false;
                    break;
                case "77777777777": Retorno = false;
                    break;
                case "88888888888": Retorno = false;
                    break;
                case "99999999999": Retorno = false;
                    break;
                case "00000000000": Retorno = false;
                    break;
            }
            if (Retorno)
            {
                int dig1 = 0;
                int dig2 = 0;
                int Mult1 = 0;
                int Mult2 = 0;
                int X = 0;
                Mult1 = 10;
                Mult2 = 11;

                for (X = 1; X <= 9; X += 1)
                {
                    dig1 = dig1 + Convert.ToInt32(cpf.Substring(X - 1, 1)) * Mult1;
                    Mult1 = Mult1 - 1;
                }

                for (X = 1; X <= 10; X += 1)
                {
                    dig2 = dig2 + Convert.ToInt32(cpf.Substring(X - 1, 1)) * Mult2;
                    Mult2 = Mult2 - 1;
                }

                System.Math.DivRem((dig1 * 10), 11, out dig1);
                System.Math.DivRem((dig2 * 10), 11, out dig2);

                if (dig1 == 10) dig1 = 0;
                if (dig2 == 10) dig2 = 0;

                if (Convert.ToInt32(cpf.Substring(9, 1)) != dig1) return false;

                if (Convert.ToInt32(cpf.Substring(10, 1)) != dig2) return false;

                return true;
            }
            else
                return Retorno;
        }

        public static bool StringValida(string texto)
        {
            if (texto != null)
            {
                var textoTrim = texto.Trim();
                if (textoTrim != "")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
