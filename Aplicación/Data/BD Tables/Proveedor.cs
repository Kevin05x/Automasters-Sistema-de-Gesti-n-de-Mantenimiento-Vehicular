using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BD_Tables
{
    public class Proveedor
    {
        public int Id_Proveedor { get; set; }
        public string Nombre { get; set; }

        public string Telefono { get; set; }

        public string Direccion {  get; set; }

        public string Correo {  get; set; }

        public string RUC { get; set; }

        public DateTime Fecha_Registro { get; set; }

        public List<Repuesto> Repuestos { get; set; } = new List<Repuesto>();
    }
}
