using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIA_MAD_FyD.Data.Entidades;
using PIA_MAD_FyD.UserControls.Admin.MainPanels;
using PIA_MAD_FyD.UserControls.Operatives.MainPanels;

namespace PIA_MAD_FyD.Data.DAO_s
{
    class Cliente_DAO
    {

        //Insertar CLiente
        public static errorsTypoCliente InsertarCliente(Cliente cliente)
        {
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                try
                {
                    SqlCommand comando = new SqlCommand("sp_InsertarCliente", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@rfc", cliente.rfc);
                    comando.Parameters.AddWithValue("@nombre", cliente.nombre);
                    comando.Parameters.AddWithValue("@apellido_Paterno", cliente.apellido_Paterno);
                    comando.Parameters.AddWithValue("@apellido_Materno", cliente.apellido_Materno);
                    comando.Parameters.AddWithValue("@correo", cliente.correo);
                    comando.Parameters.AddWithValue("@fecha_Nacimiento", cliente.fecha_Nacimiento);
                    comando.Parameters.AddWithValue("@telefono", cliente.telefono);
                    comando.Parameters.AddWithValue("@estado_Civil", cliente.estado_Civil);
                    comando.Parameters.AddWithValue("@usuario_Registrador", cliente.usuario_Registrador);
                    comando.Parameters.AddWithValue("@usuario_Modifico", cliente.usuario_Modifico);

                    //Ubicacion
                    comando.Parameters.AddWithValue("@pais", cliente.ubicacion_Cliente.pais);
                    comando.Parameters.AddWithValue("@estado", cliente.ubicacion_Cliente.estado);
                    comando.Parameters.AddWithValue("@ciudad", cliente.ubicacion_Cliente.ciudad);
                    comando.Parameters.AddWithValue("@codigo_Postal", cliente.ubicacion_Cliente.codigo_Postal);

                    comando.ExecuteNonQuery();
                    return errorsTypoCliente.Correcto;
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("RFC-Duplicado"))
                        return errorsTypoCliente.rfcDuplicado;
                    else if (ex.Message.Contains("DireccionDuplicada"))
                        return errorsTypoCliente.FechaNacimiento;
                    else
                        throw new Exception("Error SQL desconocido: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error general al insertar el usuario: " + ex.Message);
                }
            }
        }
    }
}
