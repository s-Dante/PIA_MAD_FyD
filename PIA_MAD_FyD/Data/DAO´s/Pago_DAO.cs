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
    class Pago_DAO
    {
        public static int RegistrarPagoReservacion(int idCheckOut, int usuarioRegistrador, char formaPago, decimal total)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_RegistrarPagoReservacion", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@idCheckOut", idCheckOut);
                    comando.Parameters.AddWithValue("@usuarioRegistrador", usuarioRegistrador);
                    comando.Parameters.AddWithValue("@formaPago", formaPago);
                    comando.Parameters.AddWithValue("@total", total);

                    int idPago = Convert.ToInt32(comando.ExecuteScalar());

                    return idPago;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el pago: " + ex.Message);
            }
        }


        //Calcular precio total de la reservacion
        public static decimal CalcularMontoTotalCheckOut(int idCheckOut)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    using (SqlCommand comando = new SqlCommand("sp_CalcularMontoTotalCheckOut", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@idCheckOut", idCheckOut);

                        SqlParameter totalParam = new SqlParameter("@total", SqlDbType.Decimal);
                        totalParam.Direction = ParameterDirection.Output;
                        totalParam.Precision = 12;
                        totalParam.Scale = 2;
                        comando.Parameters.Add(totalParam);

                        comando.ExecuteNonQuery();

                        return (decimal)totalParam.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al calcular el monto total del CheckOut: " + ex.Message);
            }
        }



        //Mostrar datos de pago
        public static decimal ObtenerCostoBase(int idCheckOut)
        {
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                string query = @"
            SELECT SUM(h.precio * DATEDIFF(DAY, r.fecha_Ini, r.fecha_Fin)) AS CostoBase
            FROM tbl_Reservacion r
            INNER JOIN tbl_HabitacionReserva hr ON r.id_Reservacion = hr.id_Reservacion
            INNER JOIN tbl_Habitacion h ON hr.id_Habitacion = h.id_Habitacion
            INNER JOIN tbl_CheckOut co ON r.id_Reservacion = co.id_Reservacion
            WHERE co.id_CheckOut = @idCheckOut";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@idCheckOut", idCheckOut);
                    object resultado = cmd.ExecuteScalar();
                    return resultado != DBNull.Value ? Convert.ToDecimal(resultado) : 0;
                }
            }
        }

        public static List<ServiciosExtra> ObtenerServiciosExtra(int idCheckOut)
        {
            List<ServiciosExtra> servicios = new List<ServiciosExtra>();

            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                string query = @"
            SELECT se.nombre, se.precio 
            FROM tbl_ServicioExtra se
            INNER JOIN tbl_ServicioCheckOut sco ON se.id_ServicioExtra = sco.id_ServicioExtra
            WHERE sco.id_CheckOut = @idCheckOut";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@idCheckOut", idCheckOut);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            servicios.Add(new ServiciosExtra
                            {
                                nombre = reader["nombre"].ToString(),
                                precion = Convert.ToDecimal(reader["precio"])
                            });
                        }
                    }
                }
            }

            return servicios;
        }

        public static List<string> ObtenerDescuentos(int idCheckOut)
        {
            List<string> descuentos = new List<string>();

            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                string query = @"
            SELECT d.nombre 
            FROM tbl_Descuento d
            INNER JOIN tbl_DescuentoCheckOut dco ON d.id_Descuento = dco.id_Descuento
            WHERE dco.id_CheckOut = @idCheckOut";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@idCheckOut", idCheckOut);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            descuentos.Add(reader["nombre"].ToString());
                        }
                    }
                }
            }

            return descuentos;
        }

        public static decimal ObtenerAnticipo(int idCheckOut)
        {
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                string query = @"
            SELECT r.anticipo_Pagado 
            FROM tbl_Reservacion r
            INNER JOIN tbl_CheckOut co ON r.id_Reservacion = co.id_Reservacion
            WHERE co.id_CheckOut = @idCheckOut";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@idCheckOut", idCheckOut);
                    object resultado = cmd.ExecuteScalar();
                    return resultado != DBNull.Value ? Convert.ToDecimal(resultado) : 0;
                }
            }
        }

    }
}
