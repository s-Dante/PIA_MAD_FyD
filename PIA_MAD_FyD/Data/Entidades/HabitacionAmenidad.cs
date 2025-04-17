using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class HabitacionAmenidad
    {
        public int id_Amenidad { get; set; }
        public int id_Habitacion { get; set; }

        //Constructor
        public HabitacionAmenidad(int id_Amenidad, int id_Habitacion)
        {
            this.id_Amenidad = id_Amenidad;
            this.id_Habitacion = id_Habitacion;
        }
    }
}
