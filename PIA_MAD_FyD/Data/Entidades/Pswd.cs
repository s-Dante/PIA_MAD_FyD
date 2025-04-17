using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class Pswd
    {
        public int id_Pswd { get; set; }
        public string contraseña { get; set; }
        public DateTime fecha_Creacion { get; set; }
        public int usuario { get; set; }

        // Constructor
        public Pswd(int id_Pswd, string contraseña, DateTime fecha_Creacion, int usuario)
        {
            this.id_Pswd = id_Pswd;
            this.contraseña = contraseña;
            this.fecha_Creacion = fecha_Creacion;
            this.usuario = usuario;
        }
    }
}
