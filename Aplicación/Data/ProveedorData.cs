using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Data
{
    public class ProveedorData
    {
        Connection conexion = new Connection();

        public DataTable proveedores()
        {
            try
            {

                SqlConnection con = conexion.Conexion();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Proveedor", con);
                DataTable dt = new DataTable();
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                con.Close();

                Console.WriteLine("Se obtuvieron los proveedores");
                
                return dt;

            }

            catch (Exception ex) { Console.WriteLine(ex.Message);  return null; }
            
        }

        public DataTable filtrar(string criterio)
        {
            SqlConnection con = conexion.Conexion();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Proveedor WHERE Nombre = @criterio OR Direccion = @criterio OR Telefono = @criterio", con);
            cmd.Parameters.AddWithValue("@criterio", criterio);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;
        }

        public DataTable actualizar(int id, string nombre, string direccion, string telefono, string correo, string ruc)
        {
            string consulta = "UPDATE Proveedor SET Nombre = @nombre, Direccion = @direccion, Telefono = @telefono, Correo = @correo, RUC = @ruc WHERE Id_Proveedor = @id";
            SqlConnection con = conexion.Conexion();
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@direccion", direccion);
            cmd.Parameters.AddWithValue("@telefono", telefono);
            cmd.Parameters.AddWithValue("@correo", correo);
            cmd.Parameters.AddWithValue("@ruc", ruc);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;
        }

        public DataTable insertar(string nombre, string direccion, string telefono, string correo, string ruc)
        {
            string consulta = "INSERT INTO Proveedor (Nombre, Direccion, Telefono, Correo, RUC) VALUES (@nombre, @direccion, @telefono, @correo, @ruc)";
            SqlConnection con = conexion.Conexion();
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@direccion", direccion);
            cmd.Parameters.AddWithValue("@telefono", telefono);
            cmd.Parameters.AddWithValue("@correo", correo);
            cmd.Parameters.AddWithValue("@ruc", ruc);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;
        }

        public DataTable buscar(string nombre)
        {
            SqlConnection con = conexion.Conexion();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Proveedor WHERE Nombre LIKE @nombre", con);
            cmd.Parameters.AddWithValue("@nombre", "%" + nombre + "%");
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;
        }
    }
}