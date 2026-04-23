using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BD_Tables
{
    public class Mantenimiento
    {
        public int Id_Mantenimiento { get; set; }
        public int Id_Vehiculo { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public string Estado {  get; set; }
        public string Observaciones { get; set; }
        public decimal Costo_Total { get; set; }
        public bool Pagado {  get; set; }

    }
}
