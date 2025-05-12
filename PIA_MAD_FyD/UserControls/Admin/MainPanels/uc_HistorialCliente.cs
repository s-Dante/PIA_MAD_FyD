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
using System.Reflection.Emit;
using PIA_MAD_FyD.Data.Entidades;
using PIA_MAD_FyD.Helpers.HelpClasses;

namespace PIA_MAD_FyD.UserControls.Admin.MainPanels
{
    public partial class uc_HistorialCliente: UserControl
    {
        Cliente clienteReservador;

        public uc_HistorialCliente()
        {
            InitializeComponent();

            LlenarComboBoxAnios(comboBox3);
        }

        private void LlenarComboBoxAnios(ComboBox comboBox)
        {
            int anioActual = DateTime.Now.Year;

            comboBox.Items.Clear();
            for (int i = anioActual; i >= anioActual - 10; i--)
            {
                comboBox.Items.Add(i.ToString());
            }

            comboBox.SelectedIndex = 0; // Selecciona el año más reciente por defecto
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        //RFC
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void uc_HistorialCliente_Load(object sender, EventArgs e)
        {

        }

        //AÑO
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Buscar Cliente
        private void button2_Click(object sender, EventArgs e)
        {
            string rfc = textBox2.Text;

            if (string.IsNullOrWhiteSpace(rfc))
            {
                MessageBox.Show("Introduce un RFC válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            clienteReservador = Cliente_DAO.ObtenerClientePorRFC(rfc);
            if (clienteReservador != null)
            {
                MessageBox.Show($"Cliente encontrado\nDatos:\nNombre: {clienteReservador.nombre} {clienteReservador.apellido_Paterno} {clienteReservador.apellido_Materno}\n" +
                                $"Ubicación: {clienteReservador.ubicacion_Cliente.pais}, {clienteReservador.ubicacion_Cliente.estado}, {clienteReservador.ubicacion_Cliente.ciudad}\n" +
                                $"Correo: {clienteReservador.correo}\n" + $"Telefono: {clienteReservador.telefono}",
                                "AVISO", MessageBoxButtons.OK);

                string nombreCompleto = $"{clienteReservador.nombre} {clienteReservador.apellido_Paterno} {clienteReservador.apellido_Materno}";
            }
            else
            {
                MessageBox.Show("Cliente no encontrado");
            }
        }

        //ListView para mostrar reporte
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Generar Reporte
        private void button1_Click(object sender, EventArgs e)
        {
            string rfc = textBox2.Text.ToString();
            string añoSeleccionado = comboBox3.SelectedItem.ToString();
            int? año = añoSeleccionado != "Todos" ? int.Parse(añoSeleccionado) : (int?)null;

            CargarHistorialCliente(rfc, año);
        }

        private void CargarHistorialCliente(string rfc, int? año)
        {
            listView1.Items.Clear();

            List<ReservacionHelp> historial = Reservacion_DAO.ObtenerHistorialCliente(rfc, año);

            foreach (var reservacion in historial)
            {
                // Concatenar IDs de habitaciones en una cadena separada por comas
                string habitacionesString = reservacion.habitaciones.Count > 0
                    ? string.Join(", ", reservacion.habitaciones)
                    : "Sin habitaciones asignadas";

                ListViewItem item = new ListViewItem(reservacion.clienteNombre);
                item.SubItems.Add(reservacion.ciudad);
                item.SubItems.Add(reservacion.hotelNombre);
                item.SubItems.Add(habitacionesString);
                item.SubItems.Add(reservacion.cantHabitaciones.ToString());
                item.SubItems.Add(reservacion.cantHuespedes.ToString());
                item.SubItems.Add(reservacion.idReservacion.ToString());
                item.SubItems.Add(reservacion.fechaInicio.ToString("dd/MM/yyyy"));
                item.SubItems.Add(reservacion.fechaFin.ToString("dd/MM/yyyy"));
                item.SubItems.Add(reservacion.fechaRegistro.ToString("dd/MM/yyyy"));
                item.SubItems.Add(reservacion.anticipoPagado.ToString("C"));

                listView1.Items.Add(item);
            }
        }


    }
}
