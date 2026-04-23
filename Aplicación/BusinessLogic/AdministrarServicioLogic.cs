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
    public class AdministrarServicioLogic
    {

        public DataTable listaServicio()
        {
            ServiciosData servicio = new ServiciosData();
            return servicio.servicios();
        }

        public DataTable OrdenarServicio(string tipo)
        {
            ServiciosData fil = new ServiciosData();
            return fil.Ordenar(tipo);
        }

        public DataTable insertar(string nombre, string descripcion, decimal costo, int duracion)
        {
            ServiciosData dAdServicios = new ServiciosData();
            return dAdServicios.insertar(nombre, descripcion, costo, duracion);
        }

        public DataTable actualizar( int id, string nombre, string descripcion, decimal costo, int duracion)
        {
            ServiciosData dAdServicios = new ServiciosData();
            return dAdServicios.actualizar( id,nombre, descripcion, costo, duracion);
        }

        public DataTable BuscarServicio(string nombre)
        {
            ServiciosData Busc = new ServiciosData();
            return Busc.buscar(nombre);
        }
    }
}