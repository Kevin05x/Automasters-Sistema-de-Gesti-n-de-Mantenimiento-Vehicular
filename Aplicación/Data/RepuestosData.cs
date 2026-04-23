using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.BD_Tables;
using Microsoft.Data.SqlClient;

namespace Data
{
    public class RepuestosData
    {

        Connection conexion = new Connection();

        public DataTable repuestos()
        {
            SqlConnection con = conexion.Conexion();

            SqlCommand cmd = new SqlCommand("SELECT * FROM Repuesto", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;
        }

        public DataTable filtrar(int id)
        {
            try
            {
                SqlConnection con = conexion.Conexion();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Repuesto WHERE Id_Proveedor = @idproveedor ", con);
                cmd.Parameters.AddWithValue("@idproveedor", id);
                DataTable dt = new DataTable();
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                con.Close();
                return dt;
            }
            catch (SqlException ex)
            {
                return null;
            }
        }

        public DataTable buscar(string nombre)
        {
            SqlConnection con = conexion.Conexion();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Repuesto WHERE Nombre LIKE @nombre", con);
            cmd.Parameters.AddWithValue("@nombre", "%" + nombre + "%");
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;
        }

        public List<Proveedor> Proveedores()
        {
            List<Proveedor> proveedores = new();
            SqlConnection con = conexion.Conexion();

            using (con)
            {

                SqlCommand command = new SqlCommand("Select * from Proveedor", con);

                try
                {
                    con.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Proveedor proveedor = new Proveedor()
                            {
                                Id_Proveedor = reader.GetInt32(reader.GetOrdinal("Id_Proveedor")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            };

                            // Agrega el objeto a la lista
                            proveedores.Add(proveedor);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                    return null;
                }
                return proveedores;
            }
        }

        public DataTable actualizar(int id, string nom, string des, decimal precio, int stock, int idre)
        {
            string consulta = "UPDATE Repuesto set Id_Proveedor = @id, Nombre = @nom, Descripcion = @des, Precio_Unitario = @precio, Stock = @stock " +
                " WHERE Id_Repuesto = @idre";
            SqlConnection con = conexion.Conexion();
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nom", nom);
            cmd.Parameters.AddWithValue("@des", des);
            cmd.Parameters.AddWithValue("@precio", precio);
            cmd.Parameters.AddWithValue("@stock", stock);
            cmd.Parameters.AddWithValue("@idre", idre);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;

        }

        public DataTable insertar(int id, string nom, string des, decimal precio, int stock)
        {
            string consulta = "INSERT INTO Repuesto (Id_Proveedor, Nombre, Descripcion, Precio_Unitario, Stock)" +
                " VALUES (@idpro, @nom, @des, @pre, @stock)";
            SqlConnection con = conexion.Conexion();
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@idpro", id);
            cmd.Parameters.AddWithValue("@nom", nom);
            cmd.Parameters.AddWithValue("@des", des);
            cmd.Parameters.AddWithValue("@pre", precio);
            cmd.Parameters.AddWithValue("@stock", stock);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;

        }

        public DataTable buscar(int id)
        {
            string consulta = "SELECT * FROM Empleado WHERE Id_Empleado = @id";
            using (SqlConnection con = conexion.Conexion())
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(consulta, con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader sdtr = cmd.ExecuteReader();
                dt.Load(sdtr);
                con.Close();
                return dt;
            }
        }
    }
}
