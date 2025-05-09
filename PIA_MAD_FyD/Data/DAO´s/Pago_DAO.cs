using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
