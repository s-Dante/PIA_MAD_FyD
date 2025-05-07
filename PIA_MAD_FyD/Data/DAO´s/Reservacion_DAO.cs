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
        public static bool RealizarReserva(string idCliente, int idHotel, DateTime fechaInicio, DateTime fechaFin, int cantidadHuespedes, decimal anticipo, List<Habitacion> habitaciones, int usuarioRegistrador)
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

                    // Ejecutar el SP y obtener el id de la reserva
                    object resultado = comando.ExecuteScalar();

                    // Verificar si se recibió un id válido
                    if (resultado != null && Guid.TryParse(resultado.ToString(), out Guid idReservacion))
                    {
                        Console.WriteLine($"Reservación creada con ID: {idReservacion}");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error al obtener el ID de la reserva.");
                        return false;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Capturar errores del SP
                throw new Exception($"Error al realizar la reserva: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error no controlado al realizar la reserva: " + ex.Message, ex);
            }
        }



    }
}
