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
using System.IO;
using PIA_MAD_FyD.Data.DAO_s;
using PIA_MAD_FyD.Data.Entidades;
using PIA_MAD_FyD.Helpers.HelpClasses;

namespace PIA_MAD_FyD.UserControls.Admin.MainPanels
{
    public partial class uc_ReporteVentas: UserControl
    {
        private List<Country> countries;
        private Country selectedCountry;
        private State selectedState;

        public uc_ReporteVentas()
        {
            InitializeComponent();

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
        private void uc_ReporteVentas_Load(object sender, EventArgs e)
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

        //Ciudad
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //buscar hotel
        private void button1_Click(object sender, EventArgs e)
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

        //Mostrar hoteles
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Año
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Generar Reporte
        //Generar Reporte
        private void button2_Click(object sender, EventArgs e)
        {
            string pais = comboBox1.Text;
            string estado = comboBox2.Text;
            string ciudad = comboBox3.Text;
            string anio = comboBox4.Text;
            string hotel = listBox1.SelectedItem?.ToString() ?? "Todos";

            // Validación
            if (string.IsNullOrWhiteSpace(pais) || string.IsNullOrWhiteSpace(estado) || string.IsNullOrWhiteSpace(ciudad) || string.IsNullOrWhiteSpace(anio))
            {
                MessageBox.Show("Selecciona una ubicación completa y un año.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int anioSeleccionado = int.Parse(anio);
                var reportes = Hotel_DAO.ObtenerReporteVentas(pais, estado, ciudad, hotel, anioSeleccionado);

                if (reportes.Count == 0)
                {
                    MessageBox.Show("No se encontraron datos para el reporte.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                LlenarListViewReporte(reportes);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LlenarListViewReporte(List<ReporteVenta> reportes)
        {
            listView1.Items.Clear();

            foreach (var reporte in reportes)
            {
                ListViewItem item = new ListViewItem(reporte.NombreHotel);
                item.SubItems.Add(reporte.Ciudad);
                item.SubItems.Add(reporte.Anio.ToString());
                item.SubItems.Add(reporte.Mes.ToString());
                item.SubItems.Add(reporte.IngresosHospedaje.ToString("C"));
                item.SubItems.Add(reporte.ServiciosExtra.ToString("C"));
                item.SubItems.Add(reporte.IngresoTotal.ToString("C"));

                listView1.Items.Add(item);
            }
        }


        //ListView reporte
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
