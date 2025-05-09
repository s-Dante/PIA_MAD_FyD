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

            InicializarDiseño();
        }

        //Diseño de la Factura
        private void InicializarDiseño()
        {
            // Datos del Receptor
            Label rfcReceptorLabel = new Label() { Text = "RFC Receptor", Location = new Point(10, 20) };
            TextBox rfcReceptorTextBox = new TextBox() { Location = new Point(120, 20), Width = 150 };
            Controls.Add(rfcReceptorLabel);
            Controls.Add(rfcReceptorTextBox);

            Label nombreReceptorLabel = new Label() { Text = "Nombre Receptor", Location = new Point(10, 60) };
            TextBox nombreReceptorTextBox = new TextBox() { Location = new Point(120, 60), Width = 150 };
            Controls.Add(nombreReceptorLabel);
            Controls.Add(nombreReceptorTextBox);

            Label usoCFDILabel = new Label() { Text = "Uso CFDI", Location = new Point(10, 100) };
            ComboBox usoCFDIComboBox = new ComboBox() { Location = new Point(120, 100), Width = 150 };
            usoCFDIComboBox.Items.AddRange(new string[] { "G03 - Gastos Generales", "P01 - Pagos por ventas", "I01 - Ingresos" });
            Controls.Add(usoCFDILabel);
            Controls.Add(usoCFDIComboBox);

            // DataGridView para los detalles de la factura (habitaciones)
            DataGridView facturaDetailsGrid = new DataGridView() { Location = new Point(10, 140), Width = 400, Height = 200 };
            facturaDetailsGrid.Columns.Add("Descripcion", "Descripción");
            facturaDetailsGrid.Columns.Add("Cantidad", "Cantidad");
            facturaDetailsGrid.Columns.Add("ValorUnitario", "Valor Unitario");
            facturaDetailsGrid.Columns.Add("Importe", "Importe");
            Controls.Add(facturaDetailsGrid);

            // Campos para Totales
            Label subtotalLabel = new Label() { Text = "Subtotal", Location = new Point(10, 360) };
            TextBox subtotalTextBox = new TextBox() { Location = new Point(120, 360), Width = 150, ReadOnly = true };
            Controls.Add(subtotalLabel);
            Controls.Add(subtotalTextBox);

            Label descuentoLabel = new Label() { Text = "Descuento", Location = new Point(10, 400) };
            TextBox descuentoTextBox = new TextBox() { Location = new Point(120, 400), Width = 150, Text = "0" };
            descuentoTextBox.TextChanged += (sender, args) => CalcularTotal(facturaDetailsGrid, subtotalTextBox, descuentoTextBox);
            Controls.Add(descuentoLabel);
            Controls.Add(descuentoTextBox);

            Label totalLabel = new Label() { Text = "Total", Location = new Point(10, 440) };
            TextBox totalTextBox = new TextBox() { Location = new Point(120, 440), Width = 150, ReadOnly = true };
            Controls.Add(totalLabel);
            Controls.Add(totalTextBox);

            // Métodos de pago
            Label metodoPagoLabel = new Label() { Text = "Método de Pago", Location = new Point(10, 480) };
            ComboBox metodoPagoComboBox = new ComboBox() { Location = new Point(120, 480), Width = 150 };
            metodoPagoComboBox.Items.AddRange(new string[] { "PUE - Pago Único", "PPD - Pago Parcial" });
            Controls.Add(metodoPagoLabel);
            Controls.Add(metodoPagoComboBox);

            // Botón de Guardar
            Button guardarButton = new Button() { Text = "Guardar y Generar Factura", Location = new Point(10, 520), Width = 150 };
            guardarButton.Click += (sender, args) => GuardarFactura(rfcReceptorTextBox, nombreReceptorTextBox, usoCFDIComboBox, facturaDetailsGrid, subtotalTextBox, descuentoTextBox, totalTextBox, metodoPagoComboBox);
            Controls.Add(guardarButton);

            // Botón de Imprimir
            Button imprimirButton = new Button() { Text = "Imprimir Factura", Location = new Point(170, 520), Width = 150 };
            imprimirButton.Click += (sender, args) => ImprimirFactura();
            Controls.Add(imprimirButton);
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

        private void GuardarFactura(TextBox rfcReceptorTextBox, TextBox nombreReceptorTextBox, ComboBox usoCFDIComboBox, DataGridView facturaDetailsGrid, TextBox subtotalTextBox, TextBox descuentoTextBox, TextBox totalTextBox, ComboBox metodoPagoComboBox)
        {
            // Aquí deberías guardar la factura en la base de datos
            // Insertar los datos en tbl_Factura y tbl_FacturaDetalle
            MessageBox.Show("Factura guardada con éxito.");
        }

        private void ImprimirFactura()
        {
            // Aquí puedes implementar la lógica de impresión
            // Esto podría incluir la generación de un archivo PDF o XML para la factura
            MessageBox.Show("Imprimiendo factura...");
        }

        private void Factura_Load(object sender, EventArgs e)
        {

        }
    }
}
