using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PIA_MAD_FyD.Data.DAO_s;
using PIA_MAD_FyD.Data.Entidades;

namespace PIA_MAD_FyD.UserControls.Operatives.MainPanels
{
    public partial class uc_CheckIn: UserControl
    {
        public Usuario usuarioLogeado;
        public uc_CheckIn(Usuario usuarioLogeado)
        {
            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;
        }

        //Codigo de Reservacion
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        //Realizar CHeckIn
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Guid idReservacion;
                if (!Guid.TryParse(textBox2.Text, out idReservacion))
                {
                    MessageBox.Show("El código de reservación no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int usuarioRegistro = usuarioLogeado.num_Nomina; // Reemplaza con el usuario logueado actual

                bool checkInExitoso = Reservacion_DAO.RegistrarCheckIn(idReservacion, usuarioRegistro);

                if (checkInExitoso)
                {
                    MessageBox.Show("Check-In registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al realizar el check-in: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
