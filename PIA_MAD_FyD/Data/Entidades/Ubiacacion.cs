using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class Ubiacacion
    {
        public int id_Ubicacion { get; set; }
        public string pais { get; set; }
        public string estado { get; set; }
        public string ciudad { get; set; }
        public string codigo_Postal { get; set; }
        public DateTime fecha_Registro { get; set; }
        public int usuario_Registrador { get; set; }

        Ubiacacion(int id_Ubicacion, string pais, string estado, string ciudad, string codigo_Postal, DateTime fecha_Registro, int usuario_Registrador)
        {
            this.id_Ubicacion = id_Ubicacion;
            this.pais = pais;
            this.estado = estado;
            this.ciudad = ciudad;
            this.codigo_Postal = codigo_Postal;
            this.fecha_Registro = fecha_Registro;
            this.usuario_Registrador = usuario_Registrador;
        }
    }
}
