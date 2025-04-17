using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class Cancelacion
    {
        public int id_Cancelacion { get; set; }
        public DateTime fecha_Cancelacion { get; set; }
        public string motivo { get; set; } 
        public int usuario_Cancelador { get; set; }
        public Guid id_Reservacion { get; set; }

        //Constructor
        public Cancelacion(int id_Cancelacion, DateTime fecha_Cancelacion, string motivo,
                            int usuario_Cancelador, Guid id_Reservacion)
        {
            this.id_Cancelacion = id_Cancelacion;
            this.fecha_Cancelacion = fecha_Cancelacion;
            this.motivo = motivo;
            this.usuario_Cancelador = usuario_Cancelador;
            this.id_Reservacion = id_Reservacion;
        }
    }
}
