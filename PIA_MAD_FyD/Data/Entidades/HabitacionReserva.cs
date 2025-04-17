using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class HabitacionReserva
    {
        public Guid id_Reservacion { get; set; }
        public int id_Habitacion { get; set; }

        //Constructor
        public HabitacionReserva(Guid id_Reservacion, int id_Habitacion)
        {
            this.id_Reservacion = id_Reservacion;
            this.id_Habitacion = id_Habitacion;
        }
    }
}
