using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PIA_MAD_FyD.Data.DAO_s;
using PIA_MAD_FyD.Data.Entidades;
using PIA_MAD_FyD.Helpers.Validations;
using PIA_MAD_FyD.Services.CountryData;

namespace PIA_MAD_FyD.UserControls.Admin.MainPanels
{
    public enum errorsTypoHotel
    {
        Correcto = -1,
        NumeroRepetido = 1,
        rfcDuplicado,
        ErrorDesconcido
    }

    public partial class uc_RegistrarHotel : UserControl
    {
        private List<Country> countries;
        private Country selectedCountry;
        private State selectedState;

        private Usuario usuarioLogeado;
        public uc_RegistrarHotel(Usuario usuarioLogeado)
        {
            InitializeComponent();
            InitializeComboBoxes();
            LoadCountryData();

            this.usuarioLogeado = usuarioLogeado;

            textBox6.KeyPress += textBox6_KeyPress;
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

        private void uc_RegistrarHotel_Load(object sender, EventArgs e)
        {

        }

        //Nombre
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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

        //Calle
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        //Numero
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Evitar que se desactive el evento al modificar el texto
            textBox3.TextChanged -= textBox3_TextChanged;

            // Eliminar cualquier # que no esté al principio
            string input = textBox3.Text.Replace("#", "");

            // Limitar la longitud (por ejemplo, 5 caracteres después del #)
            if (input.Length > 5)
            {
                input = input.Substring(0, 5);
            }

            // Añadir el # al inicio
            textBox3.Text = "#" + input;

            // Colocar el cursor al final, pero no antes del #
            textBox3.SelectionStart = textBox3.Text.Length;

            // Reasignar el evento
            textBox3.TextChanged += textBox3_TextChanged;
        }



        //Colonia
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        //RFC
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        //Numero de Pisos
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
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

        //Registrar
        private void button2_Click(object sender, EventArgs e)
        {
            Hotel nuevoHotel = new Hotel();
            nuevoHotel.nombre = textBox1.Text;
            nuevoHotel.rfc = textBox5.Text;
            nuevoHotel.num_Pisos = (int)numericUpDown1.Value;
            nuevoHotel.calle = textBox2.Text;
            nuevoHotel.numero = textBox3.Text;
            nuevoHotel.colonia = textBox4.Text;
            nuevoHotel.usuario_Registrador = usuarioLogeado.num_Nomina;
            nuevoHotel.usuario_Modifico = usuarioLogeado.num_Nomina;

            Ubiacacion nuevaUbicacion = new Ubiacacion();
            string paisSeleccionado = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(paisSeleccionado))
            {
                MessageBox.Show("Debes seleccionar o escribir un país.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
                string estadoSeleccionado = comboBox2.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(estadoSeleccionado))
            {
                MessageBox.Show("Debes seleccionar o escribir un estado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string ciudadSeleccionada = comboBox3.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(ciudadSeleccionada))
            {
                MessageBox.Show("Debes seleccionar o escribir una ciudad.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            nuevaUbicacion.pais = paisSeleccionado;
            nuevaUbicacion.estado = estadoSeleccionado;
            nuevaUbicacion.ciudad = ciudadSeleccionada;

            nuevaUbicacion.codigo_Postal = textBox6.Text;


            //Validaciones basicas de FrontEnd
            //Nombre
            if (string.IsNullOrEmpty(nuevoHotel.nombre))
            {
                MessageBox.Show("El campo \"nombre\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //RFC
            if(string.IsNullOrEmpty(nuevoHotel.rfc))
            {
                MessageBox.Show("El campo \"RFC\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } else
            {
                if (Validate_RFC.EsRFCValido(nuevoHotel.rfc) == false)
                {
                    MessageBox.Show("El RFC no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //Numero de pisos
            if (nuevoHotel.num_Pisos < 1)
            {
                MessageBox.Show("El campo \"Número de Pisos\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Colonia
            if (string.IsNullOrEmpty(nuevoHotel.colonia))
            {
                MessageBox.Show("El campo \"Colonia\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Calle
            if (string.IsNullOrEmpty(nuevoHotel.calle))
            {
                MessageBox.Show("El campo \"Calle\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Numero
            if (string.IsNullOrEmpty(nuevoHotel.numero))
            {
                MessageBox.Show("El campo \"Número\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Validaciones de la ubicacion
            //Pais
            if (string.IsNullOrEmpty(nuevaUbicacion.pais))
            {
                MessageBox.Show("El campo \"País\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Estado
            if (string.IsNullOrEmpty(nuevaUbicacion.estado))
            {
                MessageBox.Show("El campo \"Estado\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Ciudad
            if (string.IsNullOrEmpty(nuevaUbicacion.ciudad))
            {
                MessageBox.Show("El campo \"Ciudad\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Codigo Postal
            if (string.IsNullOrEmpty(nuevaUbicacion.codigo_Postal))
            {
                MessageBox.Show("El campo \"Código Postal\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                errorsTypoHotel resultado = Hotel_DAO.InsertarHotel(nuevoHotel, nuevaUbicacion);
                void mensajeError(string mensaje)
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                switch (resultado)
                {
                    case errorsTypoHotel.Correcto:
                        MessageBox.Show($"Hotel registrado correctamente", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case errorsTypoHotel.NumeroRepetido:
                        mensajeError("No puede registrar un hotel en la misma ubicaicon que otro hotel ya registrado");
                        break;
                    case errorsTypoHotel.rfcDuplicado:
                        mensajeError("El RFC ya está registrado.");
                        break;
                    case errorsTypoHotel.ErrorDesconcido:
                        mensajeError("Error Desconocido.");
                        break;
                    default:
                        MessageBox.Show("Error desconocido.");
                        break;
                }


                // Limpiar los campos después de registrar
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                numericUpDown1.Value = 1;
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                comboBox3.SelectedIndex = -1;
                comboBox1.Text = "Seleccionar país";
                comboBox2.Text = "Seleccionar estado";
                comboBox3.Text = "Seleccionar ciudad";
                comboBox2.Items.Clear();
                comboBox3.Items.Clear();
                comboBox2.AutoCompleteCustomSource.Clear();
                comboBox3.AutoCompleteCustomSource.Clear();
                selectedCountry = null;
                selectedState = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
