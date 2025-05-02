using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PIA_MAD_FyD.Helpers;
using PIA_MAD_FyD.Data.Entidades;
using PIA_MAD_FyD.Helpers.Validations;
using PIA_MAD_FyD.ToolTips_PopUps;
using PIA_MAD_FyD.Data.DAO_s;

namespace PIA_MAD_FyD.UserControls.Admin.MainPanels
{
    public partial class uc_PerfilUsuario: UserControl
    {
        BorderRadius borderRadius = new BorderRadius();
        private Usuario usuario = new Usuario();

        private PasswordCheck pswdCheck; // Popup para la contraseña
        private EmailCheck emailCheck; // Tooltip para el correo
        private bool correoBien = false;
        public uc_PerfilUsuario(Usuario usuarioLogeado)
        {
            InitializeComponent();

            usuario = usuarioLogeado;

            borderRadius.TargetControl = this;
            borderRadius.CornerRadius = 25;
            borderRadius.CornersToRound = BorderRadius.RoundedCorners.All;

            textBox1.Enabled = false; //Nombre
            textBox7.Enabled = false; //Apellido Paterno
            textBox8.Enabled = false; //Apellido Materno
            textBox2.Enabled = false; //Correo
            textBox4.Enabled = false; //Numero de Nomina
            textBox5.Enabled = false; //Telefono
            dateTimePicker1.Enabled = false; //Fecha de Nacimiento
            radioButton1.Enabled = false; //Tipo de usuario: Administrador
            radioButton2.Enabled = false; //Tipo de usuario: Operativo

            button1.Visible = false; //Guardar Cambios

            emailCheck = new EmailCheck();
            this.Controls.Add(emailCheck); // Agregar el tooltip al

            CargarDatosUsuario();

            textBox5.KeyPress += textBox5_KeyPress;

             
        }


        //Cargar los datos del usuario:
        private void CargarDatosUsuario()
        {
            textBox1.Text = usuario.nombre;
            textBox7.Text = usuario.apellido_Paterno;
            textBox8.Text = usuario.apellido_Materno;
            textBox2.Text = usuario.correo;
            textBox4.Text = usuario.num_Nomina.ToString();
            textBox5.Text = usuario.telefono;
            dateTimePicker1.Value = usuario.fecha_Nacimiento;

            if (usuario.tipo_Usuario == 'A')
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;
        }
        private void uc_PerfilUsuario_Load(object sender, EventArgs e)
        {

        }

        //Nombre usuario
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

        // Validar que solo se ingresen números en el TextBox
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

        //Tipo de usuario: Administrador
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        //Tipo de usuario: Operativo
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        //Modificar
        private void button2_Click(object sender, EventArgs e)
        {
            // Habilitamos los campos para edición
            textBox1.Enabled = true;
            textBox7.Enabled = true;
            textBox8.Enabled = true;
            textBox2.Enabled = true;
            textBox5.Enabled = true;
            dateTimePicker1.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;

            button1.Visible = true; //Guardar Cambios
        }

        //Guardar Cambios
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult guardar = MessageBox.Show("¿Seguro desea realizar los cambios?", "Alerta", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (guardar == DialogResult.Yes)
            {
                // Guardar los cambios realizados
                usuario.nombre = textBox1.Text;
                usuario.apellido_Paterno = textBox7.Text;
                usuario.apellido_Materno = textBox8.Text;
                usuario.correo = textBox2.Text;
                usuario.telefono = textBox5.Text;
                usuario.fecha_Nacimiento = dateTimePicker1.Value;
                if (radioButton1.Checked)
                    usuario.tipo_Usuario = 'A';
                else
                    usuario.tipo_Usuario = 'O';
                usuario.usuario_Modifico = usuario.num_Nomina;


                //Guardar los cambios en la base de datos
                //Validaciones basicas de FrontEnd
                //Nombre
                if (string.IsNullOrEmpty(usuario.nombre))
                {
                    MessageBox.Show("El campo \"nombrebre\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Apellido Paterno
                if (string.IsNullOrEmpty(usuario.apellido_Paterno))
                {
                    MessageBox.Show("El campo \"apellido paterno\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Apellido Materno
                if (string.IsNullOrEmpty(usuario.apellido_Materno))
                {
                    MessageBox.Show("El campo \"apellido materno\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Correo
                if (string.IsNullOrEmpty(usuario.correo))
                {
                    MessageBox.Show("El correo no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if (Validate_Correo.EsCorreoValido(usuario.correo) == false)
                    {
                        MessageBox.Show("El correo tiene un formato invalido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //Fecha de Nacimiento
                if (string.IsNullOrEmpty(usuario.telefono))
                {
                    MessageBox.Show("El campo \"telefono\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Tipo de Usuario
                if (usuario.tipo_Usuario == '\0')
                {
                    MessageBox.Show("Debe seleccionar un tipo de usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    Usuario_DAO.ActualizarUsuario(usuario);
                    MessageBox.Show("Cambios realizados exitosamente :)", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                // Si el usuario selecciona "No", no se realizan cambios
                MessageBox.Show("No se realizaron cambios.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // Deshabilitar los campos nuevamente
            textBox1.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            textBox2.Enabled = false;
            textBox5.Enabled = false;
            dateTimePicker1.Enabled = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            button1.Visible = false; //Guardar Cambios
        }
    }
}
