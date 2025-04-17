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
        public float precion { get; set; }
        public string descripcion { get; set; }

        //Constructor
        public ServiciosExtra(int id_ServicioExtrta, string nombre, float precion, string descripcion)
        {
            this.id_ServicioExtrta = id_ServicioExtrta;
            this.nombre = nombre;
            this.precion = precion;
            this.descripcion = descripcion;
        }
    }
}
