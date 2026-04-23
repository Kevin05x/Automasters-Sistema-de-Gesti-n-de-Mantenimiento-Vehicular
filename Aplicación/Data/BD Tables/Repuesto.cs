using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace Data.BD_Tables
{
    public class Repuesto
    {
        public int Id_Repuesto { get; set; }
        
        public int Id_Proveedor { get; set; }

        public string Nombre { get; set; }
        
        public string Descripcion { get; set; }

        public decimal Precio_Unitario { get; set; }
        
        public int stock { get; set; }


    }
}
