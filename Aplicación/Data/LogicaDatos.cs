using Microsoft.Identity.Client;
using System.Windows;
using System.Data.Sql;
using Microsoft.Data.SqlClient;
using Data.BD_Tables;
using System;
using System.Data;

namespace Data
{
    public class LogicaDatos
    {
        Connection con = new Connection();
        public bool existeUser(string usuario)
        {
            using (SqlConnection connection = con.Conexion())
            {
                SqlCommand command = new SqlCommand("SELECT count(*) FROM Empleado WHERE Usuario = @usuario", connection);
                command.Parameters.AddWithValue("@usuario", usuario); // Usar parámetros evita inyecciones SQL

                try
                {
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
                catch (Exception ex)
                {

                    return false;
                }
            }
        }

        public bool PasswordIsCorrecta(string usuario, string password)
        {

            using (SqlConnection connection = con.Conexion())
            {
                SqlCommand command = new SqlCommand("SELECT password FROM Empleado WHERE Usuario = @usuario", connection);
                command.Parameters.AddWithValue("@usuario", usuario); // Usar parámetros evita inyecciones SQL

                try
                {
                    connection.Open();
                    string passwordReal = Convert.ToString(command.ExecuteScalar());

                    return password.Equals(passwordReal);


                }
                catch (Exception ex)
                {

                    return false;
                }
            }

        }

        public string DeterminarRol(string usuario, string password)
        {

            using (SqlConnection connection = con.Conexion())
            {
                SqlCommand command = new SqlCommand("SELECT Tipo FROM Empleado WHERE Usuario = @usuario", connection);
                command.Parameters.AddWithValue("@usuario", usuario); // Usar parámetros evita inyecciones SQL

                try
                {
                    connection.Open();
                    string puesto = Convert.ToString(command.ExecuteScalar());

                    return puesto;


                }
                catch (Exception ex)
                {

                    return null;
                }
            }
        }


        public async Task InsertarMantenimientoAsync(DateTime FechaInicio, int duracion, string estado, string observaciones, double costoTot, bool pagado, List<string> servicios, string NombreEmp, string ApellidoEmp, Dictionary<string, int> repuestos, Vehiculo vehiculo)
        {
            DateTime fechaFin = FechaInicio.Date.AddDays(duracion);

            using (SqlConnection connection = con.Conexion())
            {
                await connection.OpenAsync(); // Abre la conexión solo una vez

                SqlTransaction transaction = connection.BeginTransaction(); // Inicia una transacción

                try
                {
                    // Obtener el ID del vehículo o insertarlo si no existe
                    int? idVehiculo = await ObtenerIdVehiculoPorPlacaAsync(vehiculo.Placa, connection, transaction);

                    if (idVehiculo == null)
                    {
                        await InsertarVehiculoAsync(vehiculo, connection, transaction);
                        idVehiculo = await ObtenerIdVehiculoPorPlacaAsync(vehiculo.Placa, connection, transaction);

                        if (idVehiculo == null)
                        {
                            throw new Exception("No se pudo insertar ni recuperar el ID del vehículo.");
                        }
                    }

                    // Insertar en la tabla Mantenimiento
                    SqlCommand command = new SqlCommand("INSERT INTO Mantenimiento (Id_Vehiculo, Fecha_Inicio, Fecha_Fin, Estado, Observaciones, Costo_Total, Pagado)  OUTPUT INSERTED.Id_Mantenimiento VALUES (@idVehiculo, @fechainicio, @fechafin, @estado, @observaciones, @costoT, @isPagado)", connection, transaction);

                    command.Parameters.AddWithValue("@idVehiculo", idVehiculo);
                    command.Parameters.AddWithValue("@fechainicio", FechaInicio);
                    command.Parameters.AddWithValue("@fechafin", fechaFin);
                    command.Parameters.AddWithValue("@estado", estado);
                    command.Parameters.AddWithValue("@observaciones", observaciones);
                    command.Parameters.AddWithValue("@costoT", costoTot);
                    command.Parameters.AddWithValue("@isPagado", pagado);


                    // Obtener el ID del mantenimiento insertado
                    int idMantenimiento = 0;
                    try
                    {
                        object result = await command.ExecuteScalarAsync();
                        if (result != null)
                        {
                            idMantenimiento = Convert.ToInt32(result);
                        }
                        else
                        {
                            throw new Exception("No se pudo obtener el ID del mantenimiento.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al insertar mantenimiento: " + ex.Message);
                        throw;
                    }


                    // Insertar detalles de repuestos
                    await InsertarDetalleRepuestosAsync(repuestos, idMantenimiento, connection, transaction);

                    // Insertar detalles de servicios
                    foreach (string service in servicios)
                    {
                        await InsertarDetalleMantenimientAsync(service, idMantenimiento, NombreEmp, ApellidoEmp, connection, transaction);
                    }

                    transaction.Commit(); // Confirma la transacción
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Revierte la transacción en caso de error
                    Console.WriteLine("Error al insertar el mantenimiento: " + ex.Message);
                    throw;
                }
            }
        }


        public async Task InsertarDetalleMantenimientAsync(string servicio, int idMantenimiento, string nombreEmp, string apellidoEmp, SqlConnection connection, SqlTransaction transaction)
        {
            double costoService = 0;
            int idServicio = 0;

            // Recuperar el costo y el ID del servicio
            using (SqlCommand command = new SqlCommand("SELECT Id_Servicio, Costo FROM Servicio WHERE Nombre = @NombreService", connection, transaction))
            {
                command.Parameters.AddWithValue("@NombreService", servicio);

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idServicio = reader.GetInt32(reader.GetOrdinal("Id_Servicio"));
                            costoService = Convert.ToDouble(reader.GetDecimal(reader.GetOrdinal("Costo")));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al recuperar el servicio '{servicio}': {ex.Message}");
                    return; // No continúes si no se pudo obtener el servicio
                }
            }

            if (idServicio > 0)
            {
                // Insertar datos en Detalle Mantenimiento
                using (SqlCommand command = new SqlCommand(
                    "INSERT INTO [Detalle Mantenimiento] (Id_Mantenimiento, Id_Servicio, Id_Empleado, Costo) VALUES (@IdMant, @IdServicio, @IdEmpleado, @Costo)", connection, transaction))
                {
                    command.Parameters.AddWithValue("@IdMant", idMantenimiento);
                    command.Parameters.AddWithValue("@IdServicio", idServicio);
                    command.Parameters.AddWithValue("@IdEmpleado", DeterminarIDEmpleado(nombreEmp, apellidoEmp));
                    command.Parameters.AddWithValue("@Costo", costoService);

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al insertar detalle de mantenimiento: {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"No se pudo insertar detalle de mantenimiento. Servicio '{servicio}' no encontrado.");
            }
        }


        public async Task InsertarDetalleRepuestosAsync(Dictionary<string, int> repuestos, int idMantenimiento, SqlConnection connection, SqlTransaction transaction)
        {
            foreach (KeyValuePair<string, int> repuesto in repuestos)
            {
                int idRepuesto = 0;
                double costoRepuesto = 0;

                // Recuperar el costo y el ID del repuesto
                using (SqlCommand command = new SqlCommand("SELECT Id_Repuesto, Precio_Unitario FROM Repuesto WHERE Nombre = @NombreRep", connection, transaction))
                {
                    command.Parameters.AddWithValue("@NombreRep", repuesto.Key);

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                idRepuesto = reader.GetInt32(reader.GetOrdinal("Id_Repuesto"));
                                costoRepuesto = Convert.ToDouble(reader.GetDecimal(reader.GetOrdinal("Precio_Unitario")));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al recuperar repuesto '{repuesto.Key}': {ex.Message}");
                        continue; // Salta al siguiente repuesto si ocurre un error
                    }
                }

                if (idRepuesto > 0 && costoRepuesto > 0)
                {
                    // Insertar datos en Detalle Repuesto
                    using (SqlCommand command = new SqlCommand(
                        "INSERT INTO [Detalle Repuesto] (Id_Mantenimiento, Id_Repuesto, Costo_Unitario, Cantidad, Subtotal) VALUES (@IdMantenimiento, @IdRepuesto, @CostoRepuesto, @CantidadRep, @Subtot)", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@IdMantenimiento", idMantenimiento);
                        command.Parameters.AddWithValue("@IdRepuesto", idRepuesto);
                        command.Parameters.AddWithValue("@CostoRepuesto", costoRepuesto);
                        command.Parameters.AddWithValue("@CantidadRep", repuesto.Value);
                        command.Parameters.AddWithValue("@Subtot", costoRepuesto * repuesto.Value);

                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error al insertar detalle del repuesto '{repuesto.Key}': {ex.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"No se pudo insertar el detalle del repuesto '{repuesto.Key}'. ID o costo inválido.");
                }
            }
        }

        public async Task InsertarVehiculoAsync(Vehiculo vehiculo, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand command = new SqlCommand("INSERT INTO Vehiculo (Propietario, Telefono_Propietario, Marca, Modelo, Año, Placa, Color, Tipo, Kilometraje_Actual, Fecha_Registro, Activo) VALUES (@Propietario, @Telefono_Prop, @Marca, @Modelo, @Año, @Placa, @Color, @Tipo, @Kilometraje_Act, GETDATE(), 1)", connection, transaction);

            command.Parameters.AddWithValue("@Propietario", vehiculo.Propietario);
            command.Parameters.AddWithValue("@Telefono_Prop", vehiculo.Telefono_Propietario);
            command.Parameters.AddWithValue("@Marca", vehiculo.Marca);
            command.Parameters.AddWithValue("@Modelo", vehiculo.Modelo);
            command.Parameters.AddWithValue("@Año", vehiculo.Año);
            command.Parameters.AddWithValue("@Placa", vehiculo.Placa);
            command.Parameters.AddWithValue("@Color", vehiculo.Color);
            command.Parameters.AddWithValue("@Tipo", vehiculo.Tipo);
            command.Parameters.AddWithValue("@Kilometraje_Act", vehiculo.Kilometraje_Actual);

            await command.ExecuteNonQueryAsync();
        }




        public async Task<int?> ObtenerIdVehiculoPorPlacaAsync(string placa, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand command = new SqlCommand("SELECT Id_Vehiculo FROM Vehiculo WHERE Placa = @Placa", connection, transaction);
            command.Parameters.AddWithValue("@Placa", placa);

            object result = await command.ExecuteScalarAsync();

            if (result != null && int.TryParse(result.ToString(), out int id))
            {
                return id;
            }

            return null;
        }


        public int DeterminarIDEmpleado(string NombreEmpleado, string ApellidoEmp)
        {
            using (SqlConnection connection = con.Conexion())
            {

                SqlCommand command = new SqlCommand("SELECT Id_Empleado FROM Empleado WHERE Nombre = @NombreEmp AND Apellido = @ApeEmp", connection);
                command.Parameters.AddWithValue("@NombreEmp", NombreEmpleado);
                command.Parameters.AddWithValue("@ApeEmp", ApellidoEmp);
                try
                {
                    connection.Open();
                    int IdEmp = Convert.ToInt16(command.ExecuteScalar());
                    return IdEmp;


                }
                catch (Exception ex)
                {

                    return -1;
                }


            }

        }


        public List<Servicio> ServiciosActuales()
        {

            List<Servicio> serviceList = new List<Servicio>();
            using (SqlConnection connection = con.Conexion())
            {

                SqlCommand command = new SqlCommand("Select * from Servicio", connection);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Servicio servicio = new Servicio
                            {
                                Id_Servicio = reader.GetInt32(reader.GetOrdinal("Id_Servicio")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                costo = reader.GetDecimal(reader.GetOrdinal("Costo")),
                                duracionMin = reader.GetInt32(reader.GetOrdinal("Duracion_Estimada_Minutos"))
                            };

                            // Agrega el objeto a la lista
                            serviceList.Add(servicio);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Servicio servicio = new Servicio
                    {
                        Nombre = ex.Message

                    };

                    serviceList.Add(servicio);

                    return serviceList;
                }

                return serviceList;


            }

        }


        public List<Repuesto> RepuestosActuales()
        {
            List<Repuesto> repuestos = new();

            using (SqlConnection connection = con.Conexion())
            {

                SqlCommand command = new SqlCommand("Select * from Repuesto", connection);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Repuesto repuesto = new Repuesto()
                            {
                                Id_Repuesto = reader.GetInt32(reader.GetOrdinal("Id_Repuesto")),
                                Id_Proveedor = reader.GetInt32(reader.GetOrdinal("Id_Proveedor")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                Precio_Unitario = reader.GetDecimal(reader.GetOrdinal("Precio_Unitario")),
                                stock = reader.GetInt32(reader.GetOrdinal("Stock"))

                            };

                            // Agrega el objeto a la lista
                            repuestos.Add(repuesto);
                        }
                    }

                }
                catch (Exception ex)
                {


                    return null;
                }

                return repuestos;


            }
        }
        public List<Mantenimiento> MantenimientosActuales()
        {
            List<Mantenimiento> mantenimientos = new();

            using (SqlConnection connection = con.Conexion())
            {

                SqlCommand command = new SqlCommand("Select * from Mantenimiento", connection);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Mantenimiento mantenimiento = new Mantenimiento()
                            {
                                Id_Mantenimiento = reader.GetInt32(reader.GetOrdinal("Id_Mantenimiento")),
                                Id_Vehiculo = reader.GetInt32(reader.GetOrdinal("Id_Vehiculo")),
                                Fecha_Inicio = reader.GetDateTime(reader.GetOrdinal("Fecha_Inicio")),
                                Fecha_Fin = reader.GetDateTime(reader.GetOrdinal("Fecha_Fin")),
                                Estado = reader.GetString(reader.GetOrdinal("Estado")),
                                Observaciones = reader.GetString(reader.GetOrdinal("Observaciones")),
                                Costo_Total = reader.GetDecimal(reader.GetOrdinal("Costo_Total")),
                                Pagado = reader.GetBoolean(reader.GetOrdinal("Pagado"))
                            };

                            // Agrega el objeto a la lista
                            mantenimientos.Add(mantenimiento);
                        }
                    }

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.ToString());
                    return null;
                }

                return mantenimientos;


            }


        }


        public string NombreUser(string usuario)
        {

            using (SqlConnection connection = con.Conexion())
            {

                SqlCommand command = new SqlCommand("Select Nombre, Apellido from Empleado Where Usuario = @user", connection);
                command.Parameters.AddWithValue("@user", usuario);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nombre = reader["Nombre"].ToString();
                            string apellido = reader["Apellido"].ToString();
                            return nombre + " " + apellido;
                        }

                        else return "WTF";
                    }

                }
                catch (Exception ex)
                {

                    return ex.Message;
                }


            }


        }

        public List<Empleado> MecanicosActuales()
        {
            List<Empleado> mecanicos = new();

            using (SqlConnection connection = con.Conexion())
            {

                SqlCommand command = new SqlCommand("Select * from Empleado", connection);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Empleado mecanico = new Empleado()
                            {
                                Id_Empleado = reader.GetInt32(reader.GetOrdinal("Id_Empleado")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                                Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                                Fecha_Contratacion = reader.GetDateTime(reader.GetOrdinal("Fecha_Contratacion")),
                                Salario = reader.GetDecimal(reader.GetOrdinal("Salario")),
                                Usuario = reader.GetString(reader.GetOrdinal("Usuario")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                puesto = reader.GetString(reader.GetOrdinal("Tipo"))
                            };

                            // Agrega el objeto a la lista
                            mecanicos.Add(mecanico);
                        }
                    }

                    return mecanicos;

                }

                catch (Exception ex)
                {


                    return null;
                }


            }
        }

        public void actualizarEstadoMantenimiento(int idMantenimiento, string estado)
        {

            using (SqlConnection connection = con.Conexion())
            {

                SqlCommand command = new SqlCommand("update Mantenimiento \r\nset Estado = @estado where Id_Mantenimiento= @idmant", connection);
                command.Parameters.AddWithValue("idmant", idMantenimiento);
                command.Parameters.AddWithValue("@estado", estado);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al insertar estado mantenimiento: {ex.Message}");
                }


            }
        }
    }

}
