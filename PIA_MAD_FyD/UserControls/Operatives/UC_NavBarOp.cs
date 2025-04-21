using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PIA_MAD_FyD.UserControls.Operatives
{
    public partial class UC_NavBarOp: UserControl
    {
        //Variables para la animación
        Timer slideTimer = new Timer();
        Panel currentSubMenu;
        bool isExpanding = false;
        int targetHeight = 0;
        Queue<Panel> panelsToHide = new Queue<Panel>();

        //Evento para seleccionar el menú
        public event EventHandler<string> OnMenuSelected;
        public UC_NavBarOp()
        {
            InitializeComponent();
            HideAllSubMenus();
            foreach (var panel in this.Controls.OfType<Panel>())
            {
                panel.Height = 80;
            }

            slideTimer.Interval = 10; // Velocidad de la animación (en milisegundos)
            slideTimer.Tick += SlideTimer_Tick;
        }

        private void UC_NavBarOp_Load(object sender, EventArgs e)
        {

        }

        //Método para animar el menú
        private void SlideTimer_Tick(object sender, EventArgs e)
        {
            if (currentSubMenu == null) return;

            if (isExpanding)
            {
                if (currentSubMenu.Height < targetHeight)
                {
                    currentSubMenu.Height += 10;
                }
                else
                {
                    currentSubMenu.Height = targetHeight;
                    slideTimer.Stop();
                }
            }
            else
            {
                currentSubMenu.Height = 80;
                slideTimer.Stop();

                // Si hay más paneles pendientes de cerrar
                if (panelsToHide.Count > 0)
                {
                    currentSubMenu = panelsToHide.Dequeue();
                    slideTimer.Start();
                }
            }

        }

        //Método para ocultar todos los submenús
        void HideAllSubMenus()
        {
            foreach (var panel in this.Controls.OfType<Panel>())
            {
                if (panel.Height > 80)
                {
                    panelsToHide.Enqueue(panel);
                }
            }

            if (panelsToHide.Count > 0)
            {
                currentSubMenu = panelsToHide.Dequeue();
                isExpanding = false;
                slideTimer.Start();
            }
        }

        //Método para mostrar un submenú
        void ShowSubMenu(Panel subMenu)
        {
            currentSubMenu = subMenu;
            targetHeight = 200;
            isExpanding = true;
            slideTimer.Start();
        }

        //Menu Control Clientes
        private void button2_Click(object sender, EventArgs e)
        {
            HideAllSubMenus();
            foreach (var panel in this.Controls.OfType<Panel>())
            {
                panel.Height = 80;
            }
            ShowSubMenu(panel1);
        }

        //Submenu Registrar Cliente
        private void button3_Click(object sender, EventArgs e)
        {
            OnMenuSelected?.Invoke(this, "RegistrarCliente");
        }

        //Submenu Modificar Cliente
        private void button4_Click(object sender, EventArgs e)
        {
            OnMenuSelected?.Invoke(this, "ModificarCliente");
        }

        //Menu Reservaciones
        private void button1_Click(object sender, EventArgs e)
        {
            HideAllSubMenus();
            foreach (var panel in this.Controls.OfType<Panel>())
            {
                panel.Height = 80;
            }
            OnMenuSelected?.Invoke(this, "Reservaciones");
        }

        //Menu Check In/Out
        private void button7_Click(object sender, EventArgs e)
        {
            HideAllSubMenus();
            foreach (var panel in this.Controls.OfType<Panel>())
            {
                panel.Height = 80;
            }
            ShowSubMenu(panel2);
        }

        //Submenu Check In
        private void button6_Click(object sender, EventArgs e)
        {
            OnMenuSelected?.Invoke(this, "CheckIn");
        }

        //Submenu Check Out
        private void button5_Click(object sender, EventArgs e)
        {
            OnMenuSelected?.Invoke(this, "CheckOut");
        }
    }
}
