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
    class Reservacion_DAO
    {
        //Realizar reservacion
        public static Dictionary<string, object> RealizarReserva(string idCliente, int idHotel, DateTime fechaInicio, DateTime fechaFin, int cantidadHuespedes, decimal anticipo, List<Habitacion> habitaciones, int usuarioRegistrador)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_RealizarReserva", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@idCliente", idCliente);
                    comando.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    comando.Parameters.AddWithValue("@fechaFin", fechaFin);
                    comando.Parameters.AddWithValue("@cantidadHuespedes", cantidadHuespedes);
                    comando.Parameters.AddWithValue("@anticipo", anticipo);
                    comando.Parameters.AddWithValue("@usuarioRegistrador", usuarioRegistrador);

                    string habitacionesCSV = string.Join(",", habitaciones.Select(h => h.id_Habitacion.ToString()));
                    comando.Parameters.AddWithValue("@habitacionesCSV", habitacionesCSV);

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Dictionary<string, object>
                    {
                        { "id_Reservacion", reader["id_Reservacion"].ToString() },
                        { "fecha_Ini", reader["fecha_Ini"] },
                        { "fecha_Fin", reader["fecha_Fin"] },
                        { "cant_Habitaciones", reader["cant_Habitaciones"] },
                        { "cant_Huespedes", reader["cant_Huespedes"] },
                        { "anticipo_Pagado", reader["anticipo_Pagado"] },
                        { "tipos_Habitaciones", reader["tipos_Habitaciones"].ToString() }
                    };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al realizar la reserva: " + ex.Message);
            }

            return null;
        }



        //Cancelar reservacion si no hay checkIN
        public static void CancelarReservacionesSinCheckIn()
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_CancelarReservacionesSinCheckIn", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    // Ejecutar el SP
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Manejar el error (puedes registrar el error o mostrar un mensaje)
                throw new Exception("Error al cancelar reservaciones sin check-in: " + ex.Message);
            }
        }


        //Canselar reservacion activamente
        // Cancelar reservación activamente
        public static void CancelarReservacion(Guid idReservacion, string motivo, int usuarioRegistrador)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_CancelarReservacionActiva", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@idReservacion", idReservacion);
                    comando.Parameters.AddWithValue("@usuarioCancelador", usuarioRegistrador);
                    comando.Parameters.AddWithValue("@motivoCancelacion", motivo);

                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cancelar la reservación: " + ex.Message);
            }
        }


        //Consultar reservacion
        // Consultar reservación
        public static Dictionary<string, object> ConsultarReservacion(Guid idReservacion)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_ConsultarReservacion", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@idReservacion", idReservacion);

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Dictionary<string, object>
                    {
                        { "existe", true },
                        { "id_Reservacion", reader["id_Reservacion"].ToString() },
                        { "fecha_Ini", reader["fecha_Ini"] },
                        { "fecha_Fin", reader["fecha_Fin"] },
                        { "cant_Habitaciones", reader["cant_Habitaciones"] },
                        { "cant_Huespedes", reader["cant_Huespedes"] },
                        { "anticipo_Pagado", reader["anticipo_Pagado"] },
                        { "tipos_Habitaciones", reader["tipos_Habitaciones"].ToString() }
                    };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar la reservación: " + ex.Message);
            }

            return new Dictionary<string, object> { { "existe", false } };
        }


        //Hacer CheckIn
        public static bool RegistrarCheckIn(Guid idReservacion, int usuarioRegistro)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_RegistrarCheckIn", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@idReservacion", idReservacion);
                    comando.Parameters.AddWithValue("@usuarioRegistro", usuarioRegistro);

                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el check-in: " + ex.Message);
            }
        }


        //Hacer CheckOut
        // 1. Consultar si la reservación está activa para el check-out
        public static Dictionary<string, object> ConsultarReservacionCheckOut(Guid idReservacion)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_ConsultarReservacion", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@idReservacion", idReservacion);

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Dictionary<string, object>
                            {
                                { "id_Reservacion", reader["id_Reservacion"].ToString() },
                                { "fecha_Ini", reader["fecha_Ini"] },
                                { "fecha_Fin", reader["fecha_Fin"] },
                                { "cant_Habitaciones", reader["cant_Habitaciones"] },
                                { "cant_Huespedes", reader["cant_Huespedes"] },
                                { "anticipo_Pagado", reader["anticipo_Pagado"] },
                                { "tipos_Habitaciones", reader["tipos_Habitaciones"].ToString() }
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar la reservación para el check-out: " + ex.Message);
            }
            return null;
        }

        // 2. Realizar el check-out
        public static int RealizarCheckOut(Guid idReservacion, int usuarioRegistrador)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_RegistrarCheckOut", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@idReservacion", idReservacion);
                    comando.Parameters.AddWithValue("@usuarioRegistrador", usuarioRegistrador);

                    return Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al realizar el check-out: " + ex.Message);
            }
        }

        // 3. Obtener servicios extra disponibles
        public static List<string> ObtenerServiciosExtras()
        {
            List<string> servicios = new List<string>();
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("SELECT nombre FROM tbl_ServicioExtra", conexion);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            servicios.Add(reader["nombre"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los servicios extra: " + ex.Message);
            }
            return servicios;
        }

        public static List<int> ObtenerIdsServiciosExtras(List<string> nombresServicios)
        {
            List<int> idsServicios = new List<int>();

            if (nombresServicios.Count == 0)
                return idsServicios;

            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    // Crear una lista de parámetros dinámicos
                    string parametros = string.Join(",", nombresServicios.Select((_, index) => $"@nombre{index}"));
                    string query = $"SELECT id_ServicioExtra FROM tbl_ServicioExtra WHERE nombre IN ({parametros})";

                    SqlCommand comando = new SqlCommand(query, conexion);

                    // Asignar los parámetros
                    for (int i = 0; i < nombresServicios.Count; i++)
                    {
                        comando.Parameters.AddWithValue($"@nombre{i}", nombresServicios[i]);
                    }

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idsServicios.Add(Convert.ToInt32(reader["id_ServicioExtra"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los IDs de los servicios extra: " + ex.Message);
            }

            return idsServicios;
        }


        // 4. Registrar servicios seleccionados en el check-out
        public static void RegistrarServiciosCheckOut(int idCheckOut, Guid idReservacion, CheckedListBox listbox)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    foreach (var item in listbox.CheckedItems)
                    {
                        var servicio = item as ServiciosExtra;  // Asegúrate de que este sea el tipo correcto
                        if (servicio != null)
                        {
                            string query = @"
                    INSERT INTO tbl_ServicioCheckOut (id_CheckOut, id_ServicioExtra)
                    VALUES (@idCheckOut, @idServicioExtra)";

                            using (SqlCommand cmd = new SqlCommand(query, conexion))
                            {
                                cmd.Parameters.AddWithValue("@idCheckOut", idCheckOut);
                                cmd.Parameters.AddWithValue("@idServicioExtra", servicio.id_ServicioExtrta);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar servicios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // 5. Registrar el pago
        public static void RegistrarPago(int idCheckOut, char formaPago, decimal total, int usuarioRegistrador)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_RegistrarPagoReservacion", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@idCheckOut", idCheckOut);
                    comando.Parameters.AddWithValue("@formaPago", formaPago);
                    comando.Parameters.AddWithValue("@total", total);
                    comando.Parameters.AddWithValue("@usuarioRegistrador", usuarioRegistrador);
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el pago: " + ex.Message);
            }
        }

        // 6. Generar factura
        public static void GenerarFactura(int idCheckOut, string rfcReceptor, string nombreReceptor, string usoCFDI,
                                          string metodoPago, string formaPago, string lugarEmision, int usuarioRegistrador)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("sp_GenerarFacturaReservacion", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@idCheckOut", idCheckOut);
                    comando.Parameters.AddWithValue("@rfcReceptor", rfcReceptor);
                    comando.Parameters.AddWithValue("@nombreReceptor", nombreReceptor);
                    comando.Parameters.AddWithValue("@usoCFDI", usoCFDI);
                    comando.Parameters.AddWithValue("@metodoPago", metodoPago);
                    comando.Parameters.AddWithValue("@formaPago", formaPago);
                    comando.Parameters.AddWithValue("@lugarEmision", lugarEmision);
                    comando.Parameters.AddWithValue("@usuarioRegistrador", usuarioRegistrador);
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar la factura: " + ex.Message);
            }
        }

        // 7. Obtener ID del CheckOut por Reservación
        public static int ObtenerIdCheckOutPorReservacion(Guid idReservacion)
        {
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    using (SqlCommand comando = new SqlCommand("sp_ObtenerCheckOutPorReservacion", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@idReservacion", idReservacion);

                        object resultado = comando.ExecuteScalar();

                        if (resultado != null && int.TryParse(resultado.ToString(), out int idCheckOut))
                        {
                            return idCheckOut;
                        }
                        else
                        {
                            throw new Exception("No se encontró un CheckOut asociado a la reservación.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el ID del CheckOut: " + ex.Message);
            }
        }



        //Calcular pago
        public static decimal CalcularMontoTotal(Guid idReservacion, int idCheckOut)
        {
            decimal montoTotal = 0;

            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    // 1. Cálculo del costo base (noches * precio de cada habitación)
                    string queryCostoBase = @"
            SELECT 
                SUM(h.precio * DATEDIFF(DAY, r.fecha_Ini, r.fecha_Fin)) AS costoBase
            FROM tbl_Reservacion r
            INNER JOIN tbl_HabitacionReserva hr ON r.id_Reservacion = hr.id_Reservacion
            INNER JOIN tbl_Habitacion h ON hr.id_Habitacion = h.id_Habitacion
            WHERE r.id_Reservacion = @idReservacion";

                    using (SqlCommand cmd = new SqlCommand(queryCostoBase, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idReservacion", idReservacion);
                        object resultado = cmd.ExecuteScalar();
                        montoTotal += resultado != DBNull.Value ? Convert.ToDecimal(resultado) : 0;
                    }

                    // 2. Sumar servicios extra seleccionados
                    string queryServicios = @"
            SELECT SUM(se.precio) 
            FROM tbl_ServicioCheckOut sc
            INNER JOIN tbl_ServicioExtra se ON sc.id_ServicioExtra = se.id_ServicioExtra
            WHERE sc.id_CheckOut = @idCheckOut";

                    using (SqlCommand cmd = new SqlCommand(queryServicios, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idCheckOut", idCheckOut);
                        object resultado = cmd.ExecuteScalar();
                        montoTotal += resultado != DBNull.Value ? Convert.ToDecimal(resultado) : 0;
                    }

                    // 3. Aplicar descuentos automáticos
                    // Obtener los datos necesarios para aplicar los descuentos
                    string queryReservacion = @"
            SELECT r.fecha_Ini, r.fecha_Fin, r.anticipo_Pagado 
            FROM tbl_Reservacion r 
            WHERE r.id_Reservacion = @idReservacion";

                    DateTime fechaInicio = DateTime.MinValue, fechaFin = DateTime.MinValue;
                    decimal anticipoPagado = 0;

                    using (SqlCommand cmd = new SqlCommand(queryReservacion, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idReservacion", idReservacion);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                fechaInicio = reader.GetDateTime(0);
                                fechaFin = reader.GetDateTime(1);
                                anticipoPagado = reader.GetDecimal(2);
                            }
                        }
                    }

                    // 3.1. Descuento por estancia prolongada (10% si son 7 noches o más)
                    int nochesEstancia = (fechaFin - fechaInicio).Days;
                    if (nochesEstancia >= 7)
                    {
                        montoTotal -= (0.10m * montoTotal);  // Descuento del 10%
                    }

                    // 3.2. Descuento por temporada baja (15% durante temporada baja, ejemplo mayo-sep)
                    if (fechaInicio.Month >= 5 && fechaInicio.Month <= 9)
                    {
                        montoTotal -= (0.15m * montoTotal);  // Descuento del 15%
                    }

                    // 3.3. Descuento por pago anticipado (15% si se pagó anticipo)
                    if (anticipoPagado > 0)
                    {
                        montoTotal -= (0.15m * montoTotal);  // Descuento del 15%
                    }

                    // 3.4. Descuento por grupo (10% si más de 8 personas)
                    string queryGrupo = @"
            SELECT COUNT(*) 
            FROM tbl_PersonasReserva pr
            WHERE pr.id_Reservacion = @idReservacion";

                    int numeroPersonas = 0;
                    using (SqlCommand cmd = new SqlCommand(queryGrupo, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idReservacion", idReservacion);
                        numeroPersonas = (int)cmd.ExecuteScalar();
                    }

                    if (numeroPersonas > 8)
                    {
                        montoTotal -= (0.10m * montoTotal);  // Descuento del 10%
                    }

                    // 4. Restar anticipo pagado
                    montoTotal -= anticipoPagado; // Ya descontamos el anticipo antes

                    // 5. Aplicar descuentos en porcentaje ya seleccionados por el usuario
                    string queryDescuentos = @"
            SELECT d.porcentaje 
            FROM tbl_DescuentoCheckOut dc
            INNER JOIN tbl_Descuento d ON dc.id_Descuento = d.id_Descuento
            WHERE dc.id_CheckOut = @idCheckOut";

                    using (SqlCommand cmd = new SqlCommand(queryDescuentos, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idCheckOut", idCheckOut);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                decimal porcentajeDescuento = reader.GetDecimal(0);
                                montoTotal -= (montoTotal * (porcentajeDescuento / 100));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al calcular el monto total: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                montoTotal = -1; // Indica error
            }

            return montoTotal;
        }


        //Descuentos:
        public static bool RegistrarDescuentosCheckOut(int idCheckOut, Guid idReservacion)
        {
            bool exito = true;

            try
            {
                List<string> descuentosAutomaticos = new List<string>();

                // 1. Estancia Prolongada (Ejemplo: si la estancia es mayor a 7 noches)
                if (EsEstanciaProlongada(idReservacion))
                {
                    Console.WriteLine("Descuento de Estancia Prolongada aplicado.");
                    descuentosAutomaticos.Add("Estancia Prolongada");
                }

                // 2. Temporada Baja (Ejemplo: si la fecha actual está en temporada baja)
                if (EsTemporadaBaja())
                {
                    Console.WriteLine("Descuento de Temporada Baja aplicado.");
                    descuentosAutomaticos.Add("Temporada Baja");
                }

                // 3. Pago Anticipado (Ejemplo: si el cliente pagó por adelantado)
                if (EsPagoAnticipado(idReservacion))
                {
                    Console.WriteLine("Descuento de Pago Anticipado aplicado.");
                    descuentosAutomaticos.Add("Pago Anticipado");
                }

                // 4. Descuento por Grupo (Ejemplo: si el cliente está en un grupo)
                if (EsDescuentoPorGrupo(idReservacion))
                {
                    Console.WriteLine("Descuento de Descuento por Grupo aplicado.");
                    descuentosAutomaticos.Add("Descuento por Grupo");
                }

                // Verificar si la lista está vacía antes de proceder
                if (descuentosAutomaticos.Count == 0)
                {
                    throw new Exception("No se aplican descuentos automáticos.");
                }

                // Obtener los IDs de los descuentos automáticamente aplicados
                List<int> descuentoIds = Reservacion_DAO.ObtenerIdsDescuentos(descuentosAutomaticos);

                if (descuentoIds.Count == 0)
                {
                    throw new Exception("No se pudieron obtener los IDs de los descuentos.");
                }

                Console.WriteLine("Descuentos a aplicar: " + string.Join(", ", descuentoIds));

                // Registrar los descuentos en la base de datos
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    foreach (int idDescuento in descuentoIds)
                    {
                        string query = "INSERT INTO tbl_DescuentoCheckOut (id_CheckOut, id_Descuento) VALUES (@idCheckOut, @idDescuento)";
                        using (SqlCommand cmd = new SqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@idCheckOut", idCheckOut);
                            cmd.Parameters.AddWithValue("@idDescuento", idDescuento);

                            int filasAfectadas = cmd.ExecuteNonQuery();

                            if (filasAfectadas == 0)
                            {
                                exito = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exito = false;
                Console.WriteLine("Error: " + ex.Message);
            }

            return exito;
        }


        // Función para determinar si la estancia es prolongada (mayor a 7 noches)
        private static bool EsEstanciaProlongada(Guid idReservacion)
        {
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                string query = @"
        SELECT DATEDIFF(DAY, fecha_Ini, fecha_Fin) 
        FROM tbl_Reservacion
        WHERE id_Reservacion = @idReservacion";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@idReservacion", idReservacion);
                    object resultado = cmd.ExecuteScalar();
                    return resultado != DBNull.Value && Convert.ToInt32(resultado) > 7;
                }
            }
        }

        // Función para determinar si es temporada baja (según la fecha actual)
        private static bool EsTemporadaBaja()
        {
            DateTime fechaActual = DateTime.Now;
            // Por ejemplo, si la temporada baja es entre enero y marzo
            return fechaActual.Month >= 1 && fechaActual.Month <= 3;
        }

        // Función para determinar si el cliente ha hecho un pago anticipado
        private static bool EsPagoAnticipado(Guid idReservacion)
        {
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                string query = @"
        SELECT anticipo_Pagado
        FROM tbl_Reservacion
        WHERE id_Reservacion = @idReservacion";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@idReservacion", idReservacion);
                    object resultado = cmd.ExecuteScalar();
                    return resultado != DBNull.Value && Convert.ToDecimal(resultado) > 0;
                }
            }
        }

        // Función para determinar si el cliente tiene descuento por grupo
        private static bool EsDescuentoPorGrupo(Guid idReservacion)
        {
            // Suponemos que hay un campo en la reservación que indica si es un grupo
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                string query = @"
        SELECT es_Grupo
        FROM tbl_Reservacion
        WHERE id_Reservacion = @idReservacion";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@idReservacion", idReservacion);
                    object resultado = cmd.ExecuteScalar();
                    return resultado != DBNull.Value && Convert.ToBoolean(resultado);
                }
            }
        }


        public static List<int> ObtenerIdsDescuentos(List<string> descuentosNombres)
        {
            List<int> descuentosIds = new List<int>();

            try
            {
                // Verificar si la lista de nombres de descuentos está vacía
                if (descuentosNombres.Count == 0)
                {
                    throw new Exception("La lista de descuentos no puede estar vacía.");
                }

                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    // Crear la consulta SQL con el formato correcto
                    string query = "SELECT d.id_Descuento FROM tbl_Descuento d WHERE d.nombre IN (";

                    // Agregar parámetros dinámicos para cada descuento
                    var parametros = new List<SqlParameter>();
                    for (int i = 0; i < descuentosNombres.Count; i++)
                    {
                        query += "@descuento" + i;
                        if (i < descuentosNombres.Count - 1)
                        {
                            query += ", ";
                        }

                        parametros.Add(new SqlParameter("@descuento" + i, descuentosNombres[i]));
                    }
                    query += ")"; // Cerrar la lista IN

                    // Crear el comando SQL
                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        // Agregar los parámetros al comando
                        cmd.Parameters.AddRange(parametros.ToArray());

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Leer cada ID de descuento y agregarlo a la lista
                                int idDescuento = reader.GetInt32(0);
                                descuentosIds.Add(idDescuento);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los IDs de los descuentos: " + ex.Message);
            }

            return descuentosIds;
        }



    }
}
