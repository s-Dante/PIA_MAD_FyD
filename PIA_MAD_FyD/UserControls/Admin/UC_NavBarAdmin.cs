using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PIA_MAD_FyD.Forms.Admin;

namespace PIA_MAD_FyD.UserControls.Admin
{
    public partial class UC_NavBarAdmin: UserControl
    {
        //Variables para la animación
        Timer slideTimer = new Timer();
        Panel currentSubMenu;
        bool isExpanding = false;
        int targetHeight = 0;
        Queue<Panel> panelsToHide = new Queue<Panel>();

        //Evento para seleccionar el menú
        public event EventHandler<string> OnMenuSelected;

        public UC_NavBarAdmin()
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

        private void UC_NavBarAdmin_Load(object sender, EventArgs e)
        {

        }
        private void UC_NavBarAdmin_Load_1(object sender, EventArgs e)
        {

        }

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button11 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Moccasin;
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Peru;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.button1.Size = new System.Drawing.Size(233, 80);
            this.button1.TabIndex = 2;
            this.button1.Text = "Perfil";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.PeachPuff;
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(233, 200);
            this.panel1.TabIndex = 3;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.PeachPuff;
            this.button4.Dock = System.Windows.Forms.DockStyle.Top;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Peru;
            this.button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.button4.Location = new System.Drawing.Point(0, 130);
            this.button4.Name = "button4";
            this.button4.Padding = new System.Windows.Forms.Padding(45, 0, 0, 0);
            this.button4.Size = new System.Drawing.Size(233, 50);
            this.button4.TabIndex = 5;
            this.button4.Text = "Modificar";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.PeachPuff;
            this.button3.Dock = System.Windows.Forms.DockStyle.Top;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Peru;
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.button3.Location = new System.Drawing.Point(0, 80);
            this.button3.Name = "button3";
            this.button3.Padding = new System.Windows.Forms.Padding(45, 0, 0, 0);
            this.button3.Size = new System.Drawing.Size(233, 50);
            this.button3.TabIndex = 4;
            this.button3.Text = "Registrar";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Moccasin;
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Peru;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(0, 0);
            this.button2.Name = "button2";
            this.button2.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.button2.Size = new System.Drawing.Size(233, 80);
            this.button2.TabIndex = 3;
            this.button2.Text = "Control de Usuarios";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.PeachPuff;
            this.panel2.Controls.Add(this.button5);
            this.panel2.Controls.Add(this.button6);
            this.panel2.Controls.Add(this.button7);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 280);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(233, 200);
            this.panel2.TabIndex = 4;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.PeachPuff;
            this.button5.Dock = System.Windows.Forms.DockStyle.Top;
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Peru;
            this.button5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.button5.Location = new System.Drawing.Point(0, 130);
            this.button5.Name = "button5";
            this.button5.Padding = new System.Windows.Forms.Padding(45, 0, 0, 0);
            this.button5.Size = new System.Drawing.Size(233, 50);
            this.button5.TabIndex = 5;
            this.button5.Text = "Modificar";
            this.button5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.PeachPuff;
            this.button6.Dock = System.Windows.Forms.DockStyle.Top;
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Peru;
            this.button6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.button6.Location = new System.Drawing.Point(0, 80);
            this.button6.Name = "button6";
            this.button6.Padding = new System.Windows.Forms.Padding(45, 0, 0, 0);
            this.button6.Size = new System.Drawing.Size(233, 50);
            this.button6.TabIndex = 4;
            this.button6.Text = "Registrar";
            this.button6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Moccasin;
            this.button7.Dock = System.Windows.Forms.DockStyle.Top;
            this.button7.FlatAppearance.BorderSize = 0;
            this.button7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Peru;
            this.button7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.Location = new System.Drawing.Point(0, 0);
            this.button7.Name = "button7";
            this.button7.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.button7.Size = new System.Drawing.Size(233, 80);
            this.button7.TabIndex = 3;
            this.button7.Text = "Control de Hoteles";
            this.button7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.PeachPuff;
            this.panel3.Controls.Add(this.button11);
            this.panel3.Controls.Add(this.button8);
            this.panel3.Controls.Add(this.button9);
            this.panel3.Controls.Add(this.button10);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 480);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(233, 245);
            this.panel3.TabIndex = 5;
            // 
            // button11
            // 
            this.button11.BackColor = System.Drawing.Color.PeachPuff;
            this.button11.Dock = System.Windows.Forms.DockStyle.Top;
            this.button11.FlatAppearance.BorderSize = 0;
            this.button11.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Peru;
            this.button11.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.button11.Location = new System.Drawing.Point(0, 180);
            this.button11.Name = "button11";
            this.button11.Padding = new System.Windows.Forms.Padding(45, 0, 0, 0);
            this.button11.Size = new System.Drawing.Size(233, 50);
            this.button11.TabIndex = 6;
            this.button11.Text = "Hist. de Clientes";
            this.button11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button11.UseVisualStyleBackColor = false;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.PeachPuff;
            this.button8.Dock = System.Windows.Forms.DockStyle.Top;
            this.button8.FlatAppearance.BorderSize = 0;
            this.button8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Peru;
            this.button8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.button8.Location = new System.Drawing.Point(0, 130);
            this.button8.Name = "button8";
            this.button8.Padding = new System.Windows.Forms.Padding(45, 0, 0, 0);
            this.button8.Size = new System.Drawing.Size(233, 50);
            this.button8.TabIndex = 5;
            this.button8.Text = "Ventas";
            this.button8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.PeachPuff;
            this.button9.Dock = System.Windows.Forms.DockStyle.Top;
            this.button9.FlatAppearance.BorderSize = 0;
            this.button9.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Peru;
            this.button9.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.button9.Location = new System.Drawing.Point(0, 80);
            this.button9.Name = "button9";
            this.button9.Padding = new System.Windows.Forms.Padding(45, 0, 0, 0);
            this.button9.Size = new System.Drawing.Size(233, 50);
            this.button9.TabIndex = 4;
            this.button9.Text = "Ocupacion";
            this.button9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.Moccasin;
            this.button10.Dock = System.Windows.Forms.DockStyle.Top;
            this.button10.FlatAppearance.BorderSize = 0;
            this.button10.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Peru;
            this.button10.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.Location = new System.Drawing.Point(0, 0);
            this.button10.Name = "button10";
            this.button10.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.button10.Size = new System.Drawing.Size(233, 80);
            this.button10.TabIndex = 3;
            this.button10.Text = "Reportes";
            this.button10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // UC_NavBarAdmin
            // 
            this.BackColor = System.Drawing.Color.Moccasin;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Name = "UC_NavBarAdmin";
            this.Size = new System.Drawing.Size(233, 772);
            this.Load += new System.EventHandler(this.UC_NavBarAdmin_Load_1);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

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
            if(subMenu.Name == "panel3")
            {
                targetHeight = 245;
            }else
            {
                targetHeight = 200; 
            }
            isExpanding = true;
            slideTimer.Start();
        }


        //Menu Perfil
        private void button1_Click(object sender, EventArgs e)
        {
            HideAllSubMenus();
            OnMenuSelected?.Invoke(this, "Usuarios"); //Agregar a cada boton correspondiente para mostrar otro control
        }

        //Menu Control de Usuarios
        private void button2_Click(object sender, EventArgs e)
        {
            HideAllSubMenus(); // Cierra todos
            foreach (var panel in this.Controls.OfType<Panel>())
            {
                panel.Height = 80;
            }
            ShowSubMenu(panel1);
        }

        //Submenu Registrar Usuarios
        private void button3_Click(object sender, EventArgs e)
        {
            OnMenuSelected?.Invoke(this, "RegistrarUsuario");
        }

        //Submenu Modificar Usuarios
        private void button4_Click(object sender, EventArgs e)
        {
            OnMenuSelected?.Invoke(this, "ModificarUsuario");
        }

        //Menu Control de Hoteles
        private void button7_Click(object sender, EventArgs e)
        {
            HideAllSubMenus(); // Cierra todos
            foreach (var panel in this.Controls.OfType<Panel>())
            {
                panel.Height = 80;
            }
            ShowSubMenu(panel2);
        }

        //Submenu Registrar Hoteles
        private void button6_Click(object sender, EventArgs e)
        {
            OnMenuSelected?.Invoke(this, "RegistrarHotel");
        }

        //Submenu Modificar Hoteles
        private void button5_Click(object sender, EventArgs e)
        {
            OnMenuSelected?.Invoke(this, "ModifciarHotel");
        }

        //Menu Reportes
        private void button10_Click(object sender, EventArgs e) 
        {
            HideAllSubMenus(); // Cierra todos
            foreach (var panel in this.Controls.OfType<Panel>())
            {
                panel.Height = 80;
            }
            ShowSubMenu(panel3);
        }

        //Submenu Reportes Ocupacion
        private void button9_Click(object sender, EventArgs e)
        {
            OnMenuSelected?.Invoke(this, "ReporteOcupacion");
        }

        //Submenu Reportes Ventas
        private void button8_Click(object sender, EventArgs e)
        {
            OnMenuSelected?.Invoke(this, "ReporteVentas");
        }

        //Submenu Historial de Clientes
        private void button11_Click(object sender, EventArgs e)
        {
            OnMenuSelected?.Invoke(this, "ReporteHistorialCliente");
        }
    }
}
