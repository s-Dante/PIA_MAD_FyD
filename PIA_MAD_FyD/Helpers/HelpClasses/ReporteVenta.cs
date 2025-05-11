using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Helpers.HelpClasses
{
    public class ReporteVenta
    {
        public string NombreHotel { get; set; }
        public string Ciudad { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public decimal IngresosHospedaje { get; set; }
        public decimal ServiciosExtra { get; set; }
        public decimal IngresoTotal { get; set; }


        public ReporteVenta() { }
    }
}
