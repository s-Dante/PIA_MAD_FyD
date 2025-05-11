using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PIA_MAD_FyD.Forms.Operatives
{
    public partial class Factura: Form
    {
        private int idPago;
        public Factura(int idPago)
        {
            InitializeComponent();
            this.idPago = idPago;

            GenerarCFDIFalso();
        }

        private void CalcularTotal(DataGridView facturaDetailsGrid, TextBox subtotalTextBox, TextBox descuentoTextBox)
        {
            decimal subtotal = 0;
            foreach (DataGridViewRow row in facturaDetailsGrid.Rows)
            {
                if (row.Cells["Importe"].Value != null)
                {
                    subtotal += Convert.ToDecimal(row.Cells["Importe"].Value);
                }
            }
            decimal descuento = Convert.ToDecimal(descuentoTextBox.Text);
            subtotalTextBox.Text = subtotal.ToString("F2");
            decimal total = subtotal - descuento;
            TextBox totalTextBox = Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "totalTextBox");
            totalTextBox.Text = total.ToString("F2");
        }

        //Falseo de codigo CFDI:
        private void GenerarCFDIFalso()
        {
            // Generar sellos y cadena
            string selloDigitalCFDI = GenerarSelloDigital();
            string selloSAT = GenerarSelloDigital();
            string cadenaCertificacionSAT = GenerarCadenaCertificacion();

            // Asignar valores a los labels
            SetLabelText(label26, selloDigitalCFDI, 400);
            SetLabelText(label27, selloSAT, 400);
            SetLabelText(label28, cadenaCertificacionSAT, 400);


            string razonSocial = "DAFER CORP S.A de C.V";
            string rfc = "DFC040811FYD";
            string regimenFiscal = "Opcional para grupos sociales";
            string direccion = "Seleccionar"; // Aquí podrías implementar un selector o dejarlo fijo

            // Agregar los elementos al ListView
            listView1.Items.Add(new ListViewItem("Razón Social: " + razonSocial));
            listView1.Items.Add(new ListViewItem("RFC: " + rfc));
            listView1.Items.Add(new ListViewItem("Régimen Fiscal: " + regimenFiscal));
            listView1.Items.Add(new ListViewItem("Dirección: " + direccion));
        }

        private string GenerarSelloDigital()
        {
            // Generar un hash aleatorio (solo para fines de demostración)
            StringBuilder sb = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 64; i++)
            {
                sb.Append(random.Next(0, 16).ToString("X"));
            }
            return sb.ToString();
        }

        private string GenerarCadenaCertificacion()
        {
            // Simulación de una cadena de certificación
            StringBuilder sb = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 32; i++)
            {
                sb.Append(random.Next(0, 16).ToString("X"));
            }
            return sb.ToString();
        }

        private void SetLabelText(Label label, string text, int maxWidth)
        {
            label.MaximumSize = new System.Drawing.Size(maxWidth, 0); // Ancho máximo, altura automática
            label.AutoSize = true; // Ajustar altura según contenido
            label.Text = text;
        }

        private void Factura_Load(object sender, EventArgs e)
        {

        }

        //Muestra la imagen del logo de la empresa
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        //ListView para mostrar datos del emisor AKA Hotel de la reservacion
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Listview para mostrar la informacion de factura
        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //ListView para msotrar la informacion del receptor AKA cliente
        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Label para mostrar el nombre del hotel
        private void label3_Click(object sender, EventArgs e)
        {

        }


        //Label para mostrar la direccion del hotel
        private void label4_Click(object sender, EventArgs e)
        {

        }

        //Label para mostrar la fecha de emision AKA fecha actual al momento de hacerse
        private void label6_Click(object sender, EventArgs e)
        {

        }

        //Combobox para seleccionar el uso de CFDI del cliente
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Combobox para mostrar el regimen fiscal del cliente
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //ListView para mstrar la informaicon de cada habitacion que se reservo
        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Combobox para seleccinar el metodo de pago PUE o PPD
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Label para mostrar la forma de pago seleccionada al momento de pagar
        private void label22_Click(object sender, EventArgs e)
        {

        }

        //Listbox par mostrar los servicios extra seleccionados en la reservacion
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //ListBox para mostrar los descuentos que se aplicaron
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Label para poner el subtotal
        private void label17_Click(object sender, EventArgs e)
        {

        }

        //Label para mostrar el iva
        private void label18_Click(object sender, EventArgs e)
        {

        }

        //Label para poner el total de pago
        private void label19_Click(object sender, EventArgs e)
        {

        }

        //PictureBox para poner el qr
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        //Boton para generar e imprimr la factura
        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
