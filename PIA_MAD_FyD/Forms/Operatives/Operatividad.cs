using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PIA_MAD_FyD.Data.Entidades;
using PIA_MAD_FyD.UserControls.Shared;

namespace PIA_MAD_FyD.Forms.Operatives
{
    public partial class Operatividad: Form
    {
        TopPanel topPanel = new TopPanel();
        public Usuario UsuarioLogeado { get; set; }

        public Operatividad(Usuario usuario)
        {
            InitializeComponent();
            UsuarioLogeado = usuario;
        }

        private void Operatividad_Load(object sender, EventArgs e)
        {
            string rutaProyecto = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\"));
            string rutaLogo = Path.Combine(rutaProyecto, "Assets", "Imgs", "dafer1.png");

            topPanel.Dock = DockStyle.Top;
            topPanel.Logo = Image.FromFile(rutaLogo);
            topPanel.CargarDatosUsuario(UsuarioLogeado);
            panel1.Controls.Add(topPanel);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
