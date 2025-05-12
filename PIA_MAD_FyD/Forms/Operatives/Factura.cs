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
using System.Data.SqlClient;
using PIA_MAD_FyD.Data;
using System.IO;
using PIA_MAD_FyD.Data.Entidades;

namespace PIA_MAD_FyD.Forms.Operatives
{
    public partial class Factura : Form
    {
        private int idPagoF;

        public string folio;
        public string fechaEmision;
        public string fechaCertificacion;

        public string servicio;
        public string precio;


        Usuario usuarioLogeado;
        public Factura(int idPago, Usuario usuarioLogeado)
        {
            InitializeComponent();
            idPagoF = idPago;
            this.usuarioLogeado = usuarioLogeado;

            GenerarCFDIFalso();
            LlenarListView1();
            LlenarListView2();

            LoadClientInfo(listView3, idPagoF);
            LoadReservationDetails(listView4, idPagoF);
            LoadExtraServices(listBox2, idPagoF);
            LoadDiscounts(listBox1, idPagoF);

            LlenarComboBoxes(comboBox1, comboBox2);
            comboBox3.Items.AddRange(new string[] { "PUE - Una sola Exhibicion", "PPD - Multiples Pagos" });

            CargarDatosHotel(label3, label30, label4, label6, idPagoF);
            loadSubtotalANDPay(label22, label17, label19, idPago);
        }

        //Llenar CFDI y Regimen fiscal
        private void LlenarComboBoxes(ComboBox cbUsoCFDI, ComboBox cbRegimenFiscal)
        {
            // Limpia los ComboBox antes de llenarlos
            cbUsoCFDI.Items.Clear();
            cbRegimenFiscal.Items.Clear();

            // Diccionario de usos de CFDI con sus regímenes fiscales permitidos
            Dictionary<string, string[]> usosCFDI = new Dictionary<string, string[]>
            {
                { "G03 - Gastos en general", new string[] { "601", "603", "606", "612", "620", "621", "622", "623", "624", "625", "626" } },
                { "CP01 - Pagos", new string[] { "601", "603", "605", "606", "608", "610", "611", "612", "614", "616", "620", "621", "622", "623", "624", "607", "615", "625", "626" } },
                { "S01 - Sin efectos fiscales", new string[] { "601", "603", "605", "606", "608", "610", "611", "612", "614", "616", "620", "621", "622", "623", "624", "607", "615", "625", "626" } }
            };

            // Diccionario de descripciones de régimen fiscal
            Dictionary<string, string> regimenFiscalDesc = new Dictionary<string, string>
            {
                { "601", "General de Ley Personas Morales" },
                { "603", "Personas Morales con Fines no Lucrativos" },
                { "605", "Sueldos y Salarios e Ingresos Asimilados a Salarios" },
                { "606", "Arrendamiento" },
                { "608", "Demás ingresos" },
                { "610", "Residentes en el Extranjero sin Establecimiento Permanente en México" },
                { "611", "Ingresos por Dividendos (Socios y Accionistas)" },
                { "612", "Personas Físicas con Actividades Empresariales y Profesionales" },
                { "614", "Ingresos por intereses" },
                { "616", "Sin obligaciones fiscales" },
                { "620", "Sociedades Cooperativas de Producción que optan por diferir sus ingresos" },
                { "621", "Incorporación Fiscal" },
                { "622", "Actividades Agrícolas, Ganaderas, Silvícolas y Pesqueras" },
                { "623", "Opcional para Grupos de Sociedades" },
                { "624", "Coordinados" },
                { "625", "Régimen de las Actividades Empresariales con Ingresos a través de Plataformas Tecnológicas" },
                { "626", "Régimen Simplificado de Confianza" }
            };

            // Llenar el ComboBox de Uso CFDI
            foreach (var uso in usosCFDI.Keys)
            {
                cbUsoCFDI.Items.Add(uso);
            }

            // Evento para actualizar el ComboBox de régimen fiscal según el uso CFDI seleccionado
            cbUsoCFDI.SelectedIndexChanged += (s, e) =>
            {
                cbRegimenFiscal.Items.Clear();

                if (cbUsoCFDI.SelectedItem != null)
                {
                    string selectedUso = cbUsoCFDI.SelectedItem.ToString();
                    if (usosCFDI.ContainsKey(selectedUso))
                    {
                        foreach (string regimen in usosCFDI[selectedUso])
                        {
                            string descripcion = regimenFiscalDesc.ContainsKey(regimen) ? regimenFiscalDesc[regimen] : "Sin descripción";
                            cbRegimenFiscal.Items.Add($"{regimen} - {descripcion}");
                        }
                    }
                }
            };

            // Seleccionar el primer elemento por defecto
            if (cbUsoCFDI.Items.Count > 0)
            {
                cbUsoCFDI.SelectedIndex = 0;
            }
        }


        private void LlenarListView1()
        {
            listView1.Items.Clear();

            string[,] datosEmisor = {
                { "Razon social", "DAFER CORP S.A de C.V" },
                { "RFC", "DFC040811FYD" },
                { "Regimen Fiscal", "Opcional para grupos sociales" },
                { "Direccion", "(Seleccionar dirección)" }
            };

            for (int i = 0; i < datosEmisor.GetLength(0); i++)
            {
                ListViewItem item = new ListViewItem(datosEmisor[i, 0]);
                item.SubItems.Add(datosEmisor[i, 1]);
                listView1.Items.Add(item);
            }
        }

        private void LlenarListView2()
        {
            listView2.Items.Clear();

            folio = Guid.NewGuid().ToString(); // Generar GUID para el folio
            fechaEmision = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            fechaCertificacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string[,] datosComprobante = {
                { "Tipo de comprobante", "I - Ingreso" },
                { "Certificado", "Falseado" },
                { "Fecha de emision", fechaEmision },
                { "Certificado del SAT", "Dato falso" },
                { "Folio GUID", folio },
                { "Fecha de certificacion", fechaCertificacion }
            };

            for (int i = 0; i < datosComprobante.GetLength(0); i++)
            {
                ListViewItem item = new ListViewItem(datosComprobante[i, 0]);
                item.SubItems.Add(datosComprobante[i, 1]);
                listView2.Items.Add(item);
            }
        }


        //Llenar informacion del cliente
        public void LoadClientInfo(ListView listView3, int idPago)
        {
            listView3.Items.Clear();

            using (SqlConnection connection = BD_Connection.ObtenerConexion())
            {
                string query = @"SELECT C.nombre + ' ' + C.apellido_Paterno + ' ' + C.apellido_Materno AS NombreCompleto, 
                                        C.rfc, U.pais, U.estado, U.ciudad, U.codigo_Postal, R.fecha_Ini, R.fecha_Fin, R.id_Reservacion
                                FROM tbl_Pago P 
                                INNER JOIN tbl_CheckOut CO ON P.id_CheckOut = CO.id_CheckOut
                                INNER JOIN tbl_Reservacion R ON CO.id_Reservacion = R.id_Reservacion
                                INNER JOIN tbl_Cliente C ON R.id_Cliente = C.rfc
                                INNER JOIN tbl_Ubicacion U ON C.ubicacion = U.id_Ubicacion
                                WHERE P.id_Pago = @idPago";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idPago", idPago);

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string direccion = $"{reader["pais"]}, {reader["estado"]}, {reader["ciudad"]}, CP: {reader["codigo_Postal"]}";
                                string fechaIni = Convert.ToDateTime(reader["fecha_Ini"]).ToString("dd/MM/yyyy");
                                string fechaFin = Convert.ToDateTime(reader["fecha_Fin"]).ToString("dd/MM/yyyy");

                                ListViewItem item = new ListViewItem(reader["NombreCompleto"].ToString());
                                item.SubItems.Add(reader["rfc"].ToString());
                                item.SubItems.Add(direccion);
                                item.SubItems.Add(fechaIni);
                                item.SubItems.Add(fechaFin);
                                item.SubItems.Add(reader["id_Reservacion"].ToString());
                                listView3.Items.Add(item);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al cargar la información del cliente: " + ex.Message);
                    }
                }
            }
        }

        //Llenar informacion de las habitaciones
        public void LoadReservationDetails(ListView listView4, int idPago)
        {
            listView4.Items.Clear();

            using (SqlConnection connection = BD_Connection.ObtenerConexion())
            {
                string query = @"SELECT H.id_Habitacion, H.nivel, H.vista, H.num_Camas, H.tipo_Cama, H.Capacidad, H.precio
                                 FROM tbl_Pago P
                                 INNER JOIN tbl_CheckOut CO ON P.id_CheckOut = CO.id_CheckOut
                                 INNER JOIN tbl_Reservacion R ON CO.id_Reservacion = R.id_Reservacion
                                 INNER JOIN tbl_HabitacionReserva RH ON R.id_Reservacion = RH.id_Reservacion
                                 INNER JOIN tbl_Habitacion H ON RH.id_Habitacion = H.id_Habitacion
                                 WHERE P.id_Pago = @idPago";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idPago", idPago);

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string nivel = ConvertNivel(Convert.ToChar(reader["nivel"]));
                                string vista = ConvertVista(Convert.ToChar(reader["vista"]));
                                string tipoCama = ConvertTipoCama(Convert.ToChar(reader["tipo_Cama"]));

                                ListViewItem item = new ListViewItem(reader["id_Habitacion"].ToString());
                                item.SubItems.Add(nivel);
                                item.SubItems.Add(vista);
                                item.SubItems.Add(reader["num_Camas"].ToString());
                                item.SubItems.Add(tipoCama);
                                item.SubItems.Add(reader["Capacidad"].ToString());
                                item.SubItems.Add(reader["precio"].ToString());
                                listView4.Items.Add(item);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al cargar los detalles de la reservación: " + ex.Message);
                    }
                }
            }
        }

        private string ConvertNivel(char nivel)
        {
            switch (nivel)
            {
                case 'E': return "E - Estandar";
                case 'D': return "D - Doble";
                case 'S': return "S - Suite";
                case 'P': return "P - Presidencial";
                default: return "Nivel Desconocido";
            }
        }

        private string ConvertVista(char vista)
        {
            switch (vista)
            {
                case 'M': return "M - Al Mar";
                case 'A': return "A - A la Piscina";
                case 'C': return "C - A la Ciudad";
                case 'J': return "J - Al Jardin";
                case 'O': return "O - Otros";
                default: return "Vista Desconocida";
            }
        }

        private string ConvertTipoCama(char tipoCama)
        {
            switch (tipoCama)
            {
                case 'I': return "I - Individual";
                case 'M': return "M - Matrimonial";
                case 'Q': return "Q - Queen Size";
                case 'K': return "K - King Size";
                default: return "Tipo Desconocido";
            }
        }


        //Servicios extra y descuentos
        public void LoadExtraServices(ListBox listBox2, int idPago)
        {
            listBox2.Items.Clear();

            using (SqlConnection connection = BD_Connection.ObtenerConexion())
            {
                string query = @"SELECT S.nombre, S.precio
                                 FROM tbl_ServicioCheckOut SC
                                 INNER JOIN tbl_ServicioExtra S ON SC.id_ServicioExtra = S.id_ServicioExtra
                                 INNER JOIN tbl_CheckOut CO ON SC.id_CheckOut = CO.id_CheckOut
                                 WHERE CO.id_CheckOut = (SELECT id_CheckOut FROM tbl_Pago WHERE id_Pago = @idPago)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idPago", idPago);

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                servicio = reader["nombre"].ToString();
                                precio = reader["precio"].ToString();
                                listBox2.Items.Add(servicio + " - $" + precio);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al cargar los servicios extra: " + ex.Message);
                    }
                }
            }
        }

        public void LoadDiscounts(ListBox listBox1, int idPago)
        {
            listBox1.Items.Clear();

            using (SqlConnection connection = BD_Connection.ObtenerConexion())
            {
                string query = @"SELECT D.descripcion, D.porcentaje
                                 FROM tbl_DescuentoCheckOut DC
                                 INNER JOIN tbl_Descuento D ON DC.id_Descuento = D.id_Descuento
                                 INNER JOIN tbl_CheckOut CO ON DC.id_CheckOut = CO.id_CheckOut
                                 WHERE CO.id_CheckOut = (SELECT id_CheckOut FROM tbl_Pago WHERE id_Pago = @idPago)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idPago", idPago);

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string descripcion = reader["descripcion"].ToString();
                                string monto = reader["porcentaje"].ToString();
                                listBox1.Items.Add(descripcion + " - $" + monto);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al cargar los descuentos: " + ex.Message);
                    }
                }
            }
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
            SetLabelText(label26, selloDigitalCFDI, 450);
            SetLabelText(label27, selloSAT, 450);
            SetLabelText(label28, cadenaCertificacionSAT, 450);


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
            for (int i = 0; i < 200; i++)
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
            for (int i = 0; i < 150; i++)
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


        //Datos del hotel:
        public void CargarDatosHotel(Label lblNombreHotel, Label lblRFC, Label lblDireccion, Label lblFechaFacturacion, int idPago)
        {
            using (SqlConnection connection = BD_Connection.ObtenerConexion())
            {
                string query = @"
                                SELECT HT.nombre, HT.rfc, 
                                       U.pais, U.estado, U.ciudad, U.codigo_Postal, 
                                       HT.colonia, HT.calle, HT.numero
                                FROM tbl_Pago P 
                                INNER JOIN tbl_CheckOut CO ON P.id_CheckOut = CO.id_CheckOut
                                INNER JOIN tbl_Reservacion R ON CO.id_Reservacion = R.id_Reservacion
                                INNER JOIN tbl_HabitacionReserva RH ON R.id_Reservacion = RH.id_Reservacion
                                INNER JOIN tbl_Habitacion H ON RH.id_Habitacion = H.id_Habitacion
                                INNER JOIN tbl_Hotel HT ON H.id_Hotel = HT.id_Hotel
                                INNER JOIN tbl_Ubicacion U ON HT.ubicacion = U.id_Ubicacion
                                WHERE P.id_Pago = @idPago";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idPago", idPago);

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Nombre del hotel
                                lblNombreHotel.Text = reader["nombre"].ToString();

                                // RFC del hotel
                                lblRFC.Text = reader["rfc"].ToString();

                                // Dirección del hotel
                                string direccion = $"{reader["pais"]}, {reader["estado"]}, {reader["ciudad"]}, CP: {reader["codigo_Postal"]}, " +
                                                   $"{reader["colonia"]}, {reader["calle"]}, No. {reader["numero"]}";
                                lblDireccion.Text = direccion;

                                // Fecha de facturación
                                DateTime fechaF = DateTime.Now;
                                lblFechaFacturacion.Text = fechaF.ToString("dd/MM/yyyy");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al cargar los datos del hotel: " + ex.Message);
                    }
                }
            }
        }


        public void loadSubtotalANDPay(Label labelFormaPago, Label labelSubtotal, Label labelTotal, int idPago)
        {
            using (SqlConnection conection = BD_Connection.ObtenerConexion())
            {
                string query = @"
                                SELECT P.forma_Pago, P.total
                                FROM tbl_Pago P WHERE id_Pago = @idPago";

                using (SqlCommand command = new SqlCommand(query, conection))
                {
                    command.Parameters.AddWithValue("@idPago", idPago);
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                //Forma de pago -> convertir de char a string
                                char formaPago = Convert.ToChar(reader["forma_Pago"]);

                                // Asignar el texto adecuado según el valor de forma_Pago
                                switch (formaPago)
                                {
                                    case 'T': // Si es 'C' (por ejemplo, para 'Contado')
                                        labelFormaPago.Text = "T - Pago con Tarjeta";
                                        break;
                                    case 'E': // Si es 'T' (por ejemplo, para 'Tarjeta')
                                        labelFormaPago.Text = "E - Pago en Efectivo";
                                        break;
                                    case 'C': // Si es 'P' (por ejemplo, para 'Plazos')
                                        labelFormaPago.Text = "C - Pago con Cheque";
                                        break;
                                    default:
                                        labelFormaPago.Text = "Forma de pago desconocida";
                                        break;
                                }

                                // Subtotal
                                decimal subtotal = (decimal)reader["total"];
                                labelSubtotal.Text = subtotal.ToString();

                                // Total
                                decimal total = subtotal * 1.16m; // Asegúrate de usar 'm' para el literal decimal
                                labelTotal.Text = total.ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al cargar los datos del pago: " + ex.Message);
                    }
                }
            }
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

        //Boton para guardar e imprimr la factura
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Guid folio = Guid.NewGuid(); // ID único
                string usoCFDI = comboBox1.SelectedItem?.ToString().Split('-')[0].Trim(); // e.g. "G03"
                string regimenFiscal = comboBox2.SelectedItem?.ToString(); // ya trae el código y descripción
                string metodoPago = comboBox3.SelectedItem?.ToString().Split('-')[0].Trim(); // "PUE" o "PPD"
                string formaPago = label22.Text[0].ToString(); // Primer carácter del texto, ej. 'T'

                // Datos de la factura
                decimal subtotal = decimal.Parse(label17.Text);
                decimal total = decimal.Parse(label19.Text);
                decimal iva = subtotal * 0.16m;

                string selloSAT = label27.Text;
                string selloCFDI = label26.Text;
                string cadenaSAT = label28.Text;

                string rfc_Emisor = label30.Text;
                string direccionEmisor = label4.Text;
                string nombreHotel = label3.Text;

                string rfc_Receptor = listView3.Items[0].SubItems[1].Text;
                string nombre_Receptor = listView3.Items[0].SubItems[0].Text;

                int id_CheckOut;
                using (SqlConnection connection = BD_Connection.ObtenerConexion())
                {
                    SqlCommand cmdGetCheckOut = new SqlCommand("SELECT id_CheckOut FROM tbl_Pago WHERE id_Pago = @idPago", connection);
                    cmdGetCheckOut.Parameters.AddWithValue("@idPago", idPagoF);
                    id_CheckOut = (int)cmdGetCheckOut.ExecuteScalar();
                }

                using (SqlConnection conn = BD_Connection.ObtenerConexion())
                {
                    SqlTransaction transaction = conn.BeginTransaction();
                    try
                    {
                        // Insertar en tbl_Factura
                        SqlCommand cmdFactura = new SqlCommand(@"
                    INSERT INTO tbl_Factura 
                    (folio, sello_SAT, lugar_Emision, fecha_Emision, subtotal, descuento, total, metodo_Pago, impuestos_Trasladados, impuestos_Retenidos, forma_Pago, certificado_SAT, fecha_Certificacion, 
                     rfc_Emisor, regimen_Fiscal_Emisor, direccion_Emisor, sello_Emisor, rfc_Receptor, nombre_Receptor, uso_CFDI, usuario_Registra, id_CheckOut)
                    VALUES 
                    (@folio, @sello_SAT, @lugar_Emision, GETDATE(), @subtotal, 0, @total, @metodo_Pago, @iva, 0, @forma_Pago, @certificado_SAT, GETDATE(), 
                     @rfc_Emisor, @regimenFiscal, @direccion_Emisor, @sello_Emisor, @rfc_Receptor, @nombre_Receptor, @uso_CFDI, @usuario, @idCheckOut)", conn, transaction);

                        cmdFactura.Parameters.AddWithValue("@folio", folio);
                        cmdFactura.Parameters.AddWithValue("@sello_SAT", selloSAT);
                        cmdFactura.Parameters.AddWithValue("@lugar_Emision", "Sucursal del hotel");
                        cmdFactura.Parameters.AddWithValue("@subtotal", subtotal);
                        cmdFactura.Parameters.AddWithValue("@total", total);
                        cmdFactura.Parameters.AddWithValue("@metodo_Pago", metodoPago);
                        cmdFactura.Parameters.AddWithValue("@iva", iva);
                        cmdFactura.Parameters.AddWithValue("@forma_Pago", formaPago);
                        cmdFactura.Parameters.AddWithValue("@certificado_SAT", "CERTIFICADOFAKE123456");
                        cmdFactura.Parameters.AddWithValue("@rfc_Emisor", rfc_Emisor);
                        cmdFactura.Parameters.AddWithValue("@regimenFiscal", regimenFiscal);
                        cmdFactura.Parameters.AddWithValue("@direccion_Emisor", direccionEmisor);
                        cmdFactura.Parameters.AddWithValue("@sello_Emisor", selloCFDI);
                        cmdFactura.Parameters.AddWithValue("@rfc_Receptor", rfc_Receptor);
                        cmdFactura.Parameters.AddWithValue("@nombre_Receptor", nombre_Receptor);
                        cmdFactura.Parameters.AddWithValue("@uso_CFDI", usoCFDI);
                        cmdFactura.Parameters.AddWithValue("@usuario", usuarioLogeado.num_Nomina);
                        cmdFactura.Parameters.AddWithValue("@idCheckOut", id_CheckOut);

                        cmdFactura.ExecuteNonQuery();

                        // Insertar en tbl_FacturaDetalle (habitaciones)
                        foreach (ListViewItem item in listView4.Items)
                        {
                            string desc = $"Habitación {item.SubItems[0].Text} - {item.SubItems[3].Text} camas, {item.SubItems[4].Text}";
                            decimal precio = decimal.Parse(item.SubItems[6].Text);

                            SqlCommand cmdDetalle = new SqlCommand(@"
                        INSERT INTO tbl_FacturaDetalle (descripcion, cantidad, valor_Unitario, importe, id_Factura)
                        VALUES (@descripcion, 1, @valorUnitario, @importe, @idFactura)", conn, transaction);

                            cmdDetalle.Parameters.AddWithValue("@descripcion", desc);
                            cmdDetalle.Parameters.AddWithValue("@valorUnitario", precio);
                            cmdDetalle.Parameters.AddWithValue("@importe", precio);
                            cmdDetalle.Parameters.AddWithValue("@idFactura", folio);

                            cmdDetalle.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error al guardar la factura: " + ex.Message);
                        return;
                    }
                }

                // Guardar el PDF
                // Obtener datos del cliente para el PDF
                string fechaEmision = DateTime.Now.ToString("dd/MM/yyyy");
                string fechaInicio = listView3.Items[0].SubItems[3].Text;
                string fechaFin = listView3.Items[0].SubItems[4].Text;
                string direccionCliente = listView3.Items[0].SubItems[2].Text;

                // Guardar el PDF
                GuardarFacturaComoPDF(
                    folio.ToString(),
                    nombre_Receptor,
                    rfc_Receptor,
                    direccionCliente,
                    fechaEmision,
                    fechaInicio,
                    fechaFin,
                    usoCFDI,
                    regimenFiscal,
                    nombreHotel,
                    rfc_Emisor,
                    direccionEmisor,
                    listView4,
                    listBox2,
                    listBox1,
                    metodoPago,
                    formaPago,
                    subtotal,
                    total,
                    selloCFDI,
                    selloSAT,
                    cadenaSAT
                );

                MessageBox.Show("Factura guardada e impresa correctamente.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error general: " + ex.Message);
            }
        }

        public void GuardarFacturaComoPDF(string folio, string nombreCliente, string rfcCliente, string direccionCliente,
                           string fechaEmision, string fechaInicio, string fechaFin, string usoCFDI, string regimenFiscal,
                           string nombreHotel, string rfcHotel, string direccionHotel,
                           ListView listViewHabitaciones, ListBox listBoxServicios, ListBox listBoxDescuentos,
                           string metodoPago, string formaPago, decimal subtotal, decimal total, string selloCFDI,
                           string selloSAT, string cadenaSAT)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "Guardar factura como PDF";
                saveFileDialog.FileName = $"Factura_{folio}.pdf";
                saveFileDialog.Filter = "Archivos PDF (*.pdf)|*.pdf";

                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

                string path = saveFileDialog.FileName;

                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                    PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    var boldTitle = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    var boldSub = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                    var normal = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                    // Cabecera
                    PdfPTable header = new PdfPTable(2);
                    header.WidthPercentage = 100;
                    float[] widths = { 3, 1 };
                    header.SetWidths(widths);

                    PdfPCell cellLeft = new PdfPCell();
                    cellLeft.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cellLeft.AddElement(new Paragraph("DAFER CORP S.A. de C.V.", boldTitle));
                    cellLeft.AddElement(new Paragraph($"RFC: {rfcHotel}", normal));
                    cellLeft.AddElement(new Paragraph($"Dirección: {direccionHotel}", normal));
                    cellLeft.AddElement(new Paragraph($"Fecha Emisión: {fechaEmision}", normal));
                    cellLeft.AddElement(new Paragraph($"Folio: {folio}", normal));
                    header.AddCell(cellLeft);

                    PdfPCell cellRight = new PdfPCell();
                    cellRight.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cellRight.HorizontalAlignment = Element.ALIGN_RIGHT;
                    // Puedes insertar un logo aquí si tienes imagen
                    cellRight.AddElement(new Paragraph("FACTURA", boldTitle));
                    header.AddCell(cellRight);

                    doc.Add(header);
                    doc.Add(new Paragraph("\n"));

                    // Receptor
                    doc.Add(new Paragraph("DATOS DEL CLIENTE", boldSub));
                    doc.Add(new Paragraph($"Nombre: {nombreCliente}", normal));
                    doc.Add(new Paragraph($"RFC: {rfcCliente}", normal));
                    doc.Add(new Paragraph($"Dirección: {direccionCliente}", normal));
                    doc.Add(new Paragraph($"Uso CFDI: {usoCFDI}", normal));
                    doc.Add(new Paragraph($"Régimen Fiscal: {regimenFiscal}", normal));
                    doc.Add(new Paragraph("\n"));

                    // Fechas reservación
                    doc.Add(new Paragraph("Detalles de Reservación", boldSub));
                    doc.Add(new Paragraph($"Check-In: {fechaInicio}", normal));
                    doc.Add(new Paragraph($"Check-Out: {fechaFin}", normal));
                    doc.Add(new Paragraph("\n"));

                    // Tabla de habitaciones
                    PdfPTable tablaHab = new PdfPTable(listViewHabitaciones.Columns.Count);
                    tablaHab.WidthPercentage = 100;
                    foreach (ColumnHeader col in listViewHabitaciones.Columns)
                        tablaHab.AddCell(new PdfPCell(new Phrase(col.Text, boldSub)));
                    foreach (ListViewItem row in listViewHabitaciones.Items)
                        foreach (ListViewItem.ListViewSubItem cell in row.SubItems)
                            tablaHab.AddCell(new PdfPCell(new Phrase(cell.Text, normal)));
                    doc.Add(new Paragraph("Habitaciones:", boldSub));
                    doc.Add(tablaHab);
                    doc.Add(new Paragraph("\n"));

                    // Servicios
                    if (listBoxServicios.Items.Count > 0)
                    {
                        doc.Add(new Paragraph("Servicios Extra:", boldSub));
                        foreach (var item in listBoxServicios.Items)
                            doc.Add(new Paragraph(item.ToString(), normal));
                        doc.Add(new Paragraph("\n"));
                    }

                    // Descuentos
                    if (listBoxDescuentos.Items.Count > 0)
                    {
                        doc.Add(new Paragraph("Descuentos:", boldSub));
                        foreach (var item in listBoxDescuentos.Items)
                            doc.Add(new Paragraph(item.ToString(), normal));
                        doc.Add(new Paragraph("\n"));
                    }

                    // Totales
                    doc.Add(new Paragraph("Totales", boldSub));
                    doc.Add(new Paragraph($"Subtotal: ${subtotal:F2}", normal));
                    doc.Add(new Paragraph($"IVA (16%): ${(subtotal * 0.16m):F2}", normal));
                    doc.Add(new Paragraph($"Total: ${total:F2}", normal));
                    doc.Add(new Paragraph($"Método de Pago: {metodoPago}", normal));
                    doc.Add(new Paragraph($"Forma de Pago: {formaPago}", normal));
                    doc.Add(new Paragraph("\n"));

                    // Sellos
                    doc.Add(new Paragraph("Sello Digital CFDI", boldSub));
                    doc.Add(new Paragraph(selloCFDI, normal));
                    doc.Add(new Paragraph("Sello SAT", boldSub));
                    doc.Add(new Paragraph(selloSAT, normal));
                    doc.Add(new Paragraph("Cadena de Certificación", boldSub));
                    doc.Add(new Paragraph(cadenaSAT, normal));

                    doc.Close();
                }

                MessageBox.Show($"Factura generada exitosamente en:\n{path}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        //RFC del hotel
        private void label30_Click(object sender, EventArgs e)
        {

        }

    }
}
