using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace PIA_MAD_FyD.ToolTips_PopUps
{
    public partial class EmailCheck: UserControl
    {
        public EmailCheck()
        {
            InitializeComponent();
            this.BackColor = Color.LightYellow;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Padding = new Padding(5);
            this.Visible = false; // Inicialmente oculto
        }
        private void EmailCheck_Load(object sender, EventArgs e)
        {

        }

        // Método para configurar el mensaje del tooltip
        public void SetMessage(string message)
        {
            label1.Text = message;
            AdjustSize();
        }

        // Ajusta automáticamente el tamaño del control
        private void AdjustSize()
        {
            this.Width = label1.PreferredWidth + this.Padding.Left + this.Padding.Right;
            this.Height = label1.PreferredHeight + this.Padding.Top + this.Padding.Bottom;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int radius = 10; // Radio de las esquinas
            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Crear el borde redondeado
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, radius * 2, radius * 2, 180, 90); // Esquina superior izquierda
                path.AddArc(this.Width - (radius * 2), 0, radius * 2, radius * 2, 270, 90); // Esquina superior derecha
                path.AddArc(this.Width - (radius * 2), this.Height - (radius * 2), radius * 2, radius * 2, 0, 90); // Esquina inferior derecha
                path.AddArc(0, this.Height - (radius * 2), radius * 2, radius * 2, 90, 90); // Esquina inferior izquierda
                path.CloseFigure();

                // Establecer la región del control como el borde redondeado
                this.Region = new Region(path);

                // Dibujar el borde
                using (Pen borderPen = new Pen(Color.Gray, 1))
                {
                    g.DrawPath(borderPen, path);
                }
            }
        }
    }
}
