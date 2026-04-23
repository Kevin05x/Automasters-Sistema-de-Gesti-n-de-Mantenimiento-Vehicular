using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.BD_Tables;

namespace BusinessLogic
{
    public class LogicaGeneral
    {

        public void actualizarEstadoMantenimiento()
        {
            List<Mantenimiento> mantenimientos = new LogicaDatos().MantenimientosActuales();

            foreach (var mantenimiento in mantenimientos) {


                if (DateTime.Today >= mantenimiento.Fecha_Fin)
                {

                    new LogicaDatos().actualizarEstadoMantenimiento(mantenimiento.Id_Mantenimiento, "Completado");
                
                }

            
            }

        }
    }
}
