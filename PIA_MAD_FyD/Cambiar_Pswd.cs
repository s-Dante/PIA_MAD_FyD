using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PIA_MAD_FyD.Data.DAO_s;
using PIA_MAD_FyD.ToolTips_PopUps;

namespace PIA_MAD_FyD
{
    public enum pswdChange
    {
        Exitoso = -1,
        Incorrecto = 0,
    }

    public partial class Cambiar_Pswd: Form
    {
        private PasswordCheck pswdCheck;
        private bool samePswd = false;
        private string clave = "12345678"; // Clave para realizar el cambio de contraseña
        private bool isValid = false;

        public Cambiar_Pswd()
        {
            InitializeComponent();
        }

        private void Cambiar_Pswd_Load(object sender, EventArgs e)
        {

        }

        //Correo
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //Nueva Contraseña
        private async void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (pswdCheck == null || pswdCheck.IsDisposed) // Crear el popup si no existe
            {
                pswdCheck = new PasswordCheck();
                pswdCheck.StartPosition = FormStartPosition.Manual;

                // Posicionar el popup cerca del TextBox
                Point location = textBox2.PointToScreen(new Point(0, textBox2.Height + 5));
                int xOffset = textBox2.Width + 10;
                pswdCheck.Location = new Point(location.X + xOffset, location.Y - 75);

                pswdCheck.Show(this); // Mostrar el popup
            }

            // Validar la contraseña en tiempo real
            string password = textBox2.Text;

            bool length = password.Length >= 8;
            bool uppercase = password.Any(char.IsUpper);
            bool lowercase = password.Any(char.IsLower);
            bool digit = password.Any(char.IsDigit);
            bool specialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

            pswdCheck.UpdateValidation(length, uppercase, lowercase, digit, specialChar);

            // Ocultar el popup si la contraseña ya es válida
            if (length && uppercase && lowercase && digit && specialChar)
            {
                await Task.Delay(800);
                pswdCheck.Close();
                pswdCheck = null; // Liberar referencia
            }
        }

        //Confirmar Contraseña
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if(textBox2.Text == textBox3.Text)
            {
                samePswd = true;
                textBox3.BackColor = Color.LightGreen;
            }
            else
            {
                samePswd = false;
                textBox3.BackColor = Color.LightCoral;
            }
        }

        //Clave para realizar el cambio
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text == clave)
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
        }

        //Boton para cambiar la contraseña
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

            if (!samePswd)
            {
                MessageBox.Show("Las contraseñas no coinciden.");
                return;
            }

            if (!isValid)
            {
                MessageBox.Show("La clave de cambio es incorrecta.");
                return;
            }

            try
            {
                string correo = textBox1.Text.Trim();
                string nuevaContrasena = textBox2.Text.Trim();

                Usuario_DAO.CambiarContraseña(correo, nuevaContrasena);

                MessageBox.Show("Contraseña actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cambiar la contraseña: " + ex.Message);
            }
        }

    }
}
