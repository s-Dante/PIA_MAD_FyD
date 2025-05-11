using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PIA_MAD_FyD.Services.CountryData;
using PIA_MAD_FyD.Data.Entidades;
using PIA_MAD_FyD.Data.DAO_s;
using System.IO;
using PIA_MAD_FyD.Helpers.HelpClasses;

namespace PIA_MAD_FyD.UserControls.Admin.MainPanels
{
    public partial class uc_ReporteOcupacion: UserControl
    {
        private List<Country> countries;
        private Country selectedCountry;
        private State selectedState;

        Usuario usuarioLogeado;
        public uc_ReporteOcupacion(Usuario usuarioLogeado)
        {
            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;

            InitializeComboBoxes();
            LoadCountryData();
            LlenarComboBoxAnios(comboBox4);
        }

        private void LlenarComboBoxAnios(ComboBox comboBox)
        {
            int anioActual = DateTime.Now.Year;

            comboBox.Items.Clear();
            for (int i = anioActual; i >= anioActual - 10; i--)
            {
                comboBox.Items.Add(i.ToString());
            }

            comboBox.SelectedIndex = 0; // Selecciona el año más reciente por defecto
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

        private void uc_ReporteOcupacion_Load(object sender, EventArgs e)
        {

        }

        //Pais
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

        //Estado
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

        //Cludad
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Año
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Hoteles
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Buscar hotel
        private void button2_Click(object sender, EventArgs e)
        {
            string pais = comboBox1.Text;
            string estado = comboBox2.Text;
            string ciudad = comboBox3.Text;

            // Validación básica
            if (string.IsNullOrWhiteSpace(pais) || string.IsNullOrWhiteSpace(estado) || string.IsNullOrWhiteSpace(ciudad))
            {
                MessageBox.Show("Selecciona una ubicación completa (país, estado y ciudad).", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Obtener hoteles filtrados
                var hotelesFiltrados = Hotel_DAO.ObtenerHotelesPorUbicacion(pais, estado, ciudad);

                if (hotelesFiltrados == null || hotelesFiltrados.Count == 0)
                {
                    MessageBox.Show("No se encontraron hoteles en esta ubicación.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listBox1.Items.Clear();
                    return;
                }

                // Llenar el ListBox con los nombres de los hoteles
                LlenarListBoxHoteles(hotelesFiltrados);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los hoteles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LlenarListBoxHoteles(List<Hotel> hoteles)
        {
            listBox1.Items.Clear();

            foreach (var hotel in hoteles)
            {
                listBox1.Items.Add(hotel.nombre);
            }
        }


        //Generar reporte
        private void button1_Click(object sender, EventArgs e)
        {
            string pais = comboBox1.Text;
            string estado = comboBox2.Text;
            string ciudad = comboBox3.Text;
            string hotel = listBox1.SelectedItem?.ToString(); // Obtener hotel seleccionado

            string anio = comboBox4.Text; // Año seleccionado

            // Validación básica
            if (string.IsNullOrWhiteSpace(pais) || string.IsNullOrWhiteSpace(estado) || string.IsNullOrWhiteSpace(ciudad) || string.IsNullOrWhiteSpace(anio))
            {
                MessageBox.Show("Selecciona una ubicación completa (país, estado, ciudad y año).");
                return;
            }

            // Obtener los datos de ocupación
            List<Ocupacion> ocupacionPorHotel = Hotel_DAO.ObtenerOcupacionPorHotel(pais, estado, ciudad, hotel, anio);

            if (ocupacionPorHotel == null || ocupacionPorHotel.Count == 0)
            {
                MessageBox.Show("No se encontraron datos para el reporte.");
                return;
            }

            // Mostrar los datos en el ListView general
            MostrarDatosOcupacionEnListView(ocupacionPorHotel);

            // Generar resumen de ocupación
            var resumen = GenerarResumen(ocupacionPorHotel);

            // Mostrar el resumen en el ListView resumen
            MostrarResumenEnListView(resumen);
        }

        private void MostrarDatosOcupacionEnListView(List<Ocupacion> ocupacionPorHotel)
        {
            listView1.Items.Clear();

            foreach (var item in ocupacionPorHotel)
            {
                ListViewItem lvItem = new ListViewItem(new[]
                    {
                item.NombreHotel,
                item.Ciudad,
                item.Anio.ToString(),
                item.Mes,
                item.CantidadHabitaciones.ToString(),
                item.PorcentajeOcupacion.ToString("0.##") + "%",
                item.CantidadPersonasHospedadas.ToString()
            });

                listView1.Items.Add(lvItem);
            }
        }

        private void MostrarResumenEnListView(List<ResumenOcupacion> resumen)
        {
            listView2.Items.Clear();

            foreach (var item in resumen)
            {
                ListViewItem lvItem = new ListViewItem(new[]
                {
                    item.NombreHotel,
                    item.Ciudad,
                    item.Anio.ToString(),
                    item.Mes,
                    item.PorcentajeOcupacion.ToString("0.##") + "%"
                });

                listView2.Items.Add(lvItem);
            }
        }

        private List<ResumenOcupacion> GenerarResumen(List<Ocupacion> ocupacionPorHotel)
        {
            var resumen = ocupacionPorHotel
                .GroupBy(o => new { o.Ciudad, o.NombreHotel, o.Anio, o.Mes })
                .Select(g => new ResumenOcupacion
                {
                    Ciudad = g.Key.Ciudad,
                    NombreHotel = g.Key.NombreHotel,
                    Anio = g.Key.Anio,
                    Mes = g.Key.Mes,
                    PorcentajeOcupacion = g.Average(o => o.PorcentajeOcupacion) // Promedio de ocupación
                })
                .ToList();

            return resumen;
        }


        //ListView 1
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //ListView resumen
        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
