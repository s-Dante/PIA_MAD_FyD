using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIA_MAD_FyD.Data.Entidades;

namespace PIA_MAD_FyD.Data.DAO_s
{
    class Reservacion_DAO
    {
        //Realizar reservacion
        public static Dictionary<string, object> RealizarReserva(string idCliente, int idHotel, DateTime fechaInicio, DateTime fechaFin, int cantidadHuespedes, decimal anticipo, List<Habitacion> habitaciones, int usuarioRegistrador)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_RealizarReserva", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@idCliente", idCliente);
                    comando.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    comando.Parameters.AddWithValue("@fechaFin", fechaFin);
                    comando.Parameters.AddWithValue("@cantidadHuespedes", cantidadHuespedes);
                    comando.Parameters.AddWithValue("@anticipo", anticipo);
                    comando.Parameters.AddWithValue("@usuarioRegistrador", usuarioRegistrador);

                    string habitacionesCSV = string.Join(",", habitaciones.Select(h => h.id_Habitacion.ToString()));
                    comando.Parameters.AddWithValue("@habitacionesCSV", habitacionesCSV);

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Dictionary<string, object>
                    {
                        { "id_Reservacion", reader["id_Reservacion"].ToString() },
                        { "fecha_Ini", reader["fecha_Ini"] },
                        { "fecha_Fin", reader["fecha_Fin"] },
                        { "cant_Habitaciones", reader["cant_Habitaciones"] },
                        { "cant_Huespedes", reader["cant_Huespedes"] },
                        { "anticipo_Pagado", reader["anticipo_Pagado"] },
                        { "tipos_Habitaciones", reader["tipos_Habitaciones"].ToString() }
                    };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al realizar la reserva: " + ex.Message);
            }

            return null;
        }



        //Cancelar reservacion si no hay checkIN
        public static void CancelarReservacionesSinCheckIn()
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_CancelarReservacionesSinCheckIn", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    // Ejecutar el SP
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Manejar el error (puedes registrar el error o mostrar un mensaje)
                throw new Exception("Error al cancelar reservaciones sin check-in: " + ex.Message);
            }
        }


        //Canselar reservacion activamente
        // Cancelar reservación activamente
        public static void CancelarReservacion(Guid idReservacion, string motivo, int usuarioRegistrador)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_CancelarReservacionActiva", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@idReservacion", idReservacion);
                    comando.Parameters.AddWithValue("@usuarioCancelador", usuarioRegistrador);
                    comando.Parameters.AddWithValue("@motivoCancelacion", motivo);

                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cancelar la reservación: " + ex.Message);
            }
        }


        //Consultar reservacion
        // Consultar reservación
        public static Dictionary<string, object> ConsultarReservacion(Guid idReservacion)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_ConsultarReservacion", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@idReservacion", idReservacion);

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Dictionary<string, object>
                    {
                        { "existe", true },
                        { "id_Reservacion", reader["id_Reservacion"].ToString() },
                        { "fecha_Ini", reader["fecha_Ini"] },
                        { "fecha_Fin", reader["fecha_Fin"] },
                        { "cant_Habitaciones", reader["cant_Habitaciones"] },
                        { "cant_Huespedes", reader["cant_Huespedes"] },
                        { "anticipo_Pagado", reader["anticipo_Pagado"] },
                        { "tipos_Habitaciones", reader["tipos_Habitaciones"].ToString() }
                    };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar la reservación: " + ex.Message);
            }

            return new Dictionary<string, object> { { "existe", false } };
        }


        //Hacer CheckIn
        public static bool RegistrarCheckIn(Guid idReservacion, int usuarioRegistro)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_RegistrarCheckIn", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@idReservacion", idReservacion);
                    comando.Parameters.AddWithValue("@usuarioRegistro", usuarioRegistro);

                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el check-in: " + ex.Message);
            }
        }


        //Hacer CheckOut
        // 1. Consultar si la reservación está activa para el check-out
        public static Dictionary<string, object> ConsultarReservacionCheckOut(Guid idReservacion)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_ConsultarReservacion", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@idReservacion", idReservacion);

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Dictionary<string, object>
                            {
                                { "id_Reservacion", reader["id_Reservacion"].ToString() },
                                { "fecha_Ini", reader["fecha_Ini"] },
                                { "fecha_Fin", reader["fecha_Fin"] },
                                { "cant_Habitaciones", reader["cant_Habitaciones"] },
                                { "cant_Huespedes", reader["cant_Huespedes"] },
                                { "anticipo_Pagado", reader["anticipo_Pagado"] },
                                { "tipos_Habitaciones", reader["tipos_Habitaciones"].ToString() }
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar la reservación para el check-out: " + ex.Message);
            }
            return null;
        }

        // 2. Realizar el check-out
        public static int RealizarCheckOut(Guid idReservacion, int usuarioRegistrador)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_RegistrarCheckOut", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@idReservacion", idReservacion);
                    comando.Parameters.AddWithValue("@usuarioRegistrador", usuarioRegistrador);

                    return Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al realizar el check-out: " + ex.Message);
            }
        }

        // 3. Obtener servicios extra disponibles
        public static List<string> ObtenerServiciosExtras()
        {
            List<string> servicios = new List<string>();
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("SELECT nombre FROM tbl_ServicioExtra", conexion);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            servicios.Add(reader["nombre"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los servicios extra: " + ex.Message);
            }
            return servicios;
        }


        public static List<int> ObtenerIdsServiciosExtras(List<string> nombresServicios)
        {
            List<int> idsServicios = new List<int>();

            if (nombresServicios.Count == 0)
                return idsServicios;

            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    // Crear una lista de parámetros dinámicos
                    string parametros = string.Join(",", nombresServicios.Select((_, index) => $"@nombre{index}"));
                    string query = $"SELECT id_ServicioExtra FROM tbl_ServicioExtra WHERE nombre IN ({parametros})";

                    SqlCommand comando = new SqlCommand(query, conexion);

                    // Asignar los parámetros
                    for (int i = 0; i < nombresServicios.Count; i++)
                    {
                        comando.Parameters.AddWithValue($"@nombre{i}", nombresServicios[i]);
                    }

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idsServicios.Add(Convert.ToInt32(reader["id_ServicioExtra"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los IDs de los servicios extra: " + ex.Message);
            }

            return idsServicios;
        }


        // 4. Registrar servicios seleccionados en el check-out
        public static void RegistrarServiciosCheckOut(int idCheckOut, List<int> serviciosSeleccionados)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    foreach (int idServicio in serviciosSeleccionados)
                    {
                        SqlCommand comando = new SqlCommand("sp_RegistrarServicioCheckOut", conexion);
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@idCheckOut", idCheckOut);
                        comando.Parameters.AddWithValue("@idServicioExtra", idServicio);
                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar los servicios extra en el check-out: " + ex.Message);
            }
        }

        // 5. Registrar el pago
        public static void RegistrarPago(int idCheckOut, char formaPago, decimal total, int usuarioRegistrador)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_RegistrarPagoReservacion", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@idCheckOut", idCheckOut);
                    comando.Parameters.AddWithValue("@formaPago", formaPago);
                    comando.Parameters.AddWithValue("@total", total);
                    comando.Parameters.AddWithValue("@usuarioRegistrador", usuarioRegistrador);
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el pago: " + ex.Message);
            }
        }

        // 6. Generar factura
        public static void GenerarFactura(int idCheckOut, string rfcReceptor, string nombreReceptor, string usoCFDI,
                                          string metodoPago, string formaPago, string lugarEmision, int usuarioRegistrador)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_GenerarFacturaReservacion", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@idCheckOut", idCheckOut);
                    comando.Parameters.AddWithValue("@rfcReceptor", rfcReceptor);
                    comando.Parameters.AddWithValue("@nombreReceptor", nombreReceptor);
                    comando.Parameters.AddWithValue("@usoCFDI", usoCFDI);
                    comando.Parameters.AddWithValue("@metodoPago", metodoPago);
                    comando.Parameters.AddWithValue("@formaPago", formaPago);
                    comando.Parameters.AddWithValue("@lugarEmision", lugarEmision);
                    comando.Parameters.AddWithValue("@usuarioRegistrador", usuarioRegistrador);
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar la factura: " + ex.Message);
            }
        }

        // 7. Obtener ID del CheckOut por Reservación
        public static int ObtenerIdCheckOutPorReservacion(Guid idReservacion)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    using (SqlCommand comando = new SqlCommand("sp_ObtenerCheckOutPorReservacion", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@idReservacion", idReservacion);

                        object resultado = comando.ExecuteScalar();

                        if (resultado != null && int.TryParse(resultado.ToString(), out int idCheckOut))
                        {
                            return idCheckOut;
                        }
                        else
                        {
                            throw new Exception("No se encontró un CheckOut asociado a la reservación.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el ID del CheckOut: " + ex.Message);
            }
        }

    }
}
