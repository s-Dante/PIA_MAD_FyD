using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class ServiciosExtra
    {
        public int id_ServicioExtrta { get; set; }
        public string nombre { get; set; }
        public decimal precion { get; set; }
        public string descripcion { get; set; }


        public override string ToString()
        {
            return nombre + "   |   $" + precion;
        }
        //Constructor
        public ServiciosExtra() { }
        public ServiciosExtra(int id_ServicioExtrta, string nombre, decimal precion, string descripcion)
        {
            this.id_ServicioExtrta = id_ServicioExtrta;
            this.nombre = nombre;
            this.precion = precion;
            this.descripcion = descripcion;
        }
    }
}
