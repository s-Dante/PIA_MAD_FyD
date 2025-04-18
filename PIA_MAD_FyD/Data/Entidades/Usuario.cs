using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    public class Usuario
    {
        public int num_Nomina { get; set; }
        public string nombre { get; set; }
        public string apellido_Paterno { get; set; }
        public string apellido_Materno { get; set; }
        public string correo { get; set; }
        public DateTime fecha_Nacimiento { get; set; }
        public string telefono { get; set; } // Puede ser "A" = Administrado o "O" = Operativo
        public char tipo_Usuario { get; set; }
        public DateTime fecha_Registro { get; set; }
        public DateTime fecha_Modificaion { get; set; }
        public char estatus { get; set; } // Puede ser 'A' para activo o 'B' para inactivo
        public int usuario_Registrador { get; set; }
        public int usuario_Modifico { get; set; }
        // Constructor
        public Usuario() { }
        public Usuario(int num_Nomina, string nombre, string apellido_Paterno, string apellid_materno,
                        string correo, DateTime fecha_Nacimiento, string telefono, char tipo_Usuario,
                        DateTime fecha_Registro, DateTime fecha_Modificacion, char estatus, int usuario_Registrador, int usuario_Modifico)
        {
            this.num_Nomina = num_Nomina;
            this.nombre = nombre;
            this.apellido_Paterno = apellido_Paterno;
            this.apellido_Materno = apellid_materno;
            this.correo = correo;
            this.fecha_Nacimiento = fecha_Nacimiento;
            this.telefono = telefono;
            this.tipo_Usuario = tipo_Usuario;
            this.fecha_Registro = fecha_Registro;
            this.fecha_Modificaion = fecha_Modificacion;
            this.estatus = estatus;
            this.usuario_Registrador = usuario_Registrador;
            this.usuario_Modifico = usuario_Modifico;
        }
    }
}
