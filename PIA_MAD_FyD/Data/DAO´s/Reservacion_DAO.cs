﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIA_MAD_FyD.Data.Entidades;
using System.Windows.Forms;
using PIA_MAD_FyD.Helpers.HelpClasses;

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
            catch (SqlException ex) when (ex.Number == 50001)
            {
                throw new Exception("La reservación ya ha sido cancelada previamente.");
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {
                throw new Exception("No se puede cancelar la reservación porque ya se registró un check-in.");
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
        public static List<ServiciosExtra> ObtenerServiciosExtras()
        {
            List<ServiciosExtra> servicios = new List<ServiciosExtra>();
            try
            {
                using (SqlConnection conexion = BD_Connection.ObtenerConexion())
                {
                    SqlCommand comando = new SqlCommand("SELECT * FROM tbl_ServicioExtra", conexion);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ServiciosExtra servicio = new ServiciosExtra
                            {
                                id_ServicioExtrta = (int)reader["id_ServicioExtra"],
                                nombre = reader["nombre"].ToString(),
                                descripcion = reader["descripcion"].ToString(),
                                precion = (decimal)reader["precio"]
                            };
                            servicios.Add(servicio);
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
        public static bool RegistrarServiciosCheckOut(int idCheckOut, List<int> serviciosIds)
        {
            using (SqlConnection connection = BD_Connection.ObtenerConexion())
            {
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    foreach (int servicioId in serviciosIds)
                    {
                        string query = "INSERT INTO tbl_ServicioCheckOut (id_CheckOut, id_ServicioExtra) VALUES (@idCheckOut, @idServicioExtra)";
                        using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@idCheckOut", idCheckOut);
                            cmd.Parameters.AddWithValue("@idServicioExtra", servicioId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public static bool RegistrarDescuentosCheckOut(int idCheckOut, List<int> descuentosIds)
        {
            using (SqlConnection connection = BD_Connection.ObtenerConexion())
            {
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    foreach (int descuentoId in descuentosIds)
                    {
                        string query = "INSERT INTO tbl_DescuentoCheckOut (id_CheckOut, id_Descuento) VALUES (@idCheckOut, @idDescuento)";
                        using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@idCheckOut", idCheckOut);
                            cmd.Parameters.AddWithValue("@idDescuento", descuentoId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
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
            SELECT cant_Huespedes 
            FROM tbl_Reservacion 
            WHERE id_Reservacion = @idReservacion";

                    int numeroPersonas = 0;
                    using (SqlCommand cmd = new SqlCommand(queryGrupo, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idReservacion", idReservacion);
                        object resultado = cmd.ExecuteScalar();
                        numeroPersonas = resultado != DBNull.Value ? Convert.ToInt32(resultado) : 0;
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

                if (EsEstanciaProlongada(idReservacion))
                {
                    Console.WriteLine("Aplicando descuento: Estancia Prolongada");
                    descuentosAutomaticos.Add("Estancia Prolongada");
                }

                if (EsTemporadaBaja())
                {
                    Console.WriteLine("Aplicando descuento: Temporada Baja");
                    descuentosAutomaticos.Add("Temporada Baja");
                }

                if (EsPagoAnticipado(idReservacion))
                {
                    Console.WriteLine("Aplicando descuento: Pago Anticipado");
                    descuentosAutomaticos.Add("Pago Anticipado");
                }

                if (EsDescuentoPorGrupo(idReservacion))
                {
                    Console.WriteLine("Aplicando descuento: Descuento por Grupo");
                    descuentosAutomaticos.Add("Descuento por Grupo");
                }

                Console.WriteLine("Descuentos detectados: " + string.Join(", ", descuentosAutomaticos));

                // Mostrar los descuentos aplicados o un mensaje si no hay descuentos
                if (descuentosAutomaticos.Count == 0)
                {
                    MessageBox.Show("No se aplicaron descuentos.", "Descuentos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string mensajeDescuentos = "Se aplicaron los siguientes descuentos:\n\n";
                    foreach (string descuento in descuentosAutomaticos)
                    {
                        mensajeDescuentos += $"- {descuento}\n";
                    }

                    MessageBox.Show(mensajeDescuentos, "Descuentos Aplicados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Si no hay descuentos, retornamos aquí para evitar consultas innecesarias
                if (descuentosAutomaticos.Count == 0)
                {
                    return false;
                }

                List<int> descuentoIds = ObtenerIdsDescuentos(descuentosAutomaticos);
                if (descuentoIds.Count == 0)
                {
                    Console.WriteLine("No se encontraron IDs para los descuentos.");
                    return false;
                }

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
                MessageBox.Show("Error al registrar los descuentos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    int diasEstancia = resultado != DBNull.Value ? Convert.ToInt32(resultado) : 0;
                    Console.WriteLine($"Días de estancia: {diasEstancia}");

                    return diasEstancia > 7;
                }
            }
        }


        // Función para determinar si es temporada baja (según la fecha actual)
        private static bool EsTemporadaBaja()
        {
            DateTime fechaActual = DateTime.Now;
            bool esTemporadaBaja = fechaActual.Month >= 1 && fechaActual.Month <= 3;
            Console.WriteLine($"Fecha actual: {fechaActual}, ¿Es temporada baja? {esTemporadaBaja}");
            return esTemporadaBaja;
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

                    decimal anticipo = resultado != DBNull.Value ? Convert.ToDecimal(resultado) : 0;
                    Console.WriteLine($"Anticipo pagado: {anticipo}");

                    return anticipo > 0;
                }
            }
        }


        // Función para determinar si el cliente tiene descuento por grupo
        private static bool EsDescuentoPorGrupo(Guid idReservacion)
        {
            using (SqlConnection conexion = BD_Connection.ObtenerConexion())
            {
                string query = @"
            SELECT cant_Huespedes
            FROM tbl_Reservacion
            WHERE id_Reservacion = @idReservacion";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@idReservacion", idReservacion);
                    object resultado = cmd.ExecuteScalar();

                    int cantidadHuespedes = resultado != DBNull.Value ? Convert.ToInt32(resultado) : 0;

                    // Definimos que un grupo es de 4 o más huéspedes
                    bool esGrupo = cantidadHuespedes >= 8;

                    Console.WriteLine($"Cantidad de huéspedes: {cantidadHuespedes}, ¿Es grupo? {esGrupo}");

                    return esGrupo;
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


        //Historial cliente
        public static List<ReservacionHelp> ObtenerHistorialCliente(string rfc, int? año)
        {
            List<ReservacionHelp> historial = new List<ReservacionHelp>();

            string query = @"
        SELECT 
            r.id_Reservacion,
            r.fecha_Ini AS FechaInicio,
            r.fecha_Fin AS FechaFin,
            r.cant_Habitaciones AS CantHabitaciones,
            r.cant_Huespedes AS CantHuespedes,
            r.anticipo_Pagado AS AnticipoPagado,
            r.fecha_Registro AS FechaRegistro,
            c.rfc AS ClienteRFC,
            c.nombre + ' ' + c.apellido_Paterno + ' ' + c.apellido_Materno AS ClienteNombre,
            h.nombre AS HotelNombre,
            u.ciudad AS Ciudad
        FROM tbl_Reservacion r
        INNER JOIN tbl_Cliente c ON r.id_Cliente = c.rfc
        LEFT JOIN tbl_HabitacionReserva hr ON hr.id_Reservacion = r.id_Reservacion
        LEFT JOIN tbl_Habitacion hab ON hr.id_Habitacion = hab.id_Habitacion
        LEFT JOIN tbl_Hotel h ON hab.id_Hotel = h.id_Hotel
        LEFT JOIN tbl_Ubicacion u ON h.ubicacion = u.id_Ubicacion
        WHERE (@RFC IS NULL OR c.rfc = @RFC)
        AND (@Año IS NULL OR YEAR(r.fecha_Ini) = @Año)
        ORDER BY r.fecha_Ini DESC";

            using (SqlConnection conn = BD_Connection.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@RFC", string.IsNullOrEmpty(rfc) ? (object)DBNull.Value : rfc);
                cmd.Parameters.AddWithValue("@Año", año.HasValue ? (object)año.Value : DBNull.Value);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ReservacionHelp reservacion = new ReservacionHelp
                        {
                            idReservacion = Guid.Parse(reader["id_Reservacion"].ToString()),
                            fechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                            fechaFin = Convert.ToDateTime(reader["FechaFin"]),
                            cantHabitaciones = Convert.ToInt32(reader["CantHabitaciones"]),
                            cantHuespedes = Convert.ToInt32(reader["CantHuespedes"]),
                            anticipoPagado = Convert.ToDecimal(reader["AnticipoPagado"]),
                            fechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]),
                            clienteRFC = reader["ClienteRFC"].ToString(),
                            clienteNombre = reader["ClienteNombre"].ToString(),
                            hotelNombre = reader["HotelNombre"].ToString(),
                            ciudad = reader["Ciudad"].ToString()
                        };

                        // Obtención de habitaciones relacionadas
                        reservacion.habitaciones = ObtenerHabitacionesReservadas(reservacion.idReservacion);
                        historial.Add(reservacion);
                    }
                }
            }

            return historial;
        }

        private static List<int> ObtenerHabitacionesReservadas(Guid idReservacion)
        {
            List<int> habitaciones = new List<int>();

            string query = @"
        SELECT id_Habitacion 
        FROM tbl_HabitacionReserva 
        WHERE id_Reservacion = @IdReservacion";

            using (SqlConnection conn = BD_Connection.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdReservacion", idReservacion);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        habitaciones.Add(Convert.ToInt32(reader["id_Habitacion"]));
                    }
                }
            }

            return habitaciones;
        }

    }
}
