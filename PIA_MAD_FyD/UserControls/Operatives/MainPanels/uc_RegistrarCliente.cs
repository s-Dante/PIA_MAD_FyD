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
using PIA_MAD_FyD.Services.CountryData;
using System.IO;
using PIA_MAD_FyD.Helpers.Validations;
using PIA_MAD_FyD.ToolTips_PopUps;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using PIA_MAD_FyD.Data.DAO_s;

namespace PIA_MAD_FyD.UserControls.Operatives.MainPanels
{
    public enum errorsTypoCliente
    {
        Correcto = -1,
        Correo = 1,
        rfcDuplicado,
        FechaNacimiento,
        OtroError
    }

    public partial class uc_RegistrarCliente: UserControl
    {

        Usuario usuarioLogeado;
        private List<Country> countries;
        private Country selectedCountry;
        private State selectedState;

        private EmailCheck emailCheck; // Tooltip para el correo
        private bool correoBien = false;
        public uc_RegistrarCliente(Usuario usuarioLogeado)
        {
            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;
            InitializeComboBoxes();
            LoadCountryData();


            emailCheck = new EmailCheck();
            this.Controls.Add(emailCheck); // Agregar el tooltip al

            textBox4.KeyPress += textBox4_KeyPress;

            comboBox4.Items.AddRange(new string[] { "Casado", "Soltero", "Divorciado", "Viudo", "Union Libre" }); // C = Casado, S = Soltero, D = Divorciado, V = Viudo, U = Union Libre
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
        private void uc_RegistrarCliente_Load(object sender, EventArgs e)
        {

        }

        //Nombre de cliente
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //Apellido Paterno
        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        //Apellido Materno
        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        //Correo Electrónico
        private async void textBox2_TextChanged(object sender, EventArgs e)
        {
            string correo = textBox2.Text.Trim();
            correoBien = Validate_Correo.EsCorreoValido(correo);
            if (correoBien)
            {
                emailCheck.SetMessage("✔️ Correo válido");
                emailCheck.BackColor = Color.LightGreen;
                await Task.Delay(800); // Espera antes de que desaparezca
                emailCheck.Visible = false;
            }
            else
            {
                emailCheck.SetMessage("❌ Correo inválido. Revisa el formato.");
                emailCheck.BackColor = Color.LightCoral;
                await Task.Delay(200); // Espera antes de que aparezca
                emailCheck.Visible = true;
                emailCheck.BringToFront();
            }

            // Posicionar el tooltip
            Point location = textBox2.PointToScreen(new Point(textBox2.Width + 10, 0)); // Justo al lado derecho
            location = this.PointToClient(location); // Convertimos a coordenadas del formulario
            emailCheck.Location = new Point(location.X, location.Y);
        }

        //RFC
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        //Telefono
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string input = textBox4.Text.Replace("-", ""); // Elimina los guiones

            if (input.Length > 10) // Limitar a 10 dígitos (sin guiones)
            {
                input = input.Substring(0, 10); // Solo tomar los primeros 10 dígitos
            }

            if (input.Length > 3 && input.Length <= 6)
            {
                input = input.Insert(3, "-");
            }
            else if (input.Length > 6)
            {
                input = input.Insert(3, "-").Insert(7, "-");
            }

            textBox4.Text = input;
            textBox4.SelectionStart = textBox4.Text.Length; // Mantener el cursor al final
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos y teclas de control (como backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Cancela la entrada del carácter
            }
        }


        //Fecha de nacimiento
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        //Estado civil
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
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

        //Registrar
        private void button2_Click(object sender, EventArgs e)
        {
            //Obtenemos los datos de los campos
            Cliente newCliente = new Cliente();
            newCliente.nombre = textBox1.Text;
            newCliente.apellido_Paterno = textBox7.Text;
            newCliente.apellido_Materno = textBox8.Text;
            newCliente.correo = textBox2.Text;
            newCliente.rfc = textBox3.Text;
            newCliente.telefono = textBox4.Text;
            newCliente.fecha_Nacimiento = dateTimePicker1.Value;

            newCliente.ubicacion_Cliente = new Ubiacacion();
            string paisSeleccionado = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(paisSeleccionado))
            {
                MessageBox.Show("Selecciona un país válido.");
                return;
            }
            newCliente.ubicacion_Cliente.pais = paisSeleccionado;

            string estadoSeleccionado = comboBox2.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(estadoSeleccionado))
            {
                MessageBox.Show("Selecciona un estado válido.");
                return;
            }
            newCliente.ubicacion_Cliente.estado = estadoSeleccionado;
            string ciudadSeleccionada = comboBox3.Text;
            if (string.IsNullOrEmpty(ciudadSeleccionada))
            {
                MessageBox.Show("Selecciona una ciudad válida.");
                return;
            }
            newCliente.ubicacion_Cliente.ciudad = ciudadSeleccionada;

            newCliente.ubicacion_Cliente.codigo_Postal = textBox5.Text;
            newCliente.fecha_Registro = DateTime.Now;
            newCliente.usuario_Registrador = usuarioLogeado.num_Nomina;
            newCliente.fecha_Modifico = DateTime.Now;
            newCliente.usuario_Modifico = usuarioLogeado.num_Nomina;


            string estadoCivil = comboBox4.SelectedItem?.ToString();
            switch (estadoCivil)
            {
                case "Casado":
                    newCliente.estado_Civil = 'C';
                    break;
                case "Soltero":
                    newCliente.estado_Civil = 'S';
                    break;
                case "Divorciado":
                    newCliente.estado_Civil = 'D';
                    break;
                case "Viudo":
                    newCliente.estado_Civil = 'V';
                    break;
                case "Union Libre":
                    newCliente.estado_Civil = 'U';
                    break;
                default:
                    MessageBox.Show("Selecciona un estado civil válido.");
                    return;
            }

            //Validaciones basicas de FrontEnd
            //Nombre
            if (string.IsNullOrEmpty(newCliente.nombre))
            {
                MessageBox.Show("El campo \"nombre\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Apellido Paterno
            if (string.IsNullOrEmpty(newCliente.apellido_Paterno))
            {
                MessageBox.Show("El campo \"apellido paterno\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Apellido Materno
            if (string.IsNullOrEmpty(newCliente.apellido_Materno))
            {
                MessageBox.Show("El campo \"apellido materno\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Correo
            if (string.IsNullOrEmpty(newCliente.correo))
            {
                MessageBox.Show("El correo no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (Validate_Correo.EsCorreoValido(newCliente.correo) == false)
                {
                    MessageBox.Show("El correo tiene un formato invalido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //Telefono
            if (string.IsNullOrEmpty(newCliente.telefono))
            {
                MessageBox.Show("El campo \"telefono\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Pais
            if (string.IsNullOrEmpty(newCliente.ubicacion_Cliente.pais))
            {
                MessageBox.Show("El campo \"País\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Estado
            if (string.IsNullOrEmpty(newCliente.ubicacion_Cliente.estado))
            {
                MessageBox.Show("El campo \"Estado\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Ciudad
            if (string.IsNullOrEmpty(newCliente.ubicacion_Cliente.ciudad))
            {
                MessageBox.Show("El campo \"Ciudad\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //RFC
            if (string.IsNullOrEmpty(newCliente.rfc))
            {
                MessageBox.Show("El campo \"RFC\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (Validate_RFC.EsRFCValido(newCliente.rfc) == false)
                {
                    MessageBox.Show("El RFC tiene un formato invalido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //Fecha de nacimiento
            if (newCliente.fecha_Nacimiento >= DateTime.Now)
            {
                MessageBox.Show("La fecha de nacimiento no puede ser mayor o igual a la fecha actual.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Estado Civil
            if (string.IsNullOrEmpty(newCliente.estado_Civil.ToString()))
            {
                MessageBox.Show("El campo \"Estado Civil\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Si todas las validaciones son correctas, se registra el cliente
            try
            {
                errorsTypoCliente resultado = Cliente_DAO.InsertarCliente(newCliente);
                void mensajeError(string mensaje)
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                switch (resultado)
                {
                    case errorsTypoCliente.Correcto:
                        MessageBox.Show($"Usuario registrado correctamente", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Usuario_DAO.MostrarUsuariosConContraseñas();
                        break;
                    case errorsTypoCliente.Correo:
                        mensajeError("El correo ya está registrado.");
                        break;
                    case errorsTypoCliente.rfcDuplicado:
                        mensajeError("El rfc ya está registrado.");
                        break;
                    case errorsTypoCliente.FechaNacimiento:
                        mensajeError("No se puede registrar un usuario menor de edad");
                        break;
                    default:
                        MessageBox.Show("Error desconocido.");
                        break;
                }

                limpiarControles();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        //Limpiar controles
        private void limpiarControles()
        {
            textBox3.Clear();
            textBox1.Clear();
            textBox2.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox7.Clear();
            textBox8.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
            emailCheck.Visible = false; // Ocultar el tooltip
            comboBox1.Text = "Seleccionar país";
            comboBox2.Text = "Seleccionar estado";
            comboBox3.Text = "Seleccionar ciudad";
            comboBox1.AutoCompleteCustomSource.Clear();
            comboBox2.AutoCompleteCustomSource.Clear();
            comboBox3.AutoCompleteCustomSource.Clear();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            selectedCountry = null;
            selectedState = null;
        }


        //Codigo Postal
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            // Evitar que se desactive el evento al modificar el texto
            textBox5.TextChanged -= textBox5_TextChanged;

            // Eliminar cualquier # que no esté al principio
            string input = textBox5.Text.Replace("#", "");

            // Limitar la longitud (por ejemplo, 5 caracteres después del #)
            if (input.Length > 5)
            {
                input = input.Substring(0, 5);
            }

            // Añadir el # al inicio
            textBox5.Text = "#" + input;

            // Colocar el cursor al final, pero no antes del #
            textBox5.SelectionStart = textBox5.Text.Length;

            // Reasignar el evento
            textBox5.TextChanged += textBox5_TextChanged;
        }
    }
}
