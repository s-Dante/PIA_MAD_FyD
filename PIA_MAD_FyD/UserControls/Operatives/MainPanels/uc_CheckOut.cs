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
using PIA_MAD_FyD.Helpers.FormManager;
using PIA_MAD_FyD.Forms.Operatives;
using PIA_MAD_FyD.Data;
using System.Data.SqlClient;

namespace PIA_MAD_FyD.UserControls.Operatives.MainPanels
{
    public partial class uc_CheckOut: UserControl
    {
        private int idCheckOut = -1;
        private Guid idReservacionActual;
        Usuario usuarioLogeado;
        public uc_CheckOut(Usuario usuarioLogeado)
        {
            InitializeComponent();
            this.usuarioLogeado = usuarioLogeado;
        }

        private void uc_CheckOut_Load(object sender, EventArgs e)
        {
            CargarServiciosExtras();
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void CargarServiciosExtras()
        {
            try
            {
                checkedListBox1.Items.Clear();
                var servicios = Reservacion_DAO.ObtenerServiciosExtras();

                foreach (var servicio in servicios)
                {
                    checkedListBox1.Items.Add(servicio);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar servicios extra: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //Codigo de Reservacion
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        //Buscar Reservacion
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.Clear();

                if (Guid.TryParse(textBox2.Text, out idReservacionActual))
                {
                    var reservacion = Reservacion_DAO.ConsultarReservacionCheckOut(idReservacionActual);

                    if (reservacion != null)
                    {
                        // Añadir los datos al ListView
                        listView1.Items.Add(new ListViewItem(new[] { "Fecha Inicio", reservacion["fecha_Ini"].ToString() }));
                        listView1.Items.Add(new ListViewItem(new[] { "Fecha Fin", reservacion["fecha_Fin"].ToString() }));
                        listView1.Items.Add(new ListViewItem(new[] { "Cantidad de Huéspedes", reservacion["cant_Huespedes"].ToString() }));
                        listView1.Items.Add(new ListViewItem(new[] { "Anticipo Pagado", reservacion["anticipo_Pagado"].ToString() }));
                        listView1.Items.Add(new ListViewItem(new[] { "Habitaciones", reservacion["tipos_Habitaciones"].ToString() }));

                        button2.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Reservación no encontrada o ya cancelada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Ingrese un ID de reservación válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar la reservación: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Datos reservacion
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Lista de chequeo de servicios extra
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Boton de checkout
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int usuarioRegistrador = usuarioLogeado.num_Nomina;
                idCheckOut = Reservacion_DAO.RealizarCheckOut(idReservacionActual, usuarioRegistrador);

                if (idCheckOut > 0)
                {
                    RegistrarServiciosSeleccionados();
                    MessageBox.Show("Check-Out realizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button3.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Error al realizar el Check-Out.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al realizar el Check-Out: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void RegistrarServiciosSeleccionados()
        {
            try
            {
                if (idCheckOut <= 0)
                {
                    MessageBox.Show("No se ha registrado el Check-Out correctamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                List<string> serviciosSeleccionadosNombres = new List<string>();
                List<string> descuentosSeleccionadosNombres = new List<string>();

                foreach (var item in checkedListBox1.CheckedItems)
                {
                    if (item is ServiciosExtra servicio)
                    {
                        serviciosSeleccionadosNombres.Add(servicio.nombre);
                    }

                    if (item is Descuento descuento)
                    {
                        descuentosSeleccionadosNombres.Add(descuento.nombre);
                    }
                }

                // Obtener los IDs de los servicios seleccionados
                List<int> serviciosSeleccionadosIds = Reservacion_DAO.ObtenerIdsServiciosExtras(serviciosSeleccionadosNombres);

                if (serviciosSeleccionadosIds.Count > 0)
                {
                    Reservacion_DAO.RegistrarServiciosCheckOut(idCheckOut, idReservacionActual, checkedListBox1);
                }

                // Obtener los IDs de los descuentos seleccionados
                List<int> descuentosSeleccionadosIds = Reservacion_DAO.ObtenerIdsDescuentos(descuentosSeleccionadosNombres);

                // Registrar los descuentos seleccionados
                if (descuentosSeleccionadosIds.Count > 0)
                {
                    Reservacion_DAO.RegistrarDescuentosCheckOut(idCheckOut, idReservacionActual);
                }


                // Calcular el monto total
                decimal montoTotal = Reservacion_DAO.CalcularMontoTotal(idReservacionActual, idCheckOut);

                if (montoTotal >= 0)
                {
                    // Proceder a la ventana de pago
                    Form operatividad = new Operatividad(usuarioLogeado);
                    FormManager.ShowFormParams<FormPago>(operatividad, cerrarAppAlCerrar: false, ocultarActual: true, idCheckOut, montoTotal, usuarioLogeado);
                }
                else
                {
                    MessageBox.Show("Error al calcular el monto total. Verifique los datos y vuelva a intentarlo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar los servicios extra y proceder al pago: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        //Boton de imprimir
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Guid idReservacion = Guid.Parse(textBox2.Text); // Asumiendo que el textbox tiene el ID de la reservación
                int idCheckOut = Reservacion_DAO.ObtenerIdCheckOutPorReservacion(idReservacion); // Obtener el ID de check-out
                decimal montoTotal = Pago_DAO.CalcularMontoTotalCheckOut(idCheckOut); // Calcular monto total

                Form operatividad = new Operatividad(usuarioLogeado);
                FormManager.ShowFormParams<FormPago>(operatividad, cerrarAppAlCerrar: false, ocultarActual: true, idCheckOut, montoTotal, usuarioLogeado);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar el pago: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
