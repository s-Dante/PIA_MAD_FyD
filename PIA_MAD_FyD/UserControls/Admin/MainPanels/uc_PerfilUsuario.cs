using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PIA_MAD_FyD.Helpers;

namespace PIA_MAD_FyD.UserControls.Admin.MainPanels
{
    public partial class uc_PerfilUsuario: UserControl
    {
        BorderRadius borderRadius = new BorderRadius();
        public uc_PerfilUsuario()
        {
            InitializeComponent();

            borderRadius.TargetControl = this;
            borderRadius.CornerRadius = 25;
            borderRadius.CornersToRound = BorderRadius.RoundedCorners.All;
        }

        private void uc_PerfilUsuario_Load(object sender, EventArgs e)
        {

        }
    }
}
