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
using PIA_MAD_FyD.Helpers;


namespace PIA_MAD_FyD.ToolTips_PopUps
{
    public partial class EmailCheck: UserControl
    {
        private BorderRadius borderRadius = new BorderRadius();

        public EmailCheck()
        {
            InitializeComponent();
            this.BackColor = Color.LightYellow;
            //this.BorderStyle = BorderStyle.None; // O quítalo si quieres un look limpio
            this.Padding = new Padding(5);
            this.Visible = false;

            // Aplicar bordes redondeados
            borderRadius.TargetControl = this;
            borderRadius.CornerRadius = 12;
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
    }
}
