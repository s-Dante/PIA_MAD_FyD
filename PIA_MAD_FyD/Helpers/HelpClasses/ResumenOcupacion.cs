using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Helpers.HelpClasses
{
    public class ResumenOcupacion
    {
        public string Ciudad { get; set; }
        public string NombreHotel { get; set; }
        public int Anio { get; set; }
        public string Mes { get; set; }
        public decimal PorcentajeOcupacion { get; set; }


       public ResumenOcupacion() { }
    }
}
