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
using PIA_MAD_FyD.Helpers;

namespace PIA_MAD_FyD.ToolTips_PopUps
{
    public partial class PasswordCheck: Form
    {
        private BorderRadius borderRadius = new BorderRadius();

        public PasswordCheck()
        {
            InitializeComponent();
            this.Opacity = 1;
            borderRadius.TargetControl = this;
            borderRadius.CornerRadius = 20; 
            borderRadius.CornersToRound = BorderRadius.RoundedCorners.All;
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
    }
}
