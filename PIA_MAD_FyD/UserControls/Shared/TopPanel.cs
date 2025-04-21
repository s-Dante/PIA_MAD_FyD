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

namespace PIA_MAD_FyD.UserControls.Shared
{
    public partial class TopPanel: UserControl
    {
        public TopPanel()
        {
            InitializeComponent();
            timer1.Tick += timer1_Tick;
            timer1.Interval = 1000; // 1 segundo
            timer1.Start();
        }

        private Usuario usuarioActual;

        public void CargarDatosUsuario(Usuario usuario)
        {
            usuarioActual = usuario;

            label1.Text = "Dafer Corporation.";
            label2.Text = $"{usuario.nombre} {usuario.apellido_Paterno} - {(usuario.tipo_Usuario == 'A' ? "Administrador" : "Operativo")}";

            // Puedes también actualizar la hora y fecha aquí si lo deseas
            label3.Text = DateTime.Now.ToString("HH:mm");
            label4.Text = DateTime.Now.ToString("d/MMM/yyyy");

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }


        public string NombreEmpresa
        {
            get => label1.Text;
            set => label1.Text = value;
        }

        // Propiedad para el nombre del usuario y rol
        public string InfoUsuario
        {
            get => label2.Text;
            set => label2.Text = value;
        }

        // Propiedad para cambiar el logo si lo necesitas
        public Image Logo
        {
            get => pictureBox1.Image;
            set => pictureBox1.Image = value;
        }

        private void TopPanel_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            label4.Text = DateTime.Now.ToString("d/MMM/yyyy");
        }
    }
}
