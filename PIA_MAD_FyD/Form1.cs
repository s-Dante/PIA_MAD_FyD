using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using PIA_MAD_FyD.Data.DAO_s;
using PIA_MAD_FyD.Helpers.FormManager;
using PIA_MAD_FyD.Helpers.Validations;
using PIA_MAD_FyD.ToolTips_PopUps;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace PIA_MAD_FyD
{
    
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Correo
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //Contraseña
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        //Boton de inicio
        private void button1_Click(object sender, EventArgs e)
        {
            string correo = textBox1.Text;
            string contrasena = textBox2.Text;

            try
            {
                inicioSesion resultado = Usuario_DAO.InicioSesion(correo, contrasena);

                switch (resultado)
                {
                    case inicioSesion.ExitosoAdmin:
                        MessageBox.Show("Inicio de sesión exitoso: Administrador");
                        // Redirigir al FORMS de administrador
                        break;
                    case inicioSesion.ExitosoOperativo:
                        MessageBox.Show("Inicio de sesión exitoso: Operativo");
                        // Redirigir al FORMS operativo
                        break;
                    case inicioSesion.FallidoContrasena:
                        MessageBox.Show("Contraseña incorrecta. Intenta nuevamente.");
                        break;
                    case inicioSesion.NoRegistrado:
                        MessageBox.Show("Usuario no registrado.");
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

        //Link de registro
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormManager.ShowForm<Cambiar_Pswd>(this, cerrarAppAlCerrar: false, ocultarActual: false);
            FormManager.ListForms();
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormManager.ShowForm<Registro_NoLogIn>(this, cerrarAppAlCerrar: false, ocultarActual: true);
            FormManager.ListForms();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

}
