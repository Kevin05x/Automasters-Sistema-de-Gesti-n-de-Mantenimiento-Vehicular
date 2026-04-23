using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BD_Tables
{
    public class Servicio
    {
        public int Id_Servicio { get; set; }

        public string Nombre { get; set; }

        public string descripcion {  get; set; }

        public decimal costo { get; set; }

        public int duracionMin {  get; set; }


    }
}
