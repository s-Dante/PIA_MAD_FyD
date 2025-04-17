using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Helpers.Validations
{
    class Validate_RFC
    {
        public static bool EsRFCValido(string rfc)
        {
            // Expresión regular para validar el formato del RFC
            string patronRFC = @"^([A-ZÑ&]{3,4})(\d{6})([A-Z\d]{3})$";
            return Regex.IsMatch(rfc, patronRFC);
        }
    }
}
