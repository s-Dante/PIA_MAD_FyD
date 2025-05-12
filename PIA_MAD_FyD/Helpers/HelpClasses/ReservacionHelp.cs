using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Helpers.HelpClasses
{
    class ReservacionHelp
    {
        public Guid idReservacion { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int cantHabitaciones { get; set; }
        public int cantHuespedes { get; set; }
        public decimal anticipoPagado { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string clienteRFC { get; set; }
        public string clienteNombre { get; set; }
        public string hotelNombre { get; set; }
        public string ciudad { get; set; }
        public List<int> habitaciones { get; set; }

        public ReservacionHelp()
        {
            habitaciones = new List<int>();
        }
    }
}
