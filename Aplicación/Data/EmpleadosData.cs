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
    public class EmpleadosData
    {
        Connection conexion = new Connection();

        public DataTable empleados()
        {
            SqlConnection con = conexion.Conexion();

            SqlCommand cmd = new SqlCommand("SELECT * FROM Empleado", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;
        }

        public DataTable filtrar(string tipo)
        {
            SqlConnection con = conexion.Conexion();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Empleado WHERE Tipo = @tipo ", con);
            cmd.Parameters.AddWithValue("@tipo", tipo);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;
        }

        public DataTable actualizar(int id, string nom, string ape, string tel, decimal sal, string usu, string pass, string tipo)
        {
            string consulta = "UPDATE Empleado SET Nombre = @nom, Apellido = @ape, Telefono = @tel, Salario = @sal, Usuario = @usu, " +
                              "Password = @pass, Tipo = @tipo WHERE Id_Empleado = @id";
            SqlConnection con = conexion.Conexion();
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@nom", nom);
            cmd.Parameters.AddWithValue("@ape", ape);
            cmd.Parameters.AddWithValue("@tel", tel);
            cmd.Parameters.AddWithValue("@sal", sal);
            cmd.Parameters.AddWithValue("@usu", usu);
            cmd.Parameters.AddWithValue("@pass", pass);
            cmd.Parameters.AddWithValue("@tipo", tipo);
            cmd.Parameters.AddWithValue("@id", id);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;
        }

        public DataTable insertar(string nom, string ape, string tel, string sal, string usu, string pass, string tipo)
        {
            string consulta = "INSERT INTO Empleado (Nombre, Apellido, Telefono, Salario, Usuario, Password, Tipo) " +
                " VALUES (@nom, @ape, @tel, @sal, @usu, @pas, @tipo) ";
            SqlConnection con = conexion.Conexion();
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@nom", nom);
            cmd.Parameters.AddWithValue("@ape", ape);
            cmd.Parameters.AddWithValue("@tel", tel);
            cmd.Parameters.AddWithValue("@sal", sal);
            cmd.Parameters.AddWithValue("@usu", usu);
            cmd.Parameters.AddWithValue("@pas", pass);
            cmd.Parameters.AddWithValue("@tipo", tipo);
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