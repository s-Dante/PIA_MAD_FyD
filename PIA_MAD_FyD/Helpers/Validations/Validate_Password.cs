using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Helpers.Validations
{
    class Validate_Password
    {
        public static bool EsPasswordValida(string contraseña)
        {
            string patronContraseña = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
            return Regex.IsMatch(contraseña, patronContraseña);
        }
    }
}
