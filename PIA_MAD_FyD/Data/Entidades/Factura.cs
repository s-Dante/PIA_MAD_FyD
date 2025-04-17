using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Data.Entidades
{
    class Factura
    {
        public Guid folio { get; set; }
        public string sello_SAT { get; set; }
        public string lugar_Emision { get; set; }
        public DateTime fecha_Emision { get; set; }
        public float subtotal { get; set; }
        public float descuento { get; set; }
        public float total { get; set; }
        public string metodo_Pago { get; set; }
        public float impuestos_Trasladados { get; set; }
        public float impuestos_Retenidos { get; set; }
        public string forma_Pago { get; set; }
        public string certificado_SAT { get; set; }
        public string fecha_Certificacion { get; set; }
        public string rfc_Emisor { get; set; }
        public string regimen_Fiscal_Emisor { get; set; }
        public string direccion_Emisor { get; set; }
        public string sello_Emisor { get; set; }
        public string rfc_Receptor { get; set; }
        public string nombre_Receptor { get; set; }
        public string uso_CFDI { get; set; }

        public int usuario_Registra { get; set; }
        public int id_CheckOut { get; set; }

        //Constructor
        public Factura(Guid folio, string sello_SAT, string lugar_Emision, DateTime fecha_Emision,
                        float subtotal, float descuento, float total, string metodo_Pago,
                        float impuestos_Trasladados, float impuestos_Retenidos, string forma_Pago,
                        string certificado_SAT, string fecha_Certificacion, string rfc_Emisor, 
                        string regimen_Fiscal_Emisor, string direccion_Emisor, string sello_Emisor, 
                        string rfc_Receptor, string nombre_Receptor, string uso_CFDI,
                        int usuario_Registra, int id_CheckOut)
        {
            this.folio = folio;
            this.sello_SAT = sello_SAT;
            this.lugar_Emision = lugar_Emision;
            this.fecha_Emision = fecha_Emision;
            this.subtotal = subtotal;
            this.descuento = descuento;
            this.total = total;
            this.metodo_Pago = metodo_Pago;
            this.impuestos_Trasladados = impuestos_Trasladados;
            this.impuestos_Retenidos = impuestos_Retenidos;
            this.forma_Pago = forma_Pago;
            this.certificado_SAT = certificado_SAT;
            this.fecha_Certificacion = fecha_Certificacion;
            this.rfc_Emisor = rfc_Emisor;
            this.regimen_Fiscal_Emisor = regimen_Fiscal_Emisor;
            this.direccion_Emisor = direccion_Emisor;
            this.sello_Emisor = sello_Emisor;
            this.rfc_Receptor = rfc_Receptor;
            this.nombre_Receptor = nombre_Receptor;
            this.uso_CFDI = uso_CFDI;
            this.usuario_Registra = usuario_Registra;
            this.id_CheckOut = id_CheckOut;
        }
    }
}
