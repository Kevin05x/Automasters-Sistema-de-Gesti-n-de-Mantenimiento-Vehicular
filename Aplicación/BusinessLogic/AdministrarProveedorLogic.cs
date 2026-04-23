using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLogic
{
    public class AdministrarProveedorLogic
    {
        public DataTable listaProveedores()
        {
            ProveedorData proveedor = new ProveedorData();
            return proveedor.proveedores();
        }

        public DataTable insertar(string nombre, string direccion, string telefono, string correo, string ruc)
        {
            ProveedorData dAdProveedor = new ProveedorData();
            return dAdProveedor.insertar(nombre, direccion, telefono, correo, ruc);
        }

        public DataTable actualizar(int id, string nombre, string direccion, string telefono, string correo, string ruc)
        {
            ProveedorData dAdProveedor = new ProveedorData();
            return dAdProveedor.actualizar(id, nombre, direccion, telefono, correo, ruc);
        }

        public DataTable BuscarProv(string nombre)
        {
            ProveedorData BusProv = new ProveedorData();
            return BusProv.buscar(nombre);
        }
    }
}