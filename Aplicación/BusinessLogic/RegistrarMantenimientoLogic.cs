using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.BD_Tables;
using Microsoft.Identity.Client;

namespace BusinessLogic
{
    public class RegistrarMantenimientoLogic
    {

        Vehiculo vehiculo1 = new Vehiculo();

        public void RegAuto(string Propietario, string telefonoProp, string marca, string Modelo, int Año, string placa, string color, int kilometraje, string tipo) {

            Vehiculo vehiculo = new Vehiculo() {

                Propietario = Propietario,
                Telefono_Propietario = telefonoProp,
                Marca = marca,
                Modelo = Modelo,
                Año = Año,
                Placa = placa,
                Kilometraje_Actual = kilometraje,
                Tipo = tipo,
                Color = color,
                Activo = true
                

            };

            vehiculo1 = vehiculo;


        }

        public async Task RegistrarMantenimiento(DateTime FechaInicio, int duracion, string estado, string observaciones, double costoTot, List<string> servicios, string MecanicoAcargo, Dictionary<string, int> repuestos)
        {
            //Se obtiene el nombre y apellido del mecanico a cargo
            string[] mecanico = MecanicoAcargo.Split(' ');
            string NombreEmpleado = mecanico[0];
            string ApellidoEmpleado = mecanico[1];

            bool pagado = false;


            new Data.LogicaDatos().InsertarMantenimientoAsync(FechaInicio, duracion, estado, observaciones, costoTot, pagado, servicios, NombreEmpleado, ApellidoEmpleado, repuestos, vehiculo1);

        }
    
    }
}
