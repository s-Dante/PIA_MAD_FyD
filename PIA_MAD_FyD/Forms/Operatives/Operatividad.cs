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
using PIA_MAD_FyD.UserControls.Admin;
using PIA_MAD_FyD.UserControls.Admin.MainPanels;
using PIA_MAD_FyD.UserControls.Operatives;
using PIA_MAD_FyD.UserControls.Operatives.MainPanels;
using PIA_MAD_FyD.UserControls.Shared;

namespace PIA_MAD_FyD.Forms.Operatives
{
    public partial class Operatividad: Form
    {
        TopPanel topPanel = new TopPanel();
        UC_NavBarOp uc_NavBarOp = new UC_NavBarOp();
        public Usuario UsuarioLogeado { get; set; }

        public Operatividad(Usuario usuario)
        {
            InitializeComponent();
            UsuarioLogeado = usuario;
        }

        private void Operatividad_Load(object sender, EventArgs e)
        {
            string rutaProyecto = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\"));
            string rutaLogo = Path.Combine(rutaProyecto, "Assets", "Imgs", "DaferCorpLogo.png");

            topPanel.Dock = DockStyle.Top;
            topPanel.Logo = Image.FromFile(rutaLogo);
            topPanel.CargarDatosUsuario(UsuarioLogeado);
            panel1.Controls.Add(topPanel);

            //Barra lateral
            panel2.Controls.Add(uc_NavBarOp);
            uc_NavBarOp.OnMenuSelected += Uc_NavBarAdmin_OnMenuSelected;
        }

        //Evento para seleccionar el menú
        private void Uc_NavBarAdmin_OnMenuSelected(object sender, string opcion)
        {
            //MessageBox.Show("Opción seleccionada: " + opcion); // <- Verifica esto

            panel3.Controls.Clear();

            UserControl controlAMostrar = null;

            switch (opcion)
            {
                case "RegistrarCliente":
                    controlAMostrar = new uc_RegistrarCliente(UsuarioLogeado);
                    break;
                case "ModificarCliente":
                    controlAMostrar = new uc_ModificarCliente(UsuarioLogeado);
                    break;
                case "Reservaciones":
                    controlAMostrar = new uc_Reservaciones(UsuarioLogeado);
                    break;
                case "CheckIn":
                    controlAMostrar = new uc_CheckIn();
                    break;
                case "CheckOut":
                    controlAMostrar = new uc_CheckOut();
                    break;
            }
            if (controlAMostrar != null)
            {
                controlAMostrar.Dock = DockStyle.Fill;
                panel3.Controls.Add(controlAMostrar);
            }

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
