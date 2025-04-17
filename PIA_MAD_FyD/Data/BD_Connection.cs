using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PIA_MAD_FyD.Data
{
    class BD_Connection
    {
        public static SqlConnection ObtenerConexion()
        {
            // Cadena de conexión directamente en el código
            string connectionString = "Server=OMAR-PCS\\SQLEXPRESS;Database=PIA_MAD;Integrated Security=True;MultipleActiveResultSets=True;Encrypt=False;";

            SqlConnection conexion = new SqlConnection(connectionString);

            try
            {
                // Abrimos la conexión
                conexion.Open();
                Console.WriteLine("Conexión exitosa.");
                return conexion;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al conectar a la base de datos: {ex.Message}");
                return null; // Regresa null si hay algún error
            }
        }
    }
}
