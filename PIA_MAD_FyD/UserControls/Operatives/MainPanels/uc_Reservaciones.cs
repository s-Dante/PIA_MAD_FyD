﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PIA_MAD_FyD.Data.Entidades;
using PIA_MAD_FyD.Data.DAO_s;
using PIA_MAD_FyD.Services.CountryData;
using System.IO;
using PIA_MAD_FyD.Forms.Operatives;
using System.Data.SqlClient;
using PIA_MAD_FyD.Data;

namespace PIA_MAD_FyD.UserControls.Operatives.MainPanels
{
    public partial class uc_Reservaciones: UserControl
    {
        Cliente clienteReservador;

        Hotel hotelReservar;

        List<Hotel> hoteles;

        private List<Habitacion> habitacionesSeleccionadas = new List<Habitacion>();
        private Hotel hotelSeleccionado = null;

        private List<Country> countries;
        private Country selectedCountry;
        private State selectedState;

        Usuario usuarioLogeado;
        public uc_Reservaciones(Usuario usuarioLogeado)
        {
            InitializeComponent();
            disableControllsRFC();

            InitializeComboBoxes();
            LoadCountryData();

            this.usuarioLogeado = usuarioLogeado;

            treeView1.AfterSelect += treeView1_AfterSelect;
            treeView1.CheckBoxes = true; // Habilitar las casillas de verificación
            treeView1.AfterCheck += treeView1_AfterCheck;

            InitializeListView();
            listView1.FullRowSelect = true; // Selección de fila completa
        }

        private bool isProcessingCheck = false;

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (isProcessingCheck) return;

            isProcessingCheck = true;

            try
            {
                if (e.Node.Tag is Hotel)
                {
                    MessageBox.Show("No puedes seleccionar el hotel, solo habitaciones.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Node.Checked = false;
                    return;
                }

                if (e.Node.Tag is Habitacion habitacion)
                {
                    Hotel hotelPadre = e.Node.Parent?.Tag as Hotel;

                    if (hotelSeleccionado == null)
                    {
                        hotelSeleccionado = hotelPadre;
                    }

                    if (hotelPadre != hotelSeleccionado)
                    {
                        MessageBox.Show("Solo puedes seleccionar habitaciones del mismo hotel.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Node.Checked = false;
                        return;
                    }

                    if (e.Node.Checked)
                    {
                        habitacionesSeleccionadas.Add(habitacion);
                    }
                    else
                    {
                        habitacionesSeleccionadas.Remove(habitacion);

                        if (habitacionesSeleccionadas.Count == 0)
                        {
                            hotelSeleccionado = null;
                        }
                    }

                    MostrarHabitacionesSeleccionadas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isProcessingCheck = false;
            }
        }

        private void MostrarHabitacionesSeleccionadas()
        {
            listView1.Items.Clear();

            foreach (var habitacion in habitacionesSeleccionadas)
            {
                string tipoCama = GetTipoCama(habitacion.tipo_Cama);
                string nivel = GetNivel(habitacion.nivel);
                string vista = GetVista(habitacion.vista);

                ListViewItem item = new ListViewItem(habitacion.id_Habitacion.ToString());
                item.SubItems.Add(tipoCama);
                item.SubItems.Add(habitacion.num_Camas.ToString());
                item.SubItems.Add(habitacion.capacidad.ToString());
                item.SubItems.Add(habitacion.precio.ToString("C"));
                item.SubItems.Add(nivel);
                item.SubItems.Add(vista);

                listView1.Items.Add(item);
            }
        }

        private string GetTipoCama(char tipoCama)
        {
            switch (tipoCama)
            {
                case 'I': return "Individual";
                case 'M': return "Matrimonial";
                case 'Q': return "Queen Size";
                case 'K': return "King Size";
                default: return "Desconocido";
            }
        }

        private string GetNivel(char nivel)
        {
            switch (nivel)
            {
                case 'E': return "Estandar";
                case 'D': return "Doble";
                case 'S': return "Suite";
                case 'P': return "Presidencial";
                default: return "Desconocido";
            }
        }

        private string GetVista(char vista)
        {
            switch (vista)
            {
                case 'M': return "Al Mar";
                case 'A': return "A la Alberca";
                case 'C': return "A la Ciudad";
                case 'J': return "Al Jardin";
                case 'O': return "Otros";
                default: return "Desconocido";
            }
        }

        private void InitializeListView()
        {
            // Configurar las columnas del ListView
            listView1.View = View.Details;
            listView1.Columns.Add("ID Habitacion", 100);
            listView1.Columns.Add("Tipo de Cama", 100);
            listView1.Columns.Add("Numero de Camas", 100);
            listView1.Columns.Add("Capacidad", 100);
            listView1.Columns.Add("Precio", 100);
            listView1.Columns.Add("Nivel", 100);
            listView1.Columns.Add("Vista", 100);
        }

        //ListView
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void InitializeComboBoxes()
        {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

            comboBox2.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;

            comboBox3.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;

            comboBox1.Text = "Seleccionar país";
            comboBox2.Text = "Seleccionar estado";
            comboBox3.Text = "Seleccionar ciudad";

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
        }

        private void LoadCountryData()
        {
            string path = Path.Combine(Application.StartupPath, "Services", "CountryData", "countries+states+cities.json");
            countries = CountryService.LoadCountries(path);

            AutoCompleteStringCollection countryAuto = new AutoCompleteStringCollection();
            foreach (var country in countries)
            {
                comboBox1.Items.Add(country.name);
                countryAuto.Add(country.name);
            }
            comboBox1.AutoCompleteCustomSource = countryAuto;

            numericUpDown2.Minimum = 1;
        }

        private void uc_Reservaciones_Load(object sender, EventArgs e)
        {

        }

        private void enableControllsRFC()
        {
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            button3.Enabled = true;
            treeView1.Enabled = true;
        }

        private void disableControllsRFC()
        {
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            button3.Enabled = false;
            treeView1.Enabled = false;
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            button1.Enabled = false;
        }

        //RFC DE CLIENTE
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        //BUSCAR CLIENTE
        private void button2_Click(object sender, EventArgs e)
        {
            string rfc = textBox3.Text;

            if (string.IsNullOrWhiteSpace(rfc))
            {
                MessageBox.Show("Introduce un RFC válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            clienteReservador = Cliente_DAO.ObtenerClientePorRFC(rfc);
            if (clienteReservador != null)
            {
                MessageBox.Show($"Cliente encontrado\nDatos:\nNombre: {clienteReservador.nombre} {clienteReservador.apellido_Paterno} {clienteReservador.apellido_Materno}\n" +
                                $"Ubicación: {clienteReservador.ubicacion_Cliente.pais}, {clienteReservador.ubicacion_Cliente.estado}, {clienteReservador.ubicacion_Cliente.ciudad}\n" +
                                $"Correo: {clienteReservador.correo}\n" + $"Telefono: {clienteReservador.telefono}",
                                "AVISO", MessageBoxButtons.OK);

                string nombreCompleto = $"{clienteReservador.nombre} {clienteReservador.apellido_Paterno} {clienteReservador.apellido_Materno}";
                label1.Text = nombreCompleto;
                enableControllsRFC();
            }
            else
            {
                MessageBox.Show("Cliente no encontrado");
                disableControllsRFC();
            }
        }


        //PAIS
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.Text = "Seleccionar estado";
            comboBox2.AutoCompleteCustomSource.Clear();

            comboBox3.Items.Clear();
            comboBox3.Text = "Seleccionar ciudad";
            comboBox3.AutoCompleteCustomSource.Clear();

            if (comboBox1.SelectedIndex < 0 || string.IsNullOrEmpty(comboBox1.SelectedItem?.ToString()))
                return;

            selectedCountry = countries.FirstOrDefault(c => c.name == comboBox1.SelectedItem.ToString());

            if (selectedCountry != null)
            {
                AutoCompleteStringCollection stateAuto = new AutoCompleteStringCollection();
                foreach (var state in selectedCountry.states)
                {
                    comboBox2.Items.Add(state.name);
                    stateAuto.Add(state.name);
                }
                comboBox2.AutoCompleteCustomSource = stateAuto;
            }
        }

        //ESTADO
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            comboBox3.Text = "Seleccionar ciudad";
            comboBox3.AutoCompleteCustomSource.Clear();

            if (comboBox2.SelectedIndex < 0 || selectedCountry == null)
                return;

            selectedState = selectedCountry.states.FirstOrDefault(s => s.name == comboBox2.SelectedItem.ToString());

            if (selectedState != null)
            {
                AutoCompleteStringCollection cityAuto = new AutoCompleteStringCollection();
                foreach (var city in selectedState.cities)
                {
                    comboBox3.Items.Add(city.name);
                    cityAuto.Add(city.name);
                }
                comboBox3.AutoCompleteCustomSource = cityAuto;
            }
        }

        //CIUDAD
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Buscrar Hoteles
        private void button3_Click(object sender, EventArgs e)
        {
            string pais = comboBox1.Text;
            string estado = comboBox2.Text;
            string ciudad = comboBox3.Text;

            // Validación básica
            if (string.IsNullOrWhiteSpace(pais) || string.IsNullOrWhiteSpace(estado) || string.IsNullOrWhiteSpace(ciudad))
            {
                MessageBox.Show("Selecciona una ubicación completa (país, estado y ciudad).");
                return;
            }

            // Ya incluye habitaciones dentro del SP
            hoteles = Hotel_DAO.ObtenerHotelesYHabitacionesPorUbicacion(pais, estado, ciudad);

            // Verificar si no hay resultados
            if (hoteles == null || hoteles.Count == 0)
            {
                MessageBox.Show("No se encontraron hoteles en esa ubicación.");
                return;
            }

            MostrarHotelesEnTreeView(hoteles);
        }


        private void MostrarHotelesEnTreeView(List<Hotel> hoteles)
        {
            treeView1.Nodes.Clear();

            foreach (var hotel in hoteles)
            {
                TreeNode nodoHotel = new TreeNode(hotel.nombre) { Tag = hotel };

                foreach (var habitacion in hotel.Habitaciones)
                {
                    TreeNode nodoHab = new TreeNode($"Hab. {habitacion.id_Habitacion} - {habitacion.num_Camas} camas")
                    {
                        Tag = habitacion
                    };
                    nodoHotel.Nodes.Add(nodoHab);
                }

                treeView1.Nodes.Add(nodoHotel);
            }
        }

        //TREVIEW PARA MOSTRAR HOTEL y HABITACIONES
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is Habitacion habitacionSeleccionada)
            {
                // Obtener hotel desde el nodo padre
                if (e.Node.Parent?.Tag is Hotel hotelPadre)
                {
                    // Mostrar datos del hotel
                    label5.Text = hotelPadre.nombre;
                }

                // Mostrar datos de la habitación
                char tipoCama = habitacionSeleccionada.tipo_Cama;
                switch (tipoCama)
                {
                    case 'I':
                        label24.Text = "Individual";
                        break;
                    case 'M':
                        label24
                            .Text = "Matrimonial";
                        break;
                    case 'Q':
                        label24.Text = "Queen Size";
                        break;
                    case 'K':
                        label24.Text = "King Size";
                        break;
                    default:
                        label24.Text = "Desconocido";
                        break;
                }
                label17.Text = habitacionSeleccionada.num_Camas.ToString();
                label18.Text = habitacionSeleccionada.capacidad.ToString();
                label19.Text = habitacionSeleccionada.precio.ToString();
                char nivel = habitacionSeleccionada.nivel;
                switch (nivel)
                {
                    case 'E':
                        label20.Text = "Estandar";
                        break;
                    case 'D':
                        label20.Text = "Doble";
                        break;
                    case 'S':
                        label20.Text = "Suite";
                        break;
                    case 'P':
                        label20.Text = "Precidencial";
                        break;
                    default:
                        label20.Text = "Desconocido";
                        break;
                }

                char vista = habitacionSeleccionada.vista;
                switch (vista)
                {
                    case 'M':
                        label21.Text = "Al Mar";
                        break;
                    case 'A':
                        label21.Text = "A la Alberca";
                        break;
                    case 'C':
                        label21.Text = "A la Ciudad";
                        break;
                    case 'J':
                        label21.Text = "Al Jardin";
                        break;
                    case 'O':
                        label21.Text = "Otros";
                        break;
                    default:
                        label21.Text = "Desconocido";
                        break;
                }
            }
            else if (e.Node.Tag is Hotel hotelSeleccionado)
            {
                // Si seleccionó un nodo de hotel, solo mostramos sus datos
                label5.Text = hotelSeleccionado.nombre;

                // Limpiar controles de habitación
                LimpiarControlesHabitacion();
            }
            enableControllsReservacion();
        }

        private void LimpiarControlesHabitacion()
        {
            label5.Text = "";
            label24.Text = "";
            label17.Text = "";
            label18.Text = "";
            label19.Text = "";
            label20.Text = "";
            label21.Text = "";
        }

        private void enableControllsReservacion()
        {
            dateTimePicker1.Enabled = true;
            dateTimePicker2.Enabled = true;
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            button1.Enabled = true;
        }

        private void disableControllsReservacion()
        {
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            button1.Enabled = false;
        }
        //FECHA DE ENTRADA
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        //FECHA DE SALIDA
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        //CANTIDAD DE HUESPEDES
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        //ANTICIPO
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        //RESERVAR
        private void button1_Click(object sender, EventArgs e)
        {
            // Validación de fechas
            DateTime fechaInicio = dateTimePicker1.Value;
            DateTime fechaFin = dateTimePicker2.Value;

            if (fechaInicio < DateTime.Now)
            {
                MessageBox.Show("La fecha de inicio no puede ser una fecha pasada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (fechaFin < fechaInicio)
            {
                MessageBox.Show("La fecha final no puede ser anterior a la fecha inicial.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validación de cantidad de huéspedes
            int cantidadHuespedes = (int)numericUpDown2.Value;
            int capacidadTotal = habitacionesSeleccionadas.Sum(h => h.capacidad);

            if (cantidadHuespedes > capacidadTotal)
            {
                MessageBox.Show($"La cantidad de huéspedes no puede ser mayor a la capacidad total de las habitaciones seleccionadas ({capacidadTotal} huéspedes).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validación de anticipo
            decimal anticipo = numericUpDown1.Value;
            decimal precioTotal = habitacionesSeleccionadas.Sum(h => h.precio * (decimal)(fechaFin - fechaInicio).TotalDays);

            if (anticipo > precioTotal)
            {
                MessageBox.Show($"El anticipo no puede ser mayor al precio total de la reserva ({precioTotal:C}).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Hacemos que se cancelen las reservaciones sin checkIn
            try
            {
                // Cancelar las reservaciones sin check-in
                Reservacion_DAO.CancelarReservacionesSinCheckIn();

                MessageBox.Show("Reservaciones sin check-in canceladas exitosamente.", "Proceso Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Threading.Thread.Sleep(200);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cancelar reservaciones sin check-in: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            // Llamada al DAO para realizar la reserva
            try
            {
                var reservaInfo = Reservacion_DAO.RealizarReserva(clienteReservador.rfc, hotelSeleccionado.id_Hotel, fechaInicio, fechaFin, cantidadHuespedes, anticipo, habitacionesSeleccionadas, usuarioLogeado.num_Nomina);

                if (reservaInfo != null)
                {
                    string mensaje = $"La reserva se ha realizado con éxito.\n\n" +
                                     $"Código: {reservaInfo["id_Reservacion"]}\n" +
                                     $"Fecha Inicio: {reservaInfo["fecha_Ini"]}\n" +
                                     $"Fecha Fin: {reservaInfo["fecha_Fin"]}\n" +
                                     $"Habitaciones: {reservaInfo["cant_Habitaciones"]}\n" +
                                     $"Tipos: {reservaInfo["tipos_Habitaciones"]}\n" +
                                     $"Huéspedes: {reservaInfo["cant_Huespedes"]}\n" +
                                     $"Anticipo: {reservaInfo["anticipo_Pagado"]}";

                    MessageBox.Show(mensaje, "Reserva Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (this.ParentForm is Operatividad operatividadForm)
                    {
                        operatividadForm.ReiniciarUserControlReservaciones();
                    }
                }
                else
                {
                    MessageBox.Show("No se pudo realizar la reserva. Por favor, verifica los datos e intenta nuevamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public void ReiniciarFormulario()
        {
            // Reiniciar DateTimePickers a sus valores iniciales o predeterminados
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now.AddDays(1);

            // Reiniciar NumericUpDowns
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 1;

            // Limpiar listas o selecciones
            habitacionesSeleccionadas.Clear();

            // Puedes limpiar otros controles si tienes más
            // Por ejemplo, TextBoxes, ComboBoxes, etc.
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        public static DataTable ObtenerHistorialCliente(string rfc, int año)
        {
            string query = @"
        SELECT 
            C.nombre + ' ' + C.apellido_Paterno + ' ' + C.apellido_Materno AS Nombre,
            H.ciudad,
            HT.nombre AS Hotel,
            HA.tipo_Habitacion AS TipoHabitacion,
            HA.id_Habitacion AS NumeroHabitacion,
            R.cant_Huespedes AS NumeroPersonas,
            R.id_Reservacion AS CodigoReservacion,
            R.fecha_Ini AS FechaReservacion,
            R.fecha_CheckIn AS FechaCheckIn,
            R.fecha_CheckOut AS FechaCheckOut,
            R.status_Reservacion AS Estatus,
            R.anticipo_Pagado AS Anticipo,
            R.monto_Hospedaje AS MontoHospedaje,
            R.monto_ServiciosAdicionales AS MontoServicios,
            P.total_Pago AS TotalFactura
        FROM Reservacion R
        INNER JOIN Cliente C ON R.rfc_Cliente = C.rfc
        INNER JOIN Hotel HT ON R.id_Hotel = HT.id_Hotel
        INNER JOIN Habitacion HA ON R.id_Habitacion = HA.id_Habitacion
        INNER JOIN Pago P ON R.id_Reservacion = P.id_Reservacion
        WHERE R.rfc_Cliente = @RFC";

            if (año != 0)
            {
                query += " AND YEAR(R.fecha_Ini) = @Año";
            }

            using (SqlConnection conn = BD_Connection.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RFC", rfc);
                    if (año != 0)
                        cmd.Parameters.AddWithValue("@Año", año);

                    DataTable dataTable = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dataTable);

                    return dataTable;
                }
            }
        }

    }
}
