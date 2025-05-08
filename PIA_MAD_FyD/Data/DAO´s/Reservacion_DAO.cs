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
    }
}
