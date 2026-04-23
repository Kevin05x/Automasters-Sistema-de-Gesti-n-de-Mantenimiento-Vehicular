using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Data
{
    internal class Connection
    {

    
        public SqlConnection Conexion()
        {

            SqlConnection con = new SqlConnection();

            string conex = "Data Source=.;Initial Catalog=MantenimientoVehiculos;Integrated Security=True;Trust Server Certificate=True";
            con = new SqlConnection(conex);

            return con;
        }



    }
}
