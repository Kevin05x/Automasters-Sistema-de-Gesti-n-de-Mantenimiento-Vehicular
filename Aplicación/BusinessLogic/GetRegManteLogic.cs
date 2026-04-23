using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace BusinessLogic
{
    public class GetRegManteLogic
    {

        public GetRegManteLogic()
        {

        }

        public List<string> nameServicios()
        {

            List<string> nombresServicios = new List<string>();

            LogicaDatos logicaDatos = new LogicaDatos();

            foreach (var service in logicaDatos.ServiciosActuales())
            {

                nombresServicios.Add(service.Nombre);

            }

            return nombresServicios;

        }


        public double precioServicio(string nombre)
        {
            double precio = 0;
            LogicaDatos logicaDatos = new LogicaDatos();
            foreach (var service in logicaDatos.ServiciosActuales())
            {
                if (service.Nombre.Equals(nombre))
                {
                    precio = Convert.ToDouble(service.costo);
                }
            }
            return precio;
        }

        public List<string> nameRepuestos()
        {

            List<string> nombresRep = new List<string>();

            LogicaDatos logicadatos = new LogicaDatos();
            foreach (var repuesto in logicadatos.RepuestosActuales())
            {
                nombresRep.Add(repuesto.Nombre);

            }

            return nombresRep;


        }

        public double precioRepuesto(string nombre)
        {
            double precio = 0;
            LogicaDatos logicaDatos = new LogicaDatos();
            foreach (var repuesto in logicaDatos.RepuestosActuales())
            {
                if (repuesto.Nombre.Equals(nombre))
                {
                    precio = Convert.ToDouble(repuesto.Precio_Unitario);
                }
            }
            return precio;
        }

        public List<string> NombresMecanicos()
        {
            List<string> nombresMecanicos = new List<string>();

            LogicaDatos logicaDatos = new LogicaDatos();
            foreach (var mecanico in logicaDatos.MecanicosActuales())
            {
                if(mecanico.puesto.Equals("Mecánico"))
                nombresMecanicos.Add(mecanico.Nombre + " " + mecanico.Apellido);
            }

            return nombresMecanicos;

        }
    }
}
