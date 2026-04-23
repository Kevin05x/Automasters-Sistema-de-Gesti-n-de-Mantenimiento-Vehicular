using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BD_Tables
{
    public class Vehiculo
    {
        public int Id_Vehiculo { get; set; }
        public string Propietario { get; set; }
        public string Telefono_Propietario { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Año { get; set; }
        public string Placa { get; set; }
        public string Color { get; set; }
        public string Tipo { get; set; }
        public int Kilometraje_Actual { get; set; }
        public DateTime Fecha_Registro { get; set; }
        public bool Activo { get; set; }

       

    }
}
