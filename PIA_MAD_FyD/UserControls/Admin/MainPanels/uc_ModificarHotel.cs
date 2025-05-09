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
using PIA_MAD_FyD.Helpers.Validations;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Globalization;

namespace PIA_MAD_FyD.UserControls.Admin.MainPanels
{
    public partial class uc_ModificarHotel: UserControl
    {
        private List<Country> countries;
        private Country selectedCountry;
        private State selectedState;

        private Hotel seleccionado;
        private Usuario usuarioLogeado;

        List<Hotel> hoteles = Hotel_DAO.ObtenerHotelesConUbicacion();
        public uc_ModificarHotel(Usuario usuarioLogeado)
        {
            InitializeComponent();

            this.usuarioLogeado = usuarioLogeado;

            listView1.Items.Clear();
            listView1.FullRowSelect = true;

            foreach (var hotel in hoteles)
            {
                var item = new ListViewItem(hotel.nombre);
                item.SubItems.Add(hotel.rfc);
                item.SubItems.Add(hotel.ubicacionHotel.codigo_Postal);
                item.Tag = hotel; // Guarda el objeto completo
                listView1.Items.Add(item);
            }

            InitializeComboBoxes();
            LoadCountryData();

            disableControls();
            textBox6.KeyPress += textBox6_KeyPress;
        }

        private void disableControls()
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            numericUpDown1.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;

            button1.Visible = false;
        }

        private void enableControls()
        {
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = false;
            textBox6.Enabled = true;
            numericUpDown1.Enabled = true;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;

            button1.Visible = true;
        }


        private void InitializeComboBoxes()
        {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

            comboBox2.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;

            comboBox3.DropDownStyle = ComboBoxStyle.DropDown; // Permite texto libre
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

        private void uc_ModificarHotel_Load(object sender, EventArgs e)
        {
            
        }


        //ListView mostrar Hoteles
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var item = listView1.SelectedItems[0];
                var hotel = (Hotel)item.Tag;

                textBox1.Text = hotel.nombre;
                textBox5.Text = hotel.rfc;
                textBox2.Text = hotel.colonia;
                textBox3.Text = hotel.calle;
                textBox4.Text = hotel.numero;
                numericUpDown1.Value = hotel.num_Pisos;
                textBox6.Text = hotel.ubicacionHotel.codigo_Postal;

                // === PASO 1: seleccionar país ===
                comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
                comboBox2.SelectedIndexChanged -= comboBox2_SelectedIndexChanged;

                Console.WriteLine($"Buscando país: '{hotel.ubicacionHotel.pais}'");
                Console.WriteLine("Opciones en ComboBox:");
                foreach (var itemCombo in comboBox1.Items)
                {
                    Console.WriteLine($"'{itemCombo}'");
                }
                Console.WriteLine($"Estado del Hotel: '{hotel.ubicacionHotel.estado}'");
                Console.WriteLine($"Ciudad del Hotel: '{hotel.ubicacionHotel.ciudad}'");

                int countryIndex = -1;
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    if (NormalizarTexto(comboBox1.Items[i].ToString()) == NormalizarTexto(hotel.ubicacionHotel.pais))
                    {
                        countryIndex = i;
                        break;
                    }
                }

                if (countryIndex >= 0)
                {
                    comboBox1.SelectedIndex = countryIndex;
                    selectedCountry = countries.FirstOrDefault(c => c.name == comboBox1.SelectedItem.ToString());


                    comboBox2.Items.Clear();
                    comboBox2.AutoCompleteCustomSource.Clear();
                    foreach (var state in selectedCountry.states)
                    {
                        comboBox2.Items.Add(state.name);
                        comboBox2.AutoCompleteCustomSource.Add(state.name);
                    }

                    // === PASO 2: seleccionar estado ===
                    int stateIndex = comboBox2.FindStringExact(hotel.ubicacionHotel.estado);
                    if (stateIndex >= 0)
                    {
                        comboBox2.SelectedIndex = stateIndex;
                        selectedState = selectedCountry.states.FirstOrDefault(s => s.name == hotel.ubicacionHotel.estado);

                        comboBox3.Items.Clear();
                        comboBox3.AutoCompleteCustomSource.Clear();
                        foreach (var city in selectedState.cities)
                        {
                            comboBox3.Items.Add(city.name);
                            comboBox3.AutoCompleteCustomSource.Add(city.name);
                        }

                        // === PASO 3: seleccionar ciudad ===
                        comboBox3.Text = hotel.ubicacionHotel.ciudad;
                    }
                }
                comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
                comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            }
            disableControls();
        }

        private string NormalizarTexto(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return texto;

            var textoFormD = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char c in textoFormD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC).ToLower();
        }

        //Nombre
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //Numero de pisos
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        //RFC
        private void textBox5_TextChanged(object sender, EventArgs e)
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
            comboBox3.Text = ""; // En blanco para permitir escritura
            comboBox3.AutoCompleteCustomSource.Clear();

            if (comboBox2.SelectedIndex < 0 || selectedCountry == null)
                return;

            selectedState = selectedCountry.states.FirstOrDefault(s => s.name == comboBox2.SelectedItem.ToString());

            if (selectedState != null && selectedState.cities != null && selectedState.cities.Count > 0)
            {
                AutoCompleteStringCollection cityAuto = new AutoCompleteStringCollection();
                foreach (var city in selectedState.cities)
                {
                    comboBox3.Items.Add(city.name);
                    cityAuto.Add(city.name);
                }
                comboBox3.AutoCompleteCustomSource = cityAuto;
            }
            else
            {
                // Si no hay ciudades, deja comboBox3 vacío para que el usuario escriba
                comboBox3.Text = "";
            }
        }


        //Ciudad
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Codigo Postal
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            // Quitar y volver a agregar el evento para evitar loops
            textBox6.TextChanged -= textBox6_TextChanged;

            if (textBox6.Text.Length > 10)
            {
                textBox6.Text = textBox6.Text.Substring(0, 10);
                textBox6.SelectionStart = textBox6.Text.Length; // Mantener cursor al final
            }

            textBox6.TextChanged += textBox6_TextChanged;
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos y teclas de control (como backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Cancela la entrada del carácter
            }
        }

        //Colonia
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        //Calle
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        //Numero
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        //Boton Modificar
        private void button2_Click(object sender, EventArgs e)
        {
            enableControls();
        }

        //Boton Guardar Cambios
        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Por favor, selecciona un usuario.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult guardar = MessageBox.Show("¿Seguro desea realizar los cambios?", "Alerta", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (guardar == DialogResult.Yes)
            {
                // Recupera el usuario seleccionado desde el Tag del item
                ListViewItem itemSeleccionado = listView1.SelectedItems[0];
                Hotel seleccionado = (Hotel)itemSeleccionado.Tag;


                // Asignación de valores desde los campos
                seleccionado.nombre = textBox1.Text;
                seleccionado.rfc = textBox5.Text;
                seleccionado.calle = textBox3.Text;
                seleccionado.numero = textBox4.Text;
                seleccionado.colonia = textBox2.Text;
                seleccionado.num_Pisos = (int)numericUpDown1.Value;
                seleccionado.fecha_Modifico = DateTime.Now;
                seleccionado.usuario_Modifico = usuarioLogeado.num_Nomina;

                if (seleccionado.ubicacionHotel == null)
                    seleccionado.ubicacionHotel = new Ubiacacion();


                seleccionado.ubicacionHotel.codigo_Postal = textBox6.Text;
                seleccionado.ubicacionHotel.pais = comboBox1.Text;
                seleccionado.ubicacionHotel.estado = comboBox2.Text;
                seleccionado.ubicacionHotel.ciudad = comboBox3.Text;



                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(seleccionado.nombre) ||
                    string.IsNullOrWhiteSpace(seleccionado.rfc) ||
                    string.IsNullOrWhiteSpace(seleccionado.calle) ||
                    string.IsNullOrWhiteSpace(seleccionado.numero) ||
                    string.IsNullOrWhiteSpace(seleccionado.colonia))
                {
                    MessageBox.Show("Todos los campos deben estar llenos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(seleccionado.rfc))
                {
                    MessageBox.Show("El campo \"RFC\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if (Validate_RFC.EsRFCValido(seleccionado.rfc) == false)
                    {
                        MessageBox.Show("El RFC no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (seleccionado.num_Pisos < 1)
                {
                    MessageBox.Show("El campo \"Número de Pisos\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Validaciones de la ubicacion
                //Pais
                if (string.IsNullOrEmpty(seleccionado.ubicacionHotel.pais))
                {
                    MessageBox.Show("El campo \"País\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Estado
                if (string.IsNullOrEmpty(seleccionado.ubicacionHotel.estado))
                {
                    MessageBox.Show("El campo \"Estado\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Ciudad
                if (string.IsNullOrEmpty(seleccionado.ubicacionHotel.ciudad))
                {
                    MessageBox.Show("El campo \"Ciudad\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Codigo Postal
                if (string.IsNullOrEmpty(seleccionado.ubicacionHotel.codigo_Postal))
                {
                    MessageBox.Show("El campo \"Código Postal\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                try
                {
                    Hotel_DAO.ModificarHotelConUbicacion(seleccionado);
                    MessageBox.Show("Cambios realizados exitosamente :)", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Actualizar el texto mostrado en el ListView
                    itemSeleccionado.SubItems[0].Text = seleccionado.nombre;
                    itemSeleccionado.SubItems[1].Text = seleccionado.rfc;
                    itemSeleccionado.SubItems[2].Text = seleccionado.ubicacionHotel.codigo_Postal;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

                // Deshabilitar controles
                disableControls();
            }
            else
            {
                MessageBox.Show("No se realizaron cambios.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

    }
}
