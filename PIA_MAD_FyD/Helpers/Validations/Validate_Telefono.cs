using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Helpers.Validations
{
    class Validate_Telefono
    {
        public static bool EsTelefonoValido(string telefono)
        {
            string patronTelefono = @"^\d{3}-\d{3}-\d{4}$"; // 123-456-7890
            return Regex.IsMatch(telefono, patronTelefono);
        }
    }
}
