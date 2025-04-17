using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class Habitacion
    {
        public int id_Habitacion { get; set; }
        public int num_Camas { get; set; }
        public char tipo_Cama { get; set; }
        public float precio { get; set; }
        public int capacidad { get; set; }
        public char nivel { get; set; }
        public char vista { get; set; }
        public DateTime fecha_Registro { get; set; }
        public DateTime fecha_Modifico { get; set; }
        public int usuario_Registrador { get; set; }
        public int usuario_Modifico { get; set; }
        public int id_Hotel { get; set; }

        // Constructor
        public Habitacion(int id_Habitacion, int num_Camas, char tipo_Cama, float precio, int capacidad,
                            char nivel, char vista, DateTime fecha_Registro, DateTime fecha_Modifico, 
                            int usuario_Registrador, int usuario_Modifico, int id_Hotel)
        {
            this.id_Habitacion = id_Habitacion;
            this.num_Camas = num_Camas;
            this.tipo_Cama = tipo_Cama;
            this.precio = precio;
            this.capacidad = capacidad;
            this.nivel = nivel;
            this.vista = vista;
            this.fecha_Registro = fecha_Registro;
            this.fecha_Modifico = fecha_Modifico;
            this.usuario_Registrador = usuario_Registrador;
            this.usuario_Modifico = usuario_Modifico;
            this.id_Hotel = id_Hotel;
        }
    }
}
