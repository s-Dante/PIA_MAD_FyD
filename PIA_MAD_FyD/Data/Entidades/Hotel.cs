using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PIA_MAD_FyD.Data.Entidades
{
    class Hotel
    {
        public int id_Hotel { get; set; }
        public string nombre { get; set; }
        public string calle { get; set; }
        public string numero { get; set; }
        public string colonia { get; set; }
        public int num_Pisos { get; set; }
        public DateTime fecha_InicioOP { get; set; }
        public DateTime fecha_Registro { get; set; }
        public DateTime fecha_Modifico { get; set; }
        public int usuario_Registrador { get; set; }
        public int usuario_Modifico { get; set; }
        public int ubicacion { get; set; }
        public string rfc { get; set; }

        //La ubicacion
        public Ubiacacion ubicacionHotel { get; set; } //Ubicacion del hotel

        // Constructor

        public Hotel()
        {
            // Constructor por defecto
        }
        public Hotel(int id_Hotel, string nombre, string calle, string numero, string colonia, int num_Pisos, 
                    DateTime fecha_InicioOP, DateTime fecha_Registro, DateTime fecha_Modifico, int usuario_Registrador,
                    int usuario_Modifico, int ubicacion)
        {
            this.id_Hotel = id_Hotel;
            this.nombre = nombre;
            this.calle = calle;
            this.numero = numero;
            this.colonia = colonia;
            this.num_Pisos = num_Pisos;
            this.fecha_InicioOP = fecha_InicioOP;
            this.fecha_Registro = fecha_Registro;
            this.fecha_Modifico = fecha_Modifico;
            this.usuario_Registrador = usuario_Registrador;
            this.usuario_Modifico = usuario_Modifico;
            this.ubicacion = ubicacion;
        }

    }
}
