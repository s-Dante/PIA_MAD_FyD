using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Helpers.Validations
{
    class Validate_CodigoPostal
    {
        public static bool EsCodigoPostalValido(string codigoPostal)
        {
            string patronCodigoPostal = @"^\d{5}$";
            return Regex.IsMatch(codigoPostal, patronCodigoPostal);
        }
    }
}
