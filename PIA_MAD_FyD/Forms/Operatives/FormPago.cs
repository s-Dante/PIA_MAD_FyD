using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PIA_MAD_FyD.Data.DAO_s;
using PIA_MAD_FyD.Data.Entidades;
using PIA_MAD_FyD.Helpers;
using PIA_MAD_FyD.Helpers.FormManager;

namespace PIA_MAD_FyD.Forms.Operatives
{
    public partial class FormPago: Form
    {
        private int idCheckOut;
        private decimal montoTotal;

        Usuario usuarioLogeado;


        BorderRadius borderRadius = new BorderRadius();


        public FormPago(int idCheckOut, decimal montoTotal, Usuario usuarioLogeado)
        {
            InitializeComponent();
            this.idCheckOut = idCheckOut;
            this.montoTotal = montoTotal;

            label2.Text = $"{montoTotal}";

            this.usuarioLogeado = usuarioLogeado;

            borderRadius.TargetControl = this;
            borderRadius.CornerRadius = 25;
            borderRadius.CornersToRound = BorderRadius.RoundedCorners.All;

            CargarConceptosPago();
        }

        private void CargarConceptosPago()
        {
            listBox1.Items.Clear();

            decimal costoBase = Pago_DAO.ObtenerCostoBase(idCheckOut);
            List<ServiciosExtra> serviciosExtras = Pago_DAO.ObtenerServiciosExtra(idCheckOut);
            List<string> descuentos = Pago_DAO.ObtenerDescuentos(idCheckOut);
            decimal anticipo = Pago_DAO.ObtenerAnticipo(idCheckOut);

            // Mostrar costo base
            listBox1.Items.Add($"Costo Base: {costoBase:C}");

            // Mostrar servicios extra
            if (serviciosExtras.Count > 0)
            {
                listBox1.Items.Add("Servicios Extra:");
                foreach (var servicio in serviciosExtras)
                {
                    listBox1.Items.Add($"  - {servicio.nombre}: {servicio.precion:C}");
                }
            }

            // Mostrar descuentos
            if (descuentos.Count > 0)
            {
                listBox1.Items.Add("Descuentos Aplicados:");
                foreach (var descuento in descuentos)
                {
                    listBox1.Items.Add($"  - {descuento}");
                }
            }
            else
            {
                listBox1.Items.Add("No se aplicaron descuentos.");
            }

            // Mostrar anticipo
            if (anticipo > 0)
            {
                listBox1.Items.Add($"Anticipo Pagado: -{anticipo:C}");
            }

            // Mostrar total final
            listBox1.Items.Add("-----------------------------");
            listBox1.Items.Add($"Total a Pagar: {montoTotal:C}");
        }

        private void Pago_Load(object sender, EventArgs e)
        {

        }

        //Tarjera
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        //Efectivo
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        //Cheque
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        //Total a pagar
        private void label2_Click(object sender, EventArgs e)
        {

        }

        //Pagar
        private void button2_Click(object sender, EventArgs e)
        {
            char formaPago = ' ';

            if (radioButton1.Checked)
                formaPago = 'T';
            else if (radioButton2.Checked)
                formaPago = 'E';
            else if (radioButton3.Checked)
                formaPago = 'C';
            else
            {
                MessageBox.Show("Selecciona una forma de pago.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int usuarioRegistrador = usuarioLogeado.num_Nomina;

                // Registrar el pago
                int idPago = Pago_DAO.RegistrarPagoReservacion(idCheckOut, usuarioRegistrador, formaPago, montoTotal);

                MessageBox.Show("Pago registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Navegar a la ventana de facturación
                FormManager.ShowForm<Factura>(this, cerrarAppAlCerrar: false, ocultarActual: true, idPago, usuarioLogeado);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el pago: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Cancelar
        private void button1_Click(object sender, EventArgs e)
        {
            FormManager.CloseForm<FormPago>();
        }

        //Informacion de pago
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
