using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class DescuentoCheckOut
    {
        public int id_CheckOut { get; set; }
        public int id_Descuento { get; set; }

        //Constructor
        public DescuentoCheckOut(int id_CheckOut, int id_Descuento)
        {
            this.id_CheckOut = id_CheckOut;
            this.id_Descuento = id_Descuento;
        }
    }
}
