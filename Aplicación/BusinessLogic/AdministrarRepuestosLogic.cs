using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace BusinessLogic
{

    public class AdministrarRepuestosLogic
    {
        public AdministrarRepuestosLogic()
        {

        }

        public DataTable repuestos()
        {
            DataTable dt = new DataTable();
            RepuestosData repuestos = new RepuestosData();
            dt = repuestos.repuestos();
            return dt;
        }

        public DataTable buscar(string nombre)
        {
            DataTable dt = new DataTable();
            RepuestosData repuestos = new RepuestosData();
            dt = repuestos.buscar(nombre);
            return dt;
        }

        public DataTable filtrar(int id)
        {
            DataTable dt = new DataTable();
            RepuestosData respuestos = new RepuestosData();
            dt = respuestos.filtrar(id);
            return dt;
        }

        public List<string> proveedores()
        {
            List<string> proveedores = new List<string>();
            RepuestosData repuestos = new RepuestosData();

            proveedores.Add("Todos");

            foreach (var repuesto in repuestos.Proveedores())
            {
                proveedores.Add(repuesto.Id_Proveedor + "-" + repuesto.Nombre);
            }

            return proveedores;
        }

        public DataTable insertar(int id, string nom, string des, decimal precio, int stock)
        {
            DataTable dataTable = new DataTable();
            RepuestosData respuestos = new RepuestosData();
            dataTable = respuestos.insertar(id, nom, des, precio, stock);
            return dataTable;
        }

        public DataTable actualizar(int id, string nom, string des, decimal precio, int stock, int idre)
        {
            RepuestosData dRespuestos = new RepuestosData();
            DataTable dataTable = new DataTable();
            dataTable = dRespuestos.actualizar(id, nom, des, precio, stock, idre);

            return dataTable;
        }


    }
} 

