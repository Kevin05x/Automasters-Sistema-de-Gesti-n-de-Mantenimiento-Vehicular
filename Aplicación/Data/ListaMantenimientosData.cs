using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ListaMantenimientosData
    {
        SqlConnection con;


        public DataTable mantenimientos()
        {
            Connection conexion = new Connection();
            con = conexion.Conexion();
            SqlCommand cmd = new SqlCommand("select m.Id_Mantenimiento, Propietario, Marca as 'Marca Vehiculo', Modelo, Año, Placa, Color, Tipo, Estado,  \r\n" +
                "Kilometraje_Actual, Costo_Total,\r\n(case when (Costo_Total - SUM(p.Monto)) Is NULL then Costo_Total else (Costo_Total - SUM(p.Monto)) end) as Debe , \r\n\t\t\t\t(case when Pagado = 1 then 'Si' else 'No' End) as Pagado \r\n" +
                "from Mantenimiento m \r\n\tinner join Vehiculo v on m.Id_Vehiculo = v.Id_Vehiculo\r\n\t\t\t\tLeft join Pago p ON p.Id_Mantenimiento = m.Id_Mantenimiento\r\n\r\n\t\t\t\t " +
                "Group by \r\n\t\t\t\t m.Id_Mantenimiento, Propietario, Marca , Modelo, Año, Placa, Color, Tipo, Estado,  \r\n" +
                "Kilometraje_Actual, Costo_Total, Pagado", con);
            DataTable dataTable = new DataTable();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dataTable.Load(reader);
                return dataTable;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable filtroMantenimientos(int pagado, string estado)
        {
            Connection conexion = new Connection();
            con = conexion.Conexion();
            SqlCommand cmd = new SqlCommand("select m.Id_Mantenimiento, Propietario, Marca as 'Marca Vehiculo', Modelo, Año, Placa, Color, " +
                "Tipo, Estado, Kilometraje_Actual, Costo_Total, (case when (Costo_Total - SUM(p.Monto)) Is NULL then Costo_Total else (Costo_Total - SUM(p.Monto)) end) as Debe, " +
                "(case when Pagado = 1 then 'Si' else 'No' End) as Pagado from Mantenimiento m " +
                "inner join Vehiculo v on m.Id_Vehiculo = v.Id_Vehiculo Left join Pago p ON p.Id_Mantenimiento = m.Id_Mantenimiento " +
                "where m.Estado = @Estado or m.Pagado = @Pagado " +
                "Group by m.Id_Mantenimiento, Propietario, Marca , Modelo, Año, Placa, Color, Tipo, Estado, Kilometraje_Actual, Costo_Total, Pagado", con);
            cmd.Parameters.AddWithValue("@Pagado", pagado);
            cmd.Parameters.AddWithValue("@Estado", estado);
            DataTable dataTable = new DataTable();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dataTable.Load(reader);
                return dataTable;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }

        public List<DataTable> mantenimientos(int ID)
        {
            List<DataTable> listaDataTable = new List<DataTable>();
            Connection conexion = new Connection();


            DataTable EjecutarConsulta(string consulta, int Id)
            {
                DataTable dataTable = new DataTable();
                using (con = conexion.Conexion())
                {
                    using (SqlCommand cmd = new SqlCommand(consulta, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", Id);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
                return dataTable;
            }

            //Obtenemos datos mantenimiento
            try
            {
                string consultaMantenimiento = "select Fecha_Inicio, Fecha_Fin, Observaciones, Estado\r\n" +
                "from Mantenimiento\r\nwhere Id_Mantenimiento = @Id";

                listaDataTable.Add(EjecutarConsulta(consultaMantenimiento, ID));

                string consultaVehiculo = "select Propietario, Marca, Modelo, Año, Placa, Color, v.Tipo, Kilometraje_Actual" +
                "\r\nfrom Mantenimiento m inner join Vehiculo v on m.Id_Vehiculo = v.Id_Vehiculo\r\nWhere Id_Mantenimiento = @Id";

                listaDataTable.Add(EjecutarConsulta(consultaVehiculo, ID));

                string consultaRepuestos = "select Nombre, dr.Cantidad\r\nfrom Repuesto r " +
                    "inner join [Detalle Repuesto] dr on r.Id_Repuesto = dr.Id_Repuesto \r\n" +
                    "inner join Mantenimiento m on m.Id_Mantenimiento = dr.Id_Mantenimiento\r\n" +
                    "Where m.Id_Mantenimiento = @Id";

                listaDataTable.Add(EjecutarConsulta(consultaRepuestos, ID));

                string consultaServicios = "select s.Nombre\r\nfrom Mantenimiento m " +
                    "inner join [Detalle Mantenimiento] dm on m.Id_Mantenimiento = dm.Id_Mantenimiento\r\n" +
                    "inner join Servicio s on s.Id_Servicio = dm.Id_Servicio\r\nwhere m.Id_Mantenimiento = @Id";

                listaDataTable.Add(EjecutarConsulta(consultaServicios, ID));

                string consultaEmpleados = "select s.Nombre as Servicio, e.Nombre + ' ' + e.Apellido as Nombre\r\nfrom Mantenimiento m " +
                    "inner join [Detalle Mantenimiento] dm on m.Id_Mantenimiento = dm.Id_Mantenimiento\r\n" +
                    "inner join Servicio s on s.Id_Servicio = dm.Id_Servicio\r\n" +
                    "inner join Empleado e on e.Id_Empleado = dm.Id_Empleado\r\n" +
                    "where m.Id_Mantenimiento = @Id";

                listaDataTable.Add(EjecutarConsulta(consultaEmpleados, ID));

                string consultaIncidente = "select Descripcion, Fecha_reporte, Solucion\r\nfrom Incidente i " +
                    "inner join Mantenimiento m on i.Id_Mantenimiento = m.Id_Mantenimiento\r\n" +
                    "where m.Id_Mantenimiento = @Id";

                listaDataTable.Add(EjecutarConsulta(consultaIncidente, ID));

            }
            catch (Exception ex)
            {

            }

            return listaDataTable;
        }

        public void agregarIncidente(int Id, string descripcion, string solucion)
        {
            Connection conexion = new Connection();

            using (con = conexion.Conexion())
            {
                using (SqlCommand cmd = new SqlCommand("insert into Incidente(Id_Mantenimiento, Descripcion, Solucion)\r\n" +
                    "values (@Id, @Descripcion, @Solucion)", con))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@Solucion", solucion);
                    con.Open();
                    cmd.ExecuteReader();

                }
            }

        }

        public DataTable detallePago(int Id)
        {
            Connection conexion = new Connection();
            DataTable dataTable = new DataTable();

            using (con = conexion.Conexion())
            {
                using (SqlCommand cmd = new SqlCommand("select Tipo_Pago, Monto from Pago\r\n" +
                    "where Id_Mantenimiento = @Id", con))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }
            return dataTable;
        }

        public void agregarPago(int Id, string tipoPago, double monto, string metodoPago)
        {
            Connection conexion = new Connection();

            using (con = conexion.Conexion())
            {
                using (SqlCommand cmd = new SqlCommand("insert into Pago (Id_Mantenimiento, Tipo_Pago, Monto, Metodo_Pago)\r\n" +
                    "values (@Id, @Tipo, @Monto, @Metodo)", con))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Tipo", tipoPago);
                    cmd.Parameters.AddWithValue("@Monto", monto);
                    cmd.Parameters.AddWithValue("@Metodo", metodoPago);
                    con.Open();
                    cmd.ExecuteReader();

                }
            }

        }

        public List<string> EmpleadoAcargoDeMantenimiento(int Id )
        {
            Connection conexion = new Connection();
            List<string> empleadosaCargo = new List<string>();
            using (con = conexion.Conexion())
            {
                using (SqlCommand cmd = new SqlCommand("Select distinct e.Nombre, e.Apellido From Mantenimiento m\r\n" +
                    "INNER JOIN [Detalle Mantenimiento] dm on dm.Id_Mantenimiento =  m.Id_Mantenimiento\r\n" +
                    "INNER JOIN Empleado e  on e.Id_Empleado = dm.Id_Empleado\r\n\r\nWhere m.Id_Mantenimiento = @idManteni", con))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        string nombreEmp;
                        foreach (DataRow row in reader) {

                            string nombre = Convert.ToString(reader.GetString(reader.GetOrdinal("Nombre")));
                            string apellido = Convert.ToString(reader.GetString(reader.GetOrdinal("Apellido")));

                            nombreEmp = nombre + " " + apellido;

                            empleadosaCargo.Add(nombreEmp);
                        }
                       
                    }
                }
            }
            return empleadosaCargo;
        }

    }
}
