using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BD_Tables
{
    public partial class Empleado
    {
        public int Id_Empleado { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Puesto { get; set; }
        public DateTime Fecha_Contratacion { get; set; }
        public decimal Salario { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string puesto { get; set; }

        
    }
}
