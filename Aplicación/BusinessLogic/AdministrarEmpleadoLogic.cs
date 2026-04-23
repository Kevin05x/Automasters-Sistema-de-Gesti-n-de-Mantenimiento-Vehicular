using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.BD_Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class AdministrarEmpleadoLogic
    {

        public DataTable listaEmpleado()
        {
            EmpleadosData empleado = new EmpleadosData();
            return empleado.empleados();
        }

        public DataTable filtrarempleado(string tipo)
        {
            EmpleadosData fil = new EmpleadosData();
            return fil.filtrar(tipo);
        }

        public DataTable insertar(string nom, string ape, string tel, string sal, string usu, string pass, string tipo)
        {
            EmpleadosData dAdempleados = new EmpleadosData();
            return dAdempleados.insertar(nom, ape, tel, sal, usu, pass, tipo);
        }

        public DataTable actualizar(int id, string nom, string ape, string tel, decimal sal, string usu, string pass, string tipo)
        {
            EmpleadosData dAdempleados = new EmpleadosData();
            return dAdempleados.actualizar(id, nom, ape, tel, sal, usu, pass, tipo);
        }

        

        public DataTable buscar(int id)
        {
            EmpleadosData dades = new EmpleadosData();
            return dades.buscar(id);
        }


    }
}