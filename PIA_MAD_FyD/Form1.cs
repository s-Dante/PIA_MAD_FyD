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
using PIA_MAD_FyD.Data.Entidades;
using PIA_MAD_FyD.Forms.Admin;
using PIA_MAD_FyD.Forms.Operatives;
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

            if(textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }else
            {
                try
                {
                    inicioSesion resultado = Usuario_DAO.InicioSesion(correo, contrasena);
                    Usuario usuario = Usuario_DAO.ObtenerUsuarioLogeado(correo);
                    switch (resultado)
                    {
                        case inicioSesion.ExitosoAdmin:
                            // Redirigir al FORMS de administrador
                            FormManager.ShowForm<Administracion>(this, cerrarAppAlCerrar: true, ocultarActual: true, usuario);
                            FormManager.ListForms();
                            break;
                        case inicioSesion.ExitosoOperativo:
                            // Redirigir al FORMS 
                            FormManager.ShowForm<Operatividad>(this, cerrarAppAlCerrar: true, ocultarActual: true, usuario);
                            FormManager.ListForms();
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
