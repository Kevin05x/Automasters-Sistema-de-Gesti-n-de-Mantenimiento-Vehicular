using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ServiciosData
    {
        Connection conexion = new Connection();

        public DataTable servicios()
        {
            SqlConnection con = conexion.Conexion();

            SqlCommand cmd = new SqlCommand("SELECT * FROM Servicio", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;
        }


        public DataTable Ordenar(string tipo)
        {
            SqlConnection con = conexion.Conexion();
            string query = "SELECT * FROM Servicio ";

            switch (tipo.ToLower())
            {
                case "nombre":
                    query += "ORDER BY Nombre ASC";
                    break;
                case "costo":
                    query += "ORDER BY Costo DESC";
                    break;
                case "duracion":
                    query += "ORDER BY Duracion_Estimada_Minutos ASC";
                    break;
                default:
                    throw new ArgumentException("Tipo de ordenación no válido.");
            }

            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;
        }


        public DataTable actualizar( int id,string nombre, string descripcion, decimal costo, int duracion)
        {
            string consulta = "UPDATE Servicio SET Nombre = @nombre, Descripcion = @descripcion, Costo = @costo, Duracion_Estimada_Minutos = @duracion WHERE Id_Servicio = @id";
            SqlConnection con = conexion.Conexion();
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);
            cmd.Parameters.AddWithValue("@costo", costo);
            cmd.Parameters.AddWithValue("@duracion", duracion);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;
        }


        public DataTable insertar(string nombre, string descripcion, decimal costo, int duracion)
        {
            string consulta = "INSERT INTO Servicio (Nombre, Descripcion, Costo, Duracion_Estimada_Minutos) " +
                              "VALUES (@nombre, @descripcion, @costo, @duracion)";
            SqlConnection con = conexion.Conexion();
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);
            cmd.Parameters.AddWithValue("@costo", costo);
            cmd.Parameters.AddWithValue("@duracion", duracion);
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM Servicio WHERE Nombre LIKE @nombre", con);
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