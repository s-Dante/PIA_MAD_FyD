using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class CheckOut
    {
        public int id_CheckOut { get; set; }
        public DateTime fechaYhora { get; set; }
        public int usuario_Registro { get; set; }
        public Guid id_Reservacion { get; set; }

        //Constructor
        public CheckOut(int id_CheckOut, DateTime fechaYhora, int usuario_Registro, Guid id_Reservacion)
        {
            this.id_CheckOut = id_CheckOut;
            this.fechaYhora = fechaYhora;
            this.usuario_Registro = usuario_Registro;
            this.id_Reservacion = id_Reservacion;
        }
    }
}
