using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PIA_MAD_FyD.Data.Entidades;

namespace PIA_MAD_FyD.Data.DAO_s
{
    public enum inicioSesion
    {
        ExitosoAdmin = 1,
        ExitosoOperativo = 2,
        FallidoContrasena = 3,
        NoRegistrado = -1
    }

    class Usuario_DAO
    {
        public static inicioSesion InicioSesion(string correo, string contrasena)
        {
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                try
                {
                    SqlCommand comando = new SqlCommand("sp_ValidarInicioSesion", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    // Agregamos parámetros al SP
                    comando.Parameters.AddWithValue("@correo", correo);
                    comando.Parameters.AddWithValue("@pswdIngresada", contrasena);

                    // Configuramos el parámetro de retorno correctamente
                    SqlParameter returnValue = new SqlParameter
                    {
                        ParameterName = "ReturnVal",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.ReturnValue
                    };
                    comando.Parameters.Add(returnValue);

                    // Ejecutamos el SP
                    comando.ExecuteNonQuery();

                    // Capturamos el valor de retorno
                    int resultado = (int)returnValue.Value;
                    return (inicioSesion)resultado;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al validar el inicio de sesión: " + ex.Message);
                }
            }
        }


        //Metodo para insertar un nuevo usuario
        public static errorsTypo InsertarUsuario(Usuario usuario, string pswd)
        {
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                try
                {
                    SqlCommand comando = new SqlCommand("sp_InsertarUsuario", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@num_Nomina", usuario.num_Nomina);
                    comando.Parameters.AddWithValue("@nombre", usuario.nombre);
                    comando.Parameters.AddWithValue("@apellido_Paterno", usuario.apellido_Paterno);
                    comando.Parameters.AddWithValue("@apellido_Materno", usuario.apellido_Materno);
                    comando.Parameters.AddWithValue("@correo", usuario.correo);
                    comando.Parameters.AddWithValue("@fecha_Nacimiento", usuario.fecha_Nacimiento);
                    comando.Parameters.AddWithValue("@telefono", usuario.telefono);
                    comando.Parameters.AddWithValue("@tipoUsuario", usuario.tipo_Usuario);
                    comando.Parameters.AddWithValue("@estatus", usuario.estatus);
                    comando.Parameters.AddWithValue("@usuario_Registrador", usuario.usuario_Registrador);
                    comando.Parameters.AddWithValue("@usuario_Modifico", usuario.usuario_Modifico);
                    comando.Parameters.AddWithValue("@contrasena", pswd);

                    comando.ExecuteNonQuery();
                    return errorsTypo.Correcto;
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("CorreoDuplicado"))
                        return errorsTypo.Correo;
                    else if (ex.Message.Contains("NominaDuplicada"))
                        return errorsTypo.NumeroNomina;
                    else if (ex.Message.Contains("MenorDeEdad"))
                        return errorsTypo.FechaNacimiento;
                    else
                        throw new Exception("Error SQL desconocido: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error general al insertar el usuario: " + ex.Message);
                }
            }
        }


        //Metodo para obtener todos los usuarios
        public static void MostrarUsuariosConContraseñas()
        {
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                SqlCommand comando = new SqlCommand("sp_ObtenerUsuariosConContraseñas", conexion);
                comando.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    Console.WriteLine("=== Usuarios Registrados ===");
                    while (reader.Read())
                    {
                        Console.WriteLine(
                            $"Nómina: {reader["num_Nomina"]}, Nombre: {reader["nombre"]} {reader["apellido_Paterno"]} {reader["apellido_Materno"]}, " +
                            $"Correo: {reader["correo"]}, Tipo: {reader["tipo_Usuario"]}, Estatus: {reader["estatus"]}, " +
                            $"Contraseña (hash): {reader["contraseña_Hash"]}");
                    }
                }
            }
        }

        //Metodo para actualizar la contraseña
        public static void ActualizarContrasena(string correo, string nuevaContrasena)
        {
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                SqlCommand comando = new SqlCommand("sp_ActualizarContrasena", conexion);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@correo", correo);
                comando.Parameters.AddWithValue("@nuevaContrasena", nuevaContrasena);
                comando.ExecuteNonQuery();
            }
        }

        public static void CambiarContraseña(string correo, string nuevaContrasena)
        {
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                try
                {
                    SqlCommand comando = new SqlCommand("sp_CambiarContrasena", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@correo", correo);
                    comando.Parameters.AddWithValue("@nuevaContrasena", nuevaContrasena); // texto plano

                    comando.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error SQL: " + ex.Message);
                }
            }
        }

        //Metodo para saber que usuario es el que se logeo
        public static Usuario ObtenerUsuarioLogeado(string correo)
        {
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                SqlCommand comando = new SqlCommand("sp_UsuarioLogeado", conexion);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@correoIngresado", correo);
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Usuario
                        {
                            num_Nomina = Convert.ToInt32(reader["num_Nomina"]),
                            nombre = reader["nombre"].ToString(),
                            apellido_Paterno = reader["apellido_Paterno"].ToString(),
                            apellido_Materno = reader["apellido_Materno"].ToString(),
                            correo = reader["correo"].ToString(),
                            fecha_Nacimiento = Convert.ToDateTime(reader["fecha_Nacimiento"]),
                            telefono = reader["telefono"].ToString(),
                            tipo_Usuario = Convert.ToChar(reader["tipo_Usuario"]),
                            fecha_Registro = Convert.ToDateTime(reader["fecha_Registro"]),
                            fecha_Modificaion = Convert.ToDateTime(reader["fecha_Modificacion"]),
                            estatus = Convert.ToChar(reader["estatus"]),
                            usuario_Registrador = Convert.ToInt32(reader["usuario_Registrador"]),
                            usuario_Modifico = Convert.ToInt32(reader["usuario_Modifico"])
                        };
                    }
                    else
                    { 
                        return null; // No se encontró el usuario
                    }
                }
            }
        }


    }
}
