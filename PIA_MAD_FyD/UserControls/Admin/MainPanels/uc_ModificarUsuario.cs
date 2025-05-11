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
using PIA_MAD_FyD.Helpers.Validations;
using PIA_MAD_FyD.ToolTips_PopUps;

namespace PIA_MAD_FyD.UserControls.Admin.MainPanels
{
    public partial class uc_ModificarUsuario: UserControl
    {
        List<Usuario> listaUsuarios;
        private Usuario usuarioLogeado;
        private Usuario seleccionado;

        private PasswordCheck pswdCheck; // Popup para la contraseña
        private EmailCheck emailCheck; // Tooltip para el correo
        private bool correoBien = false;
        public uc_ModificarUsuario(Usuario usuarioLogeado)
        {
            InitializeComponent();

            this.usuarioLogeado = usuarioLogeado;
            listaUsuarios = Usuario_DAO.ObtenerUsuarios(usuarioLogeado.num_Nomina);
           
            listView1.FullRowSelect = true;

            listView1.Items.Clear();
            foreach (var usuario in listaUsuarios)
            {
                string nombreCompleto = $"{usuario.nombre} {usuario.apellido_Paterno} {usuario.apellido_Materno}";
                ListViewItem item = new ListViewItem(nombreCompleto);
                item.SubItems.Add(usuario.num_Nomina.ToString());
                item.Tag = usuario;
                listView1.Items.Add(item);
            }

            textBox5.KeyPress += textBox5_KeyPress;

            emailCheck = new EmailCheck();
            this.Controls.Add(emailCheck); // Agregar el tooltip al

            //Deshabilitar controles
            deshabilitarControles();
        }

        void deshabilitarControles ()
        {
            textBox1.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            textBox2.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            dateTimePicker1.Enabled = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;

            button1.Visible = false;
        }


        void habilitarControles()
        {
            textBox1.Enabled = true;
            textBox7.Enabled = true;
            textBox8.Enabled = true;
            textBox2.Enabled = true;
            textBox4.Enabled = false;
            textBox5.Enabled = true;
            dateTimePicker1.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            button1.Visible = true;
        }

        private void uc_ModificarUsuario_Load(object sender, EventArgs e)
        {

        }

        //ListView
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem itemSeleccionado = listView1.SelectedItems[0];
                seleccionado = (Usuario)itemSeleccionado.Tag;

                textBox1.Text = seleccionado.nombre;
                textBox7.Text = seleccionado.apellido_Paterno;
                textBox8.Text = seleccionado.apellido_Materno;
                textBox2.Text = seleccionado.correo;
                textBox4.Text = seleccionado.num_Nomina.ToString();
                textBox5.Text = seleccionado.telefono;
                dateTimePicker1.Value = seleccionado.fecha_Nacimiento;

                if (seleccionado.tipo_Usuario == 'A')
                {
                    radioButton1.Checked = true;
                }
                else
                {
                    radioButton2.Checked = true;
                }

                deshabilitarControles();
            }
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

        //Numero de Nomina
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        //Telefono
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string input = textBox5.Text.Replace("-", ""); // Elimina los guiones

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

            textBox5.Text = input;
            textBox5.SelectionStart = textBox5.Text.Length; // Mantener el cursor al final
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos y teclas de control (como backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Cancela la entrada del carácter
            }
        }


        //Fecha de Nacimiento
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        //Tipo de Usuario: Administrador
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        //Tipo de Usuario: Operativo
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        //Boton Modificar
        private void button2_Click(object sender, EventArgs e)
        {
            habilitarControles();
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

                // Asignación de valores desde los campos
                seleccionado.nombre = textBox1.Text;
                seleccionado.apellido_Paterno = textBox7.Text;
                seleccionado.apellido_Materno = textBox8.Text;
                seleccionado.correo = textBox2.Text;
                seleccionado.telefono = textBox5.Text;
                seleccionado.fecha_Nacimiento = dateTimePicker1.Value;
                seleccionado.tipo_Usuario = radioButton1.Checked ? 'A' : 'O';
                seleccionado.estatus = 'A';
                seleccionado.usuario_Modifico = usuarioLogeado.num_Nomina;


                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(seleccionado.nombre) ||
                    string.IsNullOrWhiteSpace(seleccionado.apellido_Paterno) ||
                    string.IsNullOrWhiteSpace(seleccionado.apellido_Materno) ||
                    string.IsNullOrWhiteSpace(seleccionado.telefono))
                {
                    MessageBox.Show("Todos los campos deben estar llenos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Validate_Correo.EsCorreoValido(seleccionado.correo))
                {
                    MessageBox.Show("El correo tiene un formato inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    Usuario_DAO.ActualizarUsuario(seleccionado);
                    MessageBox.Show("Cambios realizados exitosamente :)", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Actualizar el texto mostrado en el ListView
                    itemSeleccionado.Text = $"{seleccionado.nombre} {seleccionado.apellido_Paterno} {seleccionado.apellido_Materno}";
                    itemSeleccionado.SubItems[1].Text = seleccionado.num_Nomina.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

                // Deshabilitar controles
                deshabilitarControles();
            }
            else
            {
                MessageBox.Show("No se realizaron cambios.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
