using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Helpers.Validations
{
    class Validate_NumCasa
    {
        public static bool EsNumeroCasaValido(string numeroCasa)
        {
            // Expresión regular para validar el formato del número de casa
            string patronNumeroCasa = @"^#\d+(-[A-Z]+)?$";
            return Regex.IsMatch(numeroCasa, patronNumeroCasa);
        }
    }
}
