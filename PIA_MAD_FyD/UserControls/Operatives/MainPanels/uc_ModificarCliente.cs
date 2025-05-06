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
using PIA_MAD_FyD.ToolTips_PopUps;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using PIA_MAD_FyD.Data.DAO_s;
using System.IO;
using System.Globalization;
using PIA_MAD_FyD.Helpers.Validations;

namespace PIA_MAD_FyD.UserControls.Operatives.MainPanels
{
    public partial class uc_ModificarCliente: UserControl
    {
        Usuario usuarioLogeado;
        private List<Country> countries;
        private Country selectedCountry;
        private State selectedState;

        Cliente seleccionado; // Cliente seleccionado para modificar

        private EmailCheck emailCheck; // Tooltip para el correo
        private bool correoBien = false;

        List<Cliente> listaCliente;

        public uc_ModificarCliente(Usuario usuarioLogeado)
        {
            InitializeComponent();
            disableControlls();

            this.usuarioLogeado = usuarioLogeado;

            emailCheck = new EmailCheck();
            this.Controls.Add(emailCheck); // Agregar el tooltip al

            comboBox4.Items.AddRange(new string[] { "Casado", "Soltero", "Divorciado", "Viudo", "Union Libre" }); // C = Casado, S = Soltero, D = Divorciado, V = Viudo, U = Union Libre

            listaCliente = Cliente_DAO.ObtenerClientes();

            listView1.FullRowSelect = true;

            listView1.Items.Clear();
            foreach (var usuario in listaCliente)
            {
                string nombreCompleto = $"{usuario.nombre} {usuario.apellido_Paterno} {usuario.apellido_Materno}";
                ListViewItem item = new ListViewItem(nombreCompleto);
                item.SubItems.Add(usuario.correo.ToString());
                string ubicacionMostrar = $"{usuario.ubicacion_Cliente.pais}, {usuario.ubicacion_Cliente.estado}, {usuario.ubicacion_Cliente.ciudad}";
                item.SubItems.Add(ubicacionMostrar);
                item.Tag = usuario;
                listView1.Items.Add(item);
            }

            InitializeComboBoxes();
            LoadCountryData();

            textBox4.KeyPress += textBox4_KeyPress;
            textBox5.KeyPress += textBox5_KeyPress;
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

        private void uc_ModificarCliente_Load(object sender, EventArgs e)
        {

        }

        private void enableControlls()
        {
            textBox1.Enabled = true; //Nombre
            textBox7.Enabled = true; //Apellido Paterno
            textBox8.Enabled = true; //Apellido Materno
            textBox2.Enabled = true; //Correo
            textBox3.Enabled = false; //RFC
            textBox4.Enabled = true; //Telefono
            dateTimePicker1.Enabled = true; //Fecha de nacimiento
            comboBox4.Enabled = true; //Estado Civil
            comboBox1.Enabled = true; //Pais
            comboBox2.Enabled = true; //Estado
            comboBox3.Enabled = true; //Ciudad
            textBox5.Enabled = true; //Codigo Postal
            button1.Visible = true; //Guardar Cambios
        }

        private void disableControlls()
        {
            textBox1.Enabled = false; //Nombre
            textBox7.Enabled = false; //Apellido Paterno
            textBox8.Enabled = false; //Apellido Materno
            textBox2.Enabled = false; //Correo
            textBox3.Enabled = false; //RFC
            textBox4.Enabled = false; //Telefono
            dateTimePicker1.Enabled = false; //Fecha de nacimiento
            comboBox4.Enabled = false; //Estado Civil
            comboBox1.Enabled = false; //Pais
            comboBox2.Enabled = false; //Estado
            comboBox3.Enabled = false; //Ciudad
            textBox5.Enabled = false; //Codigo Postal
            button1.Visible = false; //Guardar Cambios
        }

        //ListBox Mostrar Clientes registrados
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var item = listView1.SelectedItems[0];
                var cliente = (Cliente)item.Tag;

                textBox1.Text = cliente.nombre;
                textBox7.Text = cliente.apellido_Paterno;
                textBox8.Text = cliente.apellido_Materno;
                textBox2.Text = cliente.correo;
                textBox3.Text = cliente.rfc;
                textBox4.Text = cliente.telefono;
                textBox5.Text = cliente.ubicacion_Cliente.codigo_Postal;
                dateTimePicker1.Value = cliente.fecha_Nacimiento;

                char esatoCivil = cliente.estado_Civil;
                switch (esatoCivil)
                {
                    case 'C':
                        comboBox4.SelectedItem = "Casado";
                        break;
                    case 'S':
                        comboBox4.SelectedItem = "Soltero";
                        break;
                    case 'D':
                        comboBox4.SelectedItem = "Divorciado";
                        break;
                    case 'V':
                        comboBox4.SelectedItem = "Viudo";
                        break;
                    case 'U':
                        comboBox4.SelectedItem = "Union Libre";
                        break;
                    default:
                        MessageBox.Show("Estado Civil no valido");
                        break;
                }


                // === PASO 1: seleccionar país ===
                comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
                comboBox2.SelectedIndexChanged -= comboBox2_SelectedIndexChanged;

                Console.WriteLine($"Buscando país: '{cliente.ubicacion_Cliente.pais}'");
                Console.WriteLine("Opciones en ComboBox:");
                foreach (var itemCombo in comboBox1.Items)
                {
                    Console.WriteLine($"'{itemCombo}'");
                }
                Console.WriteLine($"Estado del Hotel: '{cliente.ubicacion_Cliente.estado}'");
                Console.WriteLine($"Ciudad del Hotel: '{cliente.ubicacion_Cliente.ciudad}'");

                int countryIndex = -1;
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    if (NormalizarTexto(comboBox1.Items[i].ToString()) == NormalizarTexto(cliente.ubicacion_Cliente.pais))
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
                    int stateIndex = comboBox2.FindStringExact(cliente.ubicacion_Cliente.estado);
                    if (stateIndex >= 0)
                    {
                        comboBox2.SelectedIndex = stateIndex;
                        selectedState = selectedCountry.states.FirstOrDefault(s => s.name == cliente.ubicacion_Cliente.estado);

                        comboBox3.Items.Clear();
                        comboBox3.AutoCompleteCustomSource.Clear();
                        foreach (var city in selectedState.cities)
                        {
                            comboBox3.Items.Add(city.name);
                            comboBox3.AutoCompleteCustomSource.Add(city.name);
                        }

                        // === PASO 3: seleccionar ciudad ===
                        comboBox3.Text = cliente.ubicacion_Cliente.ciudad;
                    }
                }
                comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
                comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;

                disableControlls();
            }
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

        //Apellido Paterno
        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        //Apellido Materno
        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        //Correo
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

        //Estado Civil
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

        //Codigo Postal
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            // Quitar y volver a agregar el evento para evitar loops
            textBox5.TextChanged -= textBox5_TextChanged;

            if (textBox5.Text.Length > 10)
            {
                textBox5.Text = textBox5.Text.Substring(0, 10);
                textBox5.SelectionStart = textBox5.Text.Length; // Mantener cursor al final
            }

            textBox5.TextChanged += textBox5_TextChanged;
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos y teclas de control (como backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Cancela la entrada del carácter
            }
        }

        //Modificar
        private void button2_Click(object sender, EventArgs e)
        {
            enableControlls();
        }

        //Guardar Cambios
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult guardar = MessageBox.Show("¿Seguro desea realizar los cambios?", "Alerta", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (guardar == DialogResult.Yes)
            {
                ListViewItem itemSeleccionado = listView1.SelectedItems[0];
                Cliente newCliente = (Cliente)itemSeleccionado.Tag;


                newCliente.rfc = textBox3.Text;
                newCliente.nombre = textBox1.Text;
                newCliente.apellido_Paterno = textBox7.Text;
                newCliente.apellido_Materno = textBox8.Text;
                newCliente.correo = textBox2.Text;
                newCliente.fecha_Nacimiento = dateTimePicker1.Value;
                newCliente.telefono = textBox4.Text;

                string estadoCivilSeleccionado = comboBox4.SelectedItem.ToString();
                switch (estadoCivilSeleccionado)
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
                        MessageBox.Show("Estado Civil no valido");
                        return; // Salir del método si el estado civil no es válido
                }

                newCliente.fecha_Modifico = DateTime.Now;
                newCliente.usuario_Modifico = usuarioLogeado.num_Nomina;

                newCliente.ubicacion_Cliente = new Ubiacacion();
                newCliente.ubicacion_Cliente.pais = comboBox1.SelectedItem.ToString();
                newCliente.ubicacion_Cliente.estado = comboBox2.SelectedItem.ToString();
                newCliente.ubicacion_Cliente.ciudad = comboBox3.SelectedItem.ToString();
                newCliente.ubicacion_Cliente.codigo_Postal = textBox5.Text;

                newCliente.ubicacion_Cliente.id_Ubicacion = 0; // Asignar un valor predeterminado, puedes cambiarlo según tu lógica

                // Validar campos
                if (string.IsNullOrWhiteSpace(newCliente.nombre) ||
                       string.IsNullOrWhiteSpace(newCliente.rfc) ||
                       string.IsNullOrWhiteSpace(newCliente.apellido_Materno) ||
                       string.IsNullOrWhiteSpace(newCliente.apellido_Paterno) ||
                       string.IsNullOrWhiteSpace(newCliente.correo) ||
                       string.IsNullOrWhiteSpace(newCliente.ubicacion_Cliente.codigo_Postal) ||
                       string.IsNullOrWhiteSpace(newCliente.telefono))
                {
                    MessageBox.Show("Todos los campos deben estar llenos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        MessageBox.Show("El RFC no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //Validaciones de la ubicacion
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

                //Codigo Postal
                if (string.IsNullOrEmpty(newCliente.ubicacion_Cliente.codigo_Postal))
                {
                    MessageBox.Show("El campo \"Código Postal\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    Cliente_DAO.ModificarClienteConUbicacion(newCliente);
                    MessageBox.Show("Cambios realizados exitosamente :)", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Actualizar el texto mostrado en el ListView
                    string nombreCompleto = $"{newCliente.nombre} {newCliente.apellido_Paterno} {newCliente.apellido_Materno}";
                    itemSeleccionado.SubItems[0].Text = nombreCompleto;
                    itemSeleccionado.SubItems[1].Text = newCliente.correo;
                    string ubicacionMostrar = $"{newCliente.ubicacion_Cliente.pais}, {newCliente.ubicacion_Cliente.estado}, {newCliente.ubicacion_Cliente.ciudad}";
                    itemSeleccionado.SubItems[2].Text = ubicacionMostrar;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("No se realizaron cambios.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // Deshabilitar controles
            disableControlls();
        }
    }
}
