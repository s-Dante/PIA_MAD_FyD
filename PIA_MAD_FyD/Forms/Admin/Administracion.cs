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
using PIA_MAD_FyD.UserControls.Shared;

namespace PIA_MAD_FyD.Forms.Admin
{
    public partial class Administracion: Form
    {
        TopPanel topPanel = new TopPanel();
        UC_NavBarAdmin uc_NavBarAdmin = new UC_NavBarAdmin();
        public Usuario UsuarioLogeado { get; set; }

        public Administracion(Usuario usaurio)
        {
            InitializeComponent();
            UsuarioLogeado = usaurio;
        }
        private void Administracion_Load(object sender, EventArgs e)
        {
            string rutaProyecto = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\"));
            string rutaLogo = Path.Combine(rutaProyecto, "Assets", "Imgs", "DaferCorpLogo.png");

            topPanel.Dock = DockStyle.Top;
            topPanel.Logo = Image.FromFile(rutaLogo);
            topPanel.CargarDatosUsuario(UsuarioLogeado);
            panel1.Controls.Add(topPanel);


            //Barra lateral
            panel2.Controls.Add(uc_NavBarAdmin);
            uc_NavBarAdmin.OnMenuSelected += Uc_NavBarAdmin_OnMenuSelected;

        }

        private void Uc_NavBarAdmin_OnMenuSelected(object sender, string opcion)
        {
            //MessageBox.Show("Opción seleccionada: " + opcion); // <- Verifica esto

            panel3.Controls.Clear();

            UserControl controlAMostrar = null;

            switch (opcion)
            {
                case "Perfil":
                    controlAMostrar = new uc_PerfilUsuario(UsuarioLogeado);
                    break;
                case "RegistrarUsuario":
                    controlAMostrar = new uc_RegistrarUsuario(UsuarioLogeado);
                    break;
                case "ModificarUsuario":
                    controlAMostrar = new uc_ModificarUsuario(UsuarioLogeado);
                    break;
                case "RegistrarHotel":
                    controlAMostrar = new uc_RegistrarHotel(UsuarioLogeado);
                    break;
                case "ModificarHotel":
                    controlAMostrar = new uc_ModificarHotel(UsuarioLogeado);
                    break;
                case "RegistrarHabitacion":
                    controlAMostrar = new uc_RegistrarHabitacion(UsuarioLogeado);
                    break;
                case "ModificarHabitacion":
                    controlAMostrar = new uc_ModificarHabitacion(UsuarioLogeado);
                    break;
                case "ReporteOcupacion":
                    controlAMostrar = new uc_ReporteOcupacion();
                    break;
                case "ReporteVentas":
                    controlAMostrar = new uc_ReporteVentas();
                    break;
                case "HistorialCliente":
                    controlAMostrar = new uc_HistorialCliente();
                    break;
                case "Cancelaciones":
                    controlAMostrar = new uc_Cancelaciones();
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
