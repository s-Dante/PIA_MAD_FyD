using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class Descuento
    {
        public int id_Descuento { get; set; }
        public string nombre { get; set; }
        public float porcentaje { get; set; }
        public string descripcion { get; set; }

        //Constructor
        public Descuento(int id_Descuento, string nombre, float porcentaje, string descripcion)
        {
            this.id_Descuento = id_Descuento;
            this.nombre = nombre;
            this.porcentaje = porcentaje;
            this.descripcion = descripcion;
        }
    }
}
