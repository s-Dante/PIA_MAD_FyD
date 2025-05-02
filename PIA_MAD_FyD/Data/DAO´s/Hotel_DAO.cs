using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    }
}
