using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class Amenidad
    {
        public int id_Amenidad { get; set; }
        public string amenidad { get; set; }

        public override string ToString()
        {
            return amenidad;
        }

        //Constructor
        public Amenidad() { }
        public Amenidad(int id_Amenidad, string amenidad)
        {
            this.id_Amenidad = id_Amenidad;
            this.amenidad = amenidad;
        }
    }
}
