using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Data;

namespace BusinessLogic
{
    public class ListaMantenimientoLogic
    {
        public DataTable mantenimientos()
        {
            ListaMantenimientosData listaMantenimientos = new ListaMantenimientosData();
            return listaMantenimientos.mantenimientos();
        }

        public DataTable mantenimientos(int pagado, string estado)
        {

            ListaMantenimientosData listaMantenimientos = new ListaMantenimientosData();
            return listaMantenimientos.filtroMantenimientos(pagado, estado);
        }

        public List<DataTable> detalleMantenimiento(int ID)
        {
            ListaMantenimientosData listaMantenimientos = new ListaMantenimientosData();
            return listaMantenimientos.mantenimientos(ID);
        }

        public void registrarIncidente(int Id, string descripcion, string solucion)
        {
            ListaMantenimientosData lista = new ListaMantenimientosData();
            lista.agregarIncidente(Id, descripcion, solucion);
        }

        public DataTable detallePago(int Id)
        {
            ListaMantenimientosData listaMantenimientos = new ListaMantenimientosData();
            return listaMantenimientos.detallePago(Id);
        }

        public void registrarPago(int Id, string tipoPago, double monto, string metodoPago)
        {
            ListaMantenimientosData lista = new ListaMantenimientosData();
            lista.agregarPago(Id, tipoPago, monto, metodoPago);
        }


        public string EmpleadosACargo(int id) {


            var DataEmp = new ListaMantenimientosData().EmpleadoAcargoDeMantenimiento(id);
            StringBuilder empleados = new StringBuilder();

            foreach (var empleado in DataEmp ) {

                if (empleados.Length > 0)
                {
                    empleados.Append(", ");
                }

            }

            return empleados.ToString();
        
        }
    }
}
