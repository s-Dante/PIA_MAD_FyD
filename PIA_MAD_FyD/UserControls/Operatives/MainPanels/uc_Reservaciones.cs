using System;
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

namespace PIA_MAD_FyD.UserControls.Operatives.MainPanels
{
    public partial class uc_Reservaciones: UserControl
    {
        Cliente clienteReservador;

        Hotel hotelReservar;

        List<Hotel> hoteles;

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

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }
    }
}
