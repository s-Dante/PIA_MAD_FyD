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

            checkedListBox1.Items.Clear();
            List<ServiciosExtra> listaServiciosExtra = Reservacion_DAO.ObtenerServiciosExtras();
            foreach (ServiciosExtra a in listaServiciosExtra)
            {
                checkedListBox1.Items.Add(a);
            }
        }

        private void uc_CheckOut_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            checkedListBox1.Enabled = false;
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
                if (Guid.TryParse(textBox2.Text, out idReservacionActual))
                {
                    var reservacion = Reservacion_DAO.ConsultarReservacionCheckOut(idReservacionActual);

                    if (reservacion != null)
                    {
                        // Añadir los datos al ListView
                        string mensaje = $"Reservacion encontrada:.\n\n" +
                                       $"Código: {reservacion["id_Reservacion"]}\n" +
                                       $"Fecha Inicio: {reservacion["fecha_Ini"]}\n" +
                                       $"Fecha Fin: {reservacion["fecha_Fin"]}\n" +
                                       $"Habitaciones: {reservacion["cant_Habitaciones"]}\n" +
                                       $"Tipos: {reservacion["tipos_Habitaciones"]}\n" +
                                       $"Huéspedes: {reservacion["cant_Huespedes"]}\n" +
                                       $"Anticipo: {reservacion["anticipo_Pagado"]}";

                        MessageBox.Show(mensaje, "Reserva Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        button2.Enabled = true;
                        checkedListBox1.Enabled = true;
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

        //Lista de chequeo de servicios extra
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Boton de checkout
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string mensaje = $"Seguro desea registrar el Check-Out con los siguientes servicios extra:\n\n";

                // Agregar los servicios extra seleccionados al mensaje
                foreach (var item in checkedListBox1.CheckedItems)
                {
                    if (item is ServiciosExtra servicio)
                    {
                        mensaje += $"- {servicio.nombre}: {servicio.precion:C}\n";
                    }
                }

                DialogResult aceptarCheckOut = MessageBox.Show(mensaje, "Confirmar Check-Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (aceptarCheckOut == DialogResult.Yes)
                {
                    int usuarioRegistrador = usuarioLogeado.num_Nomina;
                    idCheckOut = Reservacion_DAO.RealizarCheckOut(idReservacionActual, usuarioRegistrador);

                    if (idCheckOut > 0)
                    {
                        // Registrar los servicios extra seleccionados
                        RegistrarServiciosSeleccionados();

                        // Registrar descuentos automáticos
                        bool descuentosRegistrados = Reservacion_DAO.RegistrarDescuentosCheckOut(idCheckOut, idReservacionActual);

                        if (!descuentosRegistrados)
                        {
                            MessageBox.Show("No se registraron descuentos automáticos o no aplicaban descuentos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        MessageBox.Show("Check-Out realizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Calcular el monto total después de registrar servicios y descuentos
                        decimal montoTotal = Pago_DAO.CalcularMontoTotalCheckOut(idCheckOut);

                        // Mostrar la ventana de pago
                        Form operatividad = new Operatividad(usuarioLogeado);
                        FormManager.ShowFormParams<FormPago>(operatividad, cerrarAppAlCerrar: false, ocultarActual: true, idCheckOut, montoTotal, usuarioLogeado);
                    }
                    else
                    {
                        MessageBox.Show("Error al realizar el Check-Out.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Check-Out cancelado. No se registraron servicios extra ni descuentos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al realizar el Check-Out: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //Servicios extra
        private void RegistrarServiciosSeleccionados()
        {
            try
            {
                if (idCheckOut <= 0)
                {
                    MessageBox.Show("No se ha registrado el Check-Out correctamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    using (SqlTransaction transaccion = conexion.BeginTransaction())
                    {
                        try
                        {
                            // Insertar Servicios Extra Seleccionados
                            foreach (var item in checkedListBox1.CheckedItems)
                            {
                                if (item is ServiciosExtra servicio)
                                {
                                    string queryInsertServicio = @"
                                INSERT INTO tbl_ServicioCheckOut (id_CheckOut, id_ServicioExtra)
                                VALUES (@idCheckOut, @idServicioExtra)";

                                    using (SqlCommand cmd = new SqlCommand(queryInsertServicio, conexion, transaccion))
                                    {
                                        cmd.Parameters.AddWithValue("@idCheckOut", idCheckOut);
                                        cmd.Parameters.AddWithValue("@idServicioExtra", servicio.id_ServicioExtrta);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }

                            // Si no hubo errores, confirmar transacción
                            transaccion.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaccion.Rollback();
                            MessageBox.Show("Error al registrar los servicios extra: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar los servicios extra: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        ////Boton de imprimir
        //private void button3_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Guid idReservacion = Guid.Parse(textBox2.Text); // Asumiendo que el textbox tiene el ID de la reservación
        //        int idCheckOut = Reservacion_DAO.ObtenerIdCheckOutPorReservacion(idReservacion); // Obtener el ID de check-out
        //        decimal montoTotal = Pago_DAO.CalcularMontoTotalCheckOut(idCheckOut); // Calcular monto total

        //        Form operatividad = new Operatividad(usuarioLogeado);
        //        FormManager.ShowFormParams<FormPago>(operatividad, cerrarAppAlCerrar: false, ocultarActual: true, idCheckOut, montoTotal, usuarioLogeado);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error al iniciar el pago: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

    }
}
