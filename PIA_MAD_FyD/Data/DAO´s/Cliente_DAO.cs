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


        //Obtener Clientes
        public static List<Cliente> ObtenerClientes()
        {
            List<Cliente> clientes = new List<Cliente>();
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                SqlCommand comando = new SqlCommand("sp_ObtenerClientes", conexion);
                comando.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Cliente cliente = new Cliente
                        {
                            rfc = reader["rfc"].ToString(),
                            nombre = reader["nombre"].ToString(),
                            apellido_Paterno = reader["apellido_Paterno"].ToString(),
                            apellido_Materno = reader["apellido_Materno"].ToString(),
                            correo = reader["correo"].ToString(),
                            fecha_Nacimiento = Convert.ToDateTime(reader["fecha_Nacimiento"]),
                            estado_Civil = Convert.ToChar(reader["estado_Civil"]),
                            fecha_Registro = Convert.ToDateTime(reader["fecha_Registro"]),
                            fecha_Modifico = Convert.ToDateTime(reader["fecha_Modifico"]),
                            usuario_Registrador = Convert.ToInt32(reader["usuario_Registrador"]),
                            usuario_Modifico = Convert.ToInt32(reader["usuario_Modifico"]),
                            telefono = reader["telefono"].ToString(),
                            ubicacion = Convert.ToInt32(reader["id_Ubicacion"]),
                            ubicacion_Cliente = new Ubiacacion
                            {
                                id_Ubicacion = Convert.ToInt32(reader["id_Ubicacion"]),
                                estado = reader["estado"].ToString(),
                                ciudad = reader["ciudad"].ToString(),
                                pais = reader["pais"].ToString(),
                                codigo_Postal = reader["codigo_Postal"].ToString()
                            }
                        };
                        clientes.Add(cliente);
                    }
                }
            }
            return clientes;
        }


        public static errorsTypoCliente ModificarClienteConUbicacion(Cliente cliente)
        {
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                try
                {
                    SqlCommand comando = new SqlCommand("sp_ModificarClienteConUbicacion", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@rfc", cliente.rfc);
                    comando.Parameters.AddWithValue("@nombre", cliente.nombre);
                    comando.Parameters.AddWithValue("@apellido_Paterno", cliente.apellido_Paterno);
                    comando.Parameters.AddWithValue("@apellido_Materno", cliente.apellido_Materno);
                    comando.Parameters.AddWithValue("@correo", cliente.correo);
                    comando.Parameters.AddWithValue("@fecha_Nacimiento", cliente.fecha_Nacimiento);
                    comando.Parameters.AddWithValue("@usuario_Modifico", cliente.usuario_Modifico);
                    comando.Parameters.AddWithValue("@telefono", cliente.telefono);
                    comando.Parameters.AddWithValue("@estado_Civil", cliente.estado_Civil);

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
                        return errorsTypoCliente.OtroError;
                    else
                        throw new Exception("Error SQL desconocido: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error general al insertar el usuario: " + ex.Message);
                }
            }
        }


        //Obtener un cliente por su RFC
        public static Cliente ObtenerClientePorRFC(string rfc)
        {
            Cliente cliente = null;
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                SqlCommand comando = new SqlCommand("sp_ObtenerClientePorRFC", conexion);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@rfc", rfc);
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cliente = new Cliente
                        {
                            rfc = reader["rfc"].ToString(),
                            nombre = reader["nombre"].ToString(),
                            apellido_Paterno = reader["apellido_Paterno"].ToString(),
                            apellido_Materno = reader["apellido_Materno"].ToString(),
                            correo = reader["correo"].ToString(),
                            fecha_Nacimiento = Convert.ToDateTime(reader["fecha_Nacimiento"]),
                            estado_Civil = Convert.ToChar(reader["estado_Civil"]),
                            fecha_Registro = Convert.ToDateTime(reader["fecha_Registro"]),
                            fecha_Modifico = Convert.ToDateTime(reader["fecha_Modifico"]),
                            usuario_Registrador = Convert.ToInt32(reader["usuario_Registrador"]),
                            usuario_Modifico = Convert.ToInt32(reader["usuario_Modifico"]),
                            telefono = reader["telefono"].ToString(),
                            ubicacion = Convert.ToInt32(reader["id_Ubicacion"]),
                            ubicacion_Cliente = new Ubiacacion
                            {
                                id_Ubicacion = Convert.ToInt32(reader["id_Ubicacion"]),
                                estado = reader["estado"].ToString(),
                                ciudad = reader["ciudad"].ToString(),
                                pais = reader["pais"].ToString(),
                                codigo_Postal = reader["codigo_Postal"].ToString()
                            }
                        };
                    }
                }
            }
            return cliente;
        }
    }
}
