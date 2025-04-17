using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class FacturaDetalle
    {
        public int id_Detalle { get; set; }
        public string descripcion { get; set; }
        public int cantidad { get; set; }
        public float valor_Unitario { get; set; }
        public float importe { get; set; }
        public int id_Factura { get; set; }

        //Constructor
        public FacturaDetalle(int id_Detalle, string descripcion, int cantidad,
                                float valor_Unitario, float importe, int id_Factura)
        {
            this.id_Detalle = id_Detalle;
            this.descripcion = descripcion;
            this.cantidad = cantidad;
            this.valor_Unitario = valor_Unitario;
            this.importe = importe;
            this.id_Factura = id_Factura;
        }
    }
}
