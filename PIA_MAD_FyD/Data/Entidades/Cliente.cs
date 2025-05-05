using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class Cliente
    {
        public string rfc { get; set; }
        public string nombre { get; set; }
        public string apellido_Paterno { get; set; }
        public string apellido_Materno { get; set; }
        public string correo { get; set; }
        public DateTime fecha_Nacimiento { get; set; }
        public char estado_Civil { get; set; }
        public DateTime fecha_Registro { get; set; }
        public DateTime fecha_Modifico { get; set; }
        public int usuario_Registrador { get; set; }
        public int usuario_Modifico { get; set; }
        public int ubicacion { get; set; }

        public string telefono { get; set; } // Agregado para el teléfono

        public Ubiacacion ubicacion_Cliente { get; set; } // Relación con la clase Ubiacacion

        public Cliente() { }
        // Constructor
        public Cliente(string rfc, string nombre, string apellido_Paterno, string apellido_Materno,
                        string correo, DateTime fecha_Nacimiento, char estado_Civil, DateTime fecha_Registro,
                        DateTime fecha_Modifico, int usuario_Registrador, int usuario_Modifico, int ubicacion)
        {
            this.rfc = rfc;
            this.nombre = nombre;
            this.apellido_Paterno = apellido_Paterno;
            this.apellido_Materno = apellido_Materno;
            this.correo = correo;
            this.fecha_Nacimiento = fecha_Nacimiento;
            this.estado_Civil = estado_Civil;
            this.fecha_Registro = fecha_Registro;
            this.fecha_Modifico = fecha_Modifico;
            this.usuario_Registrador = usuario_Registrador;
            this.usuario_Modifico = usuario_Modifico;
            this.ubicacion = ubicacion;
        }
    }
}
