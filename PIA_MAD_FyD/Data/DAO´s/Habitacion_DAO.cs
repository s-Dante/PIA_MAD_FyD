using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIA_MAD_FyD.Data.Entidades;
using System.Windows.Forms;
using System.Data;

namespace PIA_MAD_FyD.Data.DAO_s
{
    class Habitacion_DAO
    {
        //Insertar una habitacion
        public static bool InsertarHabitacion(Habitacion habitacion, List<int> amenidades)
        {
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                SqlTransaction transaccion = conexion.BeginTransaction();

                try
                {
                    // Insertar la habitación
                    SqlCommand cmdInsertHabitacion = new SqlCommand("sp_InsertarHabitacion", conexion, transaccion);
                    cmdInsertHabitacion.CommandType = CommandType.StoredProcedure;
                    cmdInsertHabitacion.Parameters.AddWithValue("@id_Hotel", habitacion.id_Hotel);
                    cmdInsertHabitacion.Parameters.AddWithValue("@num_Camas", habitacion.num_Camas);
                    cmdInsertHabitacion.Parameters.AddWithValue("@tipo_Cama", habitacion.tipo_Cama);
                    cmdInsertHabitacion.Parameters.AddWithValue("@precio", habitacion.precio);
                    cmdInsertHabitacion.Parameters.AddWithValue("@capacidad", habitacion.capacidad);
                    cmdInsertHabitacion.Parameters.AddWithValue("@nivel", habitacion.nivel);
                    cmdInsertHabitacion.Parameters.AddWithValue("@vista", habitacion.vista);
                    cmdInsertHabitacion.Parameters.AddWithValue("@usuario_Registrador", habitacion.usuario_Registrador);
                    cmdInsertHabitacion.Parameters.AddWithValue("@usuario_Modifico", habitacion.usuario_Modifico);

                    int idHabitacion = Convert.ToInt32(cmdInsertHabitacion.ExecuteScalar());

                    // Insertar las amenidades relacionadas
                    foreach (int idAmenidad in amenidades)
                    {
                        SqlCommand cmdAmenidad = new SqlCommand("sp_InsertarHabitacionAmenidad", conexion, transaccion);
                        cmdAmenidad.CommandType = CommandType.StoredProcedure;
                        cmdAmenidad.Parameters.AddWithValue("@id_Habitacion", idHabitacion);
                        cmdAmenidad.Parameters.AddWithValue("@id_Amenidad", idAmenidad);

                        cmdAmenidad.ExecuteNonQuery();
                    }

                    transaccion.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    MessageBox.Show("Error al insertar la habitación y amenidades: " + ex.Message);
                    return false;
                }
            }
        }

        // Obtener todas las habitaciones por hotel
        public static List<Habitacion> ObtenerHabitacionesPorHotel(int id_Hotel)
        {
            List<Habitacion> habitaciones = new List<Habitacion>();
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                SqlCommand comando = new SqlCommand("sp_ObtenerHabitacionesPorHotel", conexion);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@id_Hotel", id_Hotel);

                try
                {
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Habitacion habitacion = new Habitacion
                        {
                            id_Habitacion = reader.GetInt32(reader.GetOrdinal("id_Habitacion")),
                            num_Camas = reader.GetInt32(reader.GetOrdinal("num_Camas")),
                            tipo_Cama = reader.GetChar(reader.GetOrdinal("tipo_Cama")),
                            precio = reader.GetDecimal(reader.GetOrdinal("precio")),
                            capacidad = reader.GetInt32(reader.GetOrdinal("capacidad")),
                            nivel = reader.GetChar(reader.GetOrdinal("nivel")),
                            vista = reader.GetChar(reader.GetOrdinal("vista")),
                            fecha_Registro = reader.GetDateTime(reader.GetOrdinal("fecha_Registro")),
                            fecha_Modifico = reader.GetDateTime(reader.GetOrdinal("fecha_Modifico")),
                            usuario_Registrador = reader.GetInt32(reader.GetOrdinal("usuario_Registrador")),
                            usuario_Modifico = reader.GetInt32(reader.GetOrdinal("usuario_Modifico")),
                            id_Hotel = reader.GetInt32(reader.GetOrdinal("id_Hotel"))
                        };
                        habitaciones.Add(habitacion);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al ejecutar el comando: " + ex.Message);
                }
            }
            return habitaciones;
        }

        public static Habitacion ObtenerHabitacionPorID(int idHabitacion)
        {
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerHabitacionPorID", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_Habitacion", idHabitacion);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Habitacion
                        {
                            id_Habitacion = (int)reader["id_Habitacion"],
                            num_Camas = (int)reader["num_Camas"],
                            tipo_Cama = Convert.ToChar(reader["tipo_Cama"]),
                            precio = Convert.ToDecimal(reader["precio"]),
                            capacidad = (int)reader["capacidad"],
                            nivel = Convert.ToChar(reader["nivel"]),
                            vista = Convert.ToChar(reader["vista"]),
                            fecha_Registro = Convert.ToDateTime(reader["fecha_Registro"]),
                            fecha_Modifico = Convert.ToDateTime(reader["fecha_Modifico"]),
                            usuario_Registrador = (int)reader["usuario_Registrador"],
                            usuario_Modifico = (int)reader["usuario_Modifico"],
                            id_Hotel = (int)reader["id_Hotel"]
                        };
                    }
                }
            }

            return null;
        }


    }
}
