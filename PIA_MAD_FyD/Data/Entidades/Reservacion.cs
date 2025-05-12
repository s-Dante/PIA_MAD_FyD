using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class Reservacion
    {
        public Guid id_Reservacion { get; set; }
        public DateTime fecha_Ini { get; set; }
        public DateTime fecha_Fin { get; set; }
        public int cant_Habitaciones { get; set; }
        public int cant_Huespedes { get; set; }
        public float anticipo_Pagado { get; set; }
        public DateTime fecha_Registro { get; set; }
        public int usuario_Registrador { get; set; }
        public int usuario_Modifico { get; set; }
        public int id_Cliente { get; set; }

        //Constructor
        public Reservacion() { }

        public Reservacion(Guid id_Reservacion, DateTime fecha_Ini, DateTime fecha_Fin, int cant_Habitaciones,
                            int cant_Huespedes, float anticipo_Pagado, DateTime fecha_Registro, int usuario_Registrador,
                            int usuario_Modifico, int id_Cliente)
        {
            this.id_Reservacion = id_Reservacion;
            this.fecha_Ini = fecha_Ini;
            this.fecha_Fin = fecha_Fin;
            this.cant_Habitaciones = cant_Habitaciones;
            this.cant_Huespedes = cant_Huespedes;
            this.anticipo_Pagado = anticipo_Pagado;
            this.fecha_Registro = fecha_Registro;
            this.usuario_Registrador = usuario_Registrador;
            this.usuario_Modifico = usuario_Modifico;
            this.id_Cliente = id_Cliente;
        }
    }
}
