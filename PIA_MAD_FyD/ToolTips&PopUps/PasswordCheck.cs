using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PIA_MAD_FyD.ToolTips_PopUps
{
    public partial class PasswordCheck: Form
    {
        public PasswordCheck()
        {
            InitializeComponent();
            this.Opacity = 0.98;

            this.Paint += PasswordCheck_Paint;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint |
              ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void PasswordCheck_Load(object sender, EventArgs e)
        {

        }

        //Aqui hacemos que el popup se genere sin quitar el foco del textbox
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Owner?.ActiveControl?.Focus(); // Restaura el foco al control activo del formulario padre
        }

        public void UpdateValidation(bool length, bool uppercase, bool lowercase, bool digit, bool specialChar)
        {
            label1.ForeColor = length ? Color.Green : Color.Red;
            label2.ForeColor = uppercase ? Color.Green : Color.Red;
            label3.ForeColor = lowercase ? Color.Green : Color.Red;
            label4.ForeColor = digit ? Color.Green : Color.Red;
            label5.ForeColor = specialChar ? Color.Green : Color.Red;

            label1.Text = length ? "✔️ Mínimo 8 caracteres" : "❌ Mínimo 8 caracteres";
            label2.Text = uppercase ? "✔️ Al menos una mayúscula" : "❌ Al menos una mayúscula";
            label3.Text = lowercase ? "✔️ Al menos una minúscula" : "❌ Al menos una minúscula";
            label4.Text = digit ? "✔️ Al menos un dígito" : "❌ Al menos un dígito";
            label5.Text = specialChar ? "✔️ Al menos un carácter especial" : "❌ Al menos un carácter especial";
        }

        // Evento Paint para bordes suaves
        private void PasswordCheck_Paint(object sender, PaintEventArgs e)
        {
            int radius = 15;  // Aumentamos el radio para una mejor curva
            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality; // Mayor suavidad en el renderizado

            using (GraphicsPath path = GetRoundedPath(this.ClientRectangle, radius))
            {
                using (SolidBrush brush = new SolidBrush(this.BackColor)) // Fondo del formulario
                {
                    g.FillPath(brush, path); // Dibuja el fondo de forma redondeada
                }

                using (Pen borderPen = new Pen(Color.FromArgb(180, 180, 180), 2)) // Borde más limpio
                {
                    g.DrawPath(borderPen, path);
                }

                this.Region = new Region(path); // Aplica el recorte al formulario
            }
        }


        // Método para crear el borde redondeado
        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.Left, rect.Top, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - (radius * 2), rect.Top, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - (radius * 2), rect.Bottom - (radius * 2), radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - (radius * 2), radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

    }
}
