using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class ServicioCheckOut
    {
        public int id_CheckOut { get; set; }
        public int id_ServicioExtra { get; set; }

        //Constructor
        public ServicioCheckOut(int id_CheckOut, int id_ServicioExtra)
        {
            this.id_CheckOut = id_CheckOut;
            this.id_ServicioExtra = id_ServicioExtra;
        }
    }
}
