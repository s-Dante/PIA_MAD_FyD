using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PIA_MAD_FyD.Data.Entidades;
using PIA_MAD_FyD.UserControls.Admin.MainPanels;

namespace PIA_MAD_FyD.Data.DAO_s
{
    class Hotel_DAO
    {
        //Metodo para insertar un hotel
        public static errorsTypoHotel InsertarHotel(Hotel hotel, Ubiacacion ubicacion)
        {
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                try
                {
                    SqlCommand comando = new SqlCommand("sp_InsertarHotel", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@nombre", hotel.nombre);
                    comando.Parameters.AddWithValue("@calle", hotel.calle);
                    comando.Parameters.AddWithValue("@numero", hotel.numero);
                    comando.Parameters.AddWithValue("@colonia", hotel.colonia);
                    comando.Parameters.AddWithValue("@num_Pisos", hotel.num_Pisos);
                    comando.Parameters.AddWithValue("@usuario_Registrador", hotel.usuario_Registrador);
                    comando.Parameters.AddWithValue("@usuario_Modifico", hotel.usuario_Modifico);
                    comando.Parameters.AddWithValue("@rfc", hotel.rfc);

                    //Ubicacion
                    comando.Parameters.AddWithValue("@pais", ubicacion.pais);
                    comando.Parameters.AddWithValue("@estado", ubicacion.estado);
                    comando.Parameters.AddWithValue("@ciudad", ubicacion.ciudad);
                    comando.Parameters.AddWithValue("@codigo_Postal", ubicacion.codigo_Postal);

                    comando.ExecuteNonQuery();
                    return errorsTypoHotel.Correcto;
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("RFC-Duplicado"))
                        return errorsTypoHotel.rfcDuplicado;
                    else if (ex.Message.Contains("DireccionDuplicada"))
                        return errorsTypoHotel.NumeroRepetido;
                    else
                        throw new Exception("Error SQL desconocido: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error general al insertar el usuario: " + ex.Message);
                }
            }
        }


        //Metodo para modificar la informacion de un hotel
        public static errorsTypoHotel ModificarHotelConUbicacion(Hotel hotel)
        {
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                try
                {
                    SqlCommand comando = new SqlCommand("sp_ModificarHotelConUbicacion", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@id_Hotel", hotel.id_Hotel);
                    comando.Parameters.AddWithValue("@nombre", hotel.nombre);
                    comando.Parameters.AddWithValue("@calle", hotel.calle);
                    comando.Parameters.AddWithValue("@numero", hotel.numero);
                    comando.Parameters.AddWithValue("@colonia", hotel.colonia);
                    comando.Parameters.AddWithValue("@num_Pisos", hotel.num_Pisos);
                    comando.Parameters.AddWithValue("@usuario_Modifico", hotel.usuario_Modifico);
                    comando.Parameters.AddWithValue("@rfc", hotel.rfc);
                    //Ubicacion
                    comando.Parameters.AddWithValue("@pais", hotel.ubicacionHotel.pais);
                    comando.Parameters.AddWithValue("@estado", hotel.ubicacionHotel.estado);
                    comando.Parameters.AddWithValue("@ciudad", hotel.ubicacionHotel.ciudad);
                    comando.Parameters.AddWithValue("@codigo_Postal", hotel.ubicacionHotel.codigo_Postal);
                    comando.ExecuteNonQuery();
                    return errorsTypoHotel.Correcto;
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("RFC-Duplicado"))
                        return errorsTypoHotel.rfcDuplicado;
                    else if (ex.Message.Contains("DireccionDuplicada"))
                        return errorsTypoHotel.NumeroRepetido;
                    else
                        throw new Exception("Error SQL desconocido: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error general al insertar el usuario: " + ex.Message);
                }
            }
        }


        //Metodo para obtener todos los hoteles
        public static List<Hotel> ObtenerHotelesConUbicacion()
        {
            List<Hotel> hoteles = new List<Hotel>();
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                SqlCommand comando = new SqlCommand("sp_ObtenerHotelesConUbicacion", conexion);
                comando.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    var hotel = new Hotel
                    {
                        id_Hotel = reader.GetInt32(0),
                        nombre = reader.GetString(1),
                        calle = reader.GetString(2),
                        numero = reader.GetString(3),
                        colonia = reader.GetString(4),
                        num_Pisos = reader.GetInt32(5),
                        rfc = reader.GetString(6),
                        ubicacion = reader.GetInt32(7),
                        ubicacionHotel = new Ubiacacion
                        {
                            id_Ubicacion = reader.GetInt32(7),
                            pais = reader.GetString(8),
                            estado = reader.GetString(9),
                            ciudad = reader.GetString(10),
                            codigo_Postal = reader.GetString(11)
                        }
                    };

                    hoteles.Add(hotel);
                }
            }

            return hoteles;
        }


        //hoteles con habitaicon
        public static List<Hotel> ObtenerHotelesConHabitaciones()
        {
            var hoteles = new Dictionary<int, Hotel>();

            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerHotelesConHabitaciones", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idHotel = reader.GetInt32(reader.GetOrdinal("id_Hotel"));

                        if (!hoteles.ContainsKey(idHotel))
                        {
                            Hotel hotel = new Hotel
                            {
                                id_Hotel = idHotel,
                                nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                calle = reader.GetString(reader.GetOrdinal("calle")),
                                numero = reader.GetString(reader.GetOrdinal("numero")),
                                colonia = reader.GetString(reader.GetOrdinal("colonia")),
                                num_Pisos = reader.GetInt32(reader.GetOrdinal("num_Pisos")),
                                rfc = reader.IsDBNull(reader.GetOrdinal("rfc")) ? null : reader.GetString(reader.GetOrdinal("rfc")),
                                ubicacionHotel = new Ubiacacion
                                {
                                    id_Ubicacion = reader.GetInt32(reader.GetOrdinal("id_Ubicacion")),
                                    pais = reader.GetString(reader.GetOrdinal("pais")),
                                    estado = reader.GetString(reader.GetOrdinal("estado")),
                                    ciudad = reader.GetString(reader.GetOrdinal("ciudad")),
                                    codigo_Postal = reader.GetString(reader.GetOrdinal("codigo_Postal"))
                                },
                                Habitaciones = new List<Habitacion>()
                            };
                            hoteles[idHotel] = hotel;
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("id_Habitacion")))
                        {
                            Habitacion hab = new Habitacion
                            {
                                id_Habitacion = reader.GetInt32(reader.GetOrdinal("id_Habitacion")),
                                num_Camas = reader.GetInt32(reader.GetOrdinal("num_Camas")),
                                tipo_Cama = reader.GetString(reader.GetOrdinal("tipo_Cama"))[0],
                                precio = reader.GetDecimal(reader.GetOrdinal("precio")),
                                capacidad = reader.GetInt32(reader.GetOrdinal("capacidad")),
                                nivel = reader.GetString(reader.GetOrdinal("nivel"))[0],
                                vista = reader.GetString(reader.GetOrdinal("vista"))[0],
                                id_Hotel = idHotel
                            };

                            hoteles[idHotel].Habitaciones.Add(hab);
                        }
                    }
                }
            }

            return hoteles.Values.ToList();
        }


        //Obtener las amenidades de cada habitacion
        public static List<Amenidad> ObtenerAmenidadesPorHabitacion(int idHabitacion)
        {
            List<Amenidad> lista = new List<Amenidad>();
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                SqlCommand comando = new SqlCommand("sp_ObtenerAmenidadesPorHabitacion", conexion);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@id_Habitacion", idHabitacion);

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Amenidad a = new Amenidad();
                        a.id_Amenidad = reader.GetInt32(0);
                        a.amenidad = reader.GetString(1);
                        lista.Add(a);
                    }
                }
            }
            return lista;
        }

        //Modificar habitaicon
        public static bool ActualizarHabitacion(Habitacion h, Usuario usuarioLogeado)
        {
            using (var conexion = BD_Connection.ObtenerConexion())
            {
                try
                {
                    SqlCommand comando = new SqlCommand("sp_ActualizarHabitacion", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@num_Camas", h.num_Camas);
                    comando.Parameters.AddWithValue("@capacidad", h.capacidad);
                    comando.Parameters.AddWithValue("@tipo_Cama", h.tipo_Cama);
                    comando.Parameters.AddWithValue("@precio", h.precio);
                    comando.Parameters.AddWithValue("@nivel", h.nivel);
                    comando.Parameters.AddWithValue("@vista", h.vista);
                    comando.Parameters.AddWithValue("@id_Habitacion", h.id_Habitacion);
                    comando.Parameters.AddWithValue("@usuario_Modifico", usuarioLogeado.num_Nomina);

                    int filasAfectadas = comando.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar la habitación: " + ex.Message);
                    return false;
                }
            }
        }


        public static void ActualizarAmenidadesDeHabitacion(int idHabitacion, List<Amenidad> nuevasAmenidades)
        {
            using (var conexion = BD_Connection.ObtenerConexion())
            {
                try
                {
                    // Eliminar amenidades actuales
                    var deleteCmd = new SqlCommand("sp_ActualizarAmenidadesPorHabitacion", conexion);
                    deleteCmd.CommandType = CommandType.StoredProcedure;
                    deleteCmd.Parameters.AddWithValue("@id_Habitacion", idHabitacion);
                    deleteCmd.ExecuteNonQuery();

                    // Insertar nuevas
                    foreach (var amenidad in nuevasAmenidades)
                    {
                        var insertCmd = new SqlCommand("sp_InsertarAmenidadHabitacion", conexion);
                        insertCmd.CommandType = CommandType.StoredProcedure;
                        insertCmd.Parameters.AddWithValue("@id_Habitacion", idHabitacion);
                        insertCmd.Parameters.AddWithValue("@id_Amenidad", amenidad.id_Amenidad);
                        insertCmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al actualizar amenidades: {ex.Message}");
                }
            }
        }



    }
}
