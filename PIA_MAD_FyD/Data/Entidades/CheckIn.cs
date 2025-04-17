using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class CheckIn
    {
        public int id_CheckIn { get; set; }
        public DateTime fechaYhora { get; set; }
        public int usuario_Registro { get; set; }
        public Guid id_Reservacion { get; set; }

        // Constructor
        public CheckIn(int id_CheckIn, DateTime fechaYhora, int usuario_Registro, Guid id_Reservacion)
        {
            this.id_CheckIn = id_CheckIn;
            this.fechaYhora = fechaYhora;
            this.usuario_Registro = usuario_Registro;
            this.id_Reservacion = id_Reservacion;
        }
    }
}
