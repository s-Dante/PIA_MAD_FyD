using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class Pago
    {
        public int id_Pago { get; set; }
        public char forma_Pago { get; set; }
        public float total { get; set; }
        public int usuario_Registra { get; set; }
        public int id_CheckOut { get; set; }

        //Constructor
        public Pago(int id_Pago, char forma_Pago, float total, int usuario_Registra, int id_CheckOut)
        {
            this.id_Pago = id_Pago;
            this.forma_Pago = forma_Pago;
            this.total = total;
            this.usuario_Registra = usuario_Registra;
            this.id_CheckOut = id_CheckOut;
        }
    }
}
