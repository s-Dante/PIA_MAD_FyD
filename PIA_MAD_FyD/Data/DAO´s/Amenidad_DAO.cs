using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIA_MAD_FyD.Data.Entidades;
using System.Windows.Forms;

namespace PIA_MAD_FyD.Data.DAO_s
{
    class Amenidad_DAO
    {
        public static List<Amenidad> ObtenerAmenidades()
        {
            List<Amenidad> amenidades = new List<Amenidad>();

            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                try
                {
                    SqlCommand comando = new SqlCommand("sp_ObtenerAmenidades", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Amenidad addAmenidad = new Amenidad
                        {
                            id_Amenidad = (int)reader["id_Amenidad"],
                            amenidad = reader["amenidad"].ToString()
                        };
                        amenidades.Add(addAmenidad);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener las amenidades: " + ex.Message);
                }
            }

            return amenidades;
        }

    }
}
