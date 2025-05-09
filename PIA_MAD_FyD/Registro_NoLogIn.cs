using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PIA_MAD_FyD.Data.DAO_s;
using PIA_MAD_FyD.Data.Entidades;
using PIA_MAD_FyD.Helpers.FormManager;
using PIA_MAD_FyD.Helpers.Validations;
using PIA_MAD_FyD.ToolTips_PopUps;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PIA_MAD_FyD
{
    public enum errorsTypo
    {
        Correcto = -1,
        Correo = 1,
        NumeroNomina,
        FechaNacimiento,
        ErrorDesconocido
    }


    public partial class Registro_NoLogIn: Form
    {
        private PasswordCheck pswdCheck; // Popup para la contraseña
        private EmailCheck emailCheck; // Tooltip para el correo
        private bool correoBien = false;
        private string claveAcceso = "ADMIN";
        private bool isAdmin = false;

        private bool isResetting = false;

        public Registro_NoLogIn()
        {
            InitializeComponent();
            this.FormClosed += Registro_NoLogIn_FormClosed;
            emailCheck = new EmailCheck();
            this.Controls.Add(emailCheck); // Agregar el tooltip al 

            textBox5.KeyPress += textBox5_KeyPress;
        }

        //Return to the main Form
        private void button1_Click(object sender, EventArgs e)
        {
            // Verifica si Form1 ya existe
            if (!FormManager.ContainsForm<Form1>())
            {
                FormManager.ShowForm<Form1>(cerrarAppAlCerrar: true, ocultarActual: true);  // Si no existe, crea uno nuevo
            }
            else
            {
                FormManager.ShowForm<Form1>(cerrarAppAlCerrar: true, ocultarActual: true);  // Si ya existe, solo lo trae al frente
            }

            // Cierra Form2
            this.Close();
        }


        private void Registro_NoLogIn_Load(object sender, EventArgs e)
        {

        }
        private void Registro_NoLogIn_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormManager.ShowHiddenForm<Form1>(); // Muestra todos los formularios ocultos
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
            if (isResetting) return;

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


        //Contraseña
        private async void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (isResetting) return;

            if (pswdCheck == null || pswdCheck.IsDisposed) // Crear el popup si no existe
            {
                pswdCheck = new PasswordCheck();
                pswdCheck.StartPosition = FormStartPosition.Manual;

                // Posicionar el popup cerca del TextBox
                Point location = textBox3.PointToScreen(new Point(0, textBox3.Height + 5));
                int xOffset = textBox3.Width + 10;
                pswdCheck.Location = new Point(location.X + xOffset, location.Y - 75);

                pswdCheck.Show(this); // Mostrar el popup
            }

            // Validar la contraseña en tiempo real
            string password = textBox3.Text;

            bool length = password.Length >= 8;
            bool uppercase = password.Any(char.IsUpper);
            bool lowercase = password.Any(char.IsLower);
            bool digit = password.Any(char.IsDigit);
            bool specialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

            if (pswdCheck != null && !pswdCheck.IsDisposed)
            {
                pswdCheck.UpdateValidation(length, uppercase, lowercase, digit, specialChar);
            }


            // Ocultar el popup si la contraseña ya es válida
            if (length && uppercase && lowercase && digit && specialChar)
            {
                await Task.Delay(800);
                if (pswdCheck != null && !pswdCheck.IsDisposed)
                {
                    pswdCheck.Close();
                    pswdCheck = null;
                }
            }
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

        //Clave de acceso
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if(textBox6.Text == claveAcceso)
            {
                isAdmin = true;
            }
            else
            {
                isAdmin = false;
            }
        }

        //Boton de registro
        private void button2_Click(object sender, EventArgs e)
        {
            //Aqui la logica de insertar el usuario si se presiono el boton de registro
            Usuario nuevoUsuario = new Usuario();
            nuevoUsuario.nombre = textBox1.Text;
            nuevoUsuario.apellido_Paterno = textBox7.Text;
            nuevoUsuario.apellido_Materno = textBox8.Text;
            nuevoUsuario.correo = textBox2.Text;
            nuevoUsuario.fecha_Nacimiento = dateTimePicker1.Value;
            nuevoUsuario.telefono = textBox5.Text;
            nuevoUsuario.tipo_Usuario = radioButton1.Checked ? 'A' : 'O'; // 'A' para Administrador, 'O' para Operativo
            nuevoUsuario.fecha_Registro = DateTime.Now;
            nuevoUsuario.fecha_Modificaion = DateTime.Now;
            nuevoUsuario.estatus = 'A'; // 'A' para activo, 'B' para inactivo
            string numeroNomina = textBox4.Text;
            nuevoUsuario.usuario_Registrador = 1111;
            nuevoUsuario.usuario_Modifico = 1111;

            string claveAcceso = textBox6.Text;
            string pswd = textBox3.Text;

            //Validaciones basicas de FrontEnd
            //Nombre
            if (string.IsNullOrEmpty(nuevoUsuario.nombre))
            {
                MessageBox.Show("El campo \"nombre\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Apellido Paterno
            if (string.IsNullOrEmpty(nuevoUsuario.apellido_Paterno))
            {
                MessageBox.Show("El campo \"apellido paterno\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Apellido Materno
            if (string.IsNullOrEmpty(nuevoUsuario.apellido_Materno))
            {
                MessageBox.Show("El campo \"apellido materno\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Correo
            if (string.IsNullOrEmpty(nuevoUsuario.correo))
            {
                MessageBox.Show("El correo no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }else
            {
                if (Validate_Correo.EsCorreoValido(nuevoUsuario.correo) == false)
                {
                    MessageBox.Show("El correo tiene un formato invalido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //Contraseña
            if(string.IsNullOrEmpty(pswd))
            {
                MessageBox.Show("El campo \"contraseña\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }else if(Validate_Password.EsPasswordValida(pswd) == false)
            {
                MessageBox.Show("La contraseña no cumple con los requisitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Fecha de Nacimiento
            if (string.IsNullOrEmpty(nuevoUsuario.telefono))
            {
                MessageBox.Show("El campo \"telefono\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Numero de Nomina
            if (string.IsNullOrEmpty(numeroNomina))
            {
                MessageBox.Show("El campo \"numero de nomina\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }else
            {
                nuevoUsuario.num_Nomina = int.Parse(numeroNomina);
            }

            //Clave de acceso
            if (string.IsNullOrEmpty(claveAcceso))
            {
                MessageBox.Show("El campo \"clave de administracion\" no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Tipo de Usuario
            if (nuevoUsuario.tipo_Usuario == '\0')
            {
                MessageBox.Show("Debe seleccionar un tipo de usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Aqui llamamos al DAO para insertar el usuario
            if (isAdmin)
            {
                try
                {
                    errorsTypo resultado = Usuario_DAO.InsertarUsuario(nuevoUsuario, pswd);
                    void mensajeError(string mensaje)
                    {
                        MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    switch (resultado)
                    {
                        case errorsTypo.Correcto:
                            MessageBox.Show($"Usuario registrado correctamente", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Usuario_DAO.MostrarUsuariosConContraseñas();
                            break;
                        case errorsTypo.Correo:
                            mensajeError("El correo ya está registrado.");
                            break;
                        case errorsTypo.NumeroNomina:
                            mensajeError("El número de nómina ya está registrado.");
                            break;
                        case errorsTypo.FechaNacimiento:
                            mensajeError("No se puede registrar un usuario menor de edad");
                            break;
                        default:
                            MessageBox.Show("Error desconocido.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("La clave de acceso es incorrecta. No se puede registrar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            isResetting = true;

            // Limpiar los campos después de registrar
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            dateTimePicker1.Value = DateTime.Now;
            radioButton1.Checked = false;
            radioButton2.Checked = false;

            emailCheck.Visible = false;
            isResetting = false;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        //RadioButton de Administrador
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        //RadioButton de Operativo
        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {

        }

    }
}
