using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PIA_MAD_FyD.Data.Entidades;
using PIA_MAD_FyD.Data.DAO_s;

namespace PIA_MAD_FyD.UserControls.Admin.MainPanels
{
    public partial class uc_Cancelaciones: UserControl
    {
        private Usuario usuarioLogeado;
        public uc_Cancelaciones(Usuario usuarioLogeado)
        {
            InitializeComponent();

            this.usuarioLogeado = usuarioLogeado;

            button1.Visible = false; 
            textBox1.Enabled = false; //Motivo de cancelacion
        }

        private void uc_Cancelaciones_Load(object sender, EventArgs e)
        {

        }

        //Para ingresar el codigo de reservacion
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        //Btn para buscar y mostrar si exciste la reservacion
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

                var reservaInfo = Reservacion_DAO.ConsultarReservacion(idReservacion);

                if (!(bool)reservaInfo["existe"])
                {
                    MessageBox.Show("La reservación no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    button1.Visible = false;
                    textBox1.Enabled = false; //Motivo de cancelacion
                    return;
                }

                // Mostrar información de la reservación
                string mensaje = $"Reservación encontrada:\n" +
                                 $"Código: {reservaInfo["id_Reservacion"]}\n" +
                                 $"Fecha Inicio: {reservaInfo["fecha_Ini"]}\n" +
                                 $"Fecha Fin: {reservaInfo["fecha_Fin"]}\n" +
                                 $"Habitaciones: {reservaInfo["cant_Habitaciones"]}\n" +
                                 $"Tipos: {reservaInfo["tipos_Habitaciones"]}\n" +
                                 $"Huéspedes: {reservaInfo["cant_Huespedes"]}\n" +
                                 $"Anticipo: {reservaInfo["anticipo_Pagado"]}";

                MessageBox.Show(mensaje, "Información de la Reservación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Habilitar el TextBox y el botón de cancelación
                button1.Visible = true;
                textBox1.Enabled = true; //Motivo de cancelacion

                // Guardar el ID de la reservación en una variable de clase para usarlo después
                textBox2.Tag = idReservacion;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //Btn para cancelar la reservacion
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que el campo de motivo no esté vacío
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Por favor, ingresa un motivo para la cancelación.", "Motivo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener el ID de la reservación guardado en el Tag del TextBox
                Guid idReservacion = (Guid)textBox2.Tag;

                // Llamar al método para cancelar la reservación
                Reservacion_DAO.CancelarReservacion(idReservacion, textBox1.Text, usuarioLogeado.num_Nomina);

                MessageBox.Show("La reservación ha sido cancelada correctamente.", "Cancelación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar los campos y desactivar el botón
                textBox2.Clear();
                textBox1.Clear();
                textBox1.Enabled = false;
                button1.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al cancelar la reservación: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Para ingresar el motivo de la cancelacion
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
