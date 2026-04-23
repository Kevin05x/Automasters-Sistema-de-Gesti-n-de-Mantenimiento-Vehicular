using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BusinessLogic;

namespace MantenimientoUI.VentanasEmer
{
    /// <summary>
    /// Lógica de interacción para DetalleMantenimiento.xaml
    /// </summary>
    public partial class DetalleMantenimiento : Window
    {
        public DetalleMantenimiento()
        {
            InitializeComponent();
            List<String> listaVehiculo = new List<String>();
        }

        public void llenarDatos(int Id)
        {
            ListaMantenimientoLogic logic = new ListaMantenimientoLogic();
            List<DataTable> datalleMantenimiento = logic.detalleMantenimiento(Id);



            lbPropietario.Content = datalleMantenimiento[1].Rows[0]["Propietario"].ToString();

            lbxObservaciones.Items.Add(datalleMantenimiento[0].Rows[0]["Observaciones"].ToString());
            lbFechaInicio.Content = datalleMantenimiento[0].Rows[0]["Fecha_Inicio"].ToString();
            lbFechaFin.Content = datalleMantenimiento[0].Rows[0]["Fecha_Fin"].ToString();
            lbEstado.Content = datalleMantenimiento[0].Rows[0]["Estado"].ToString();
            

            DataRow row = datalleMantenimiento[1].Rows[0];

            lbxCaracteristicasVehiculo.Items.Add("Marca: " + row["Marca"].ToString());
            lbxCaracteristicasVehiculo.Items.Add("Modelo: " + row["Modelo"].ToString());
            lbxCaracteristicasVehiculo.Items.Add("Año: " + row["Año"].ToString());
            lbxCaracteristicasVehiculo.Items.Add("Placa: " + row["Placa"].ToString());
            lbxCaracteristicasVehiculo.Items.Add("Color: " + row["Color"].ToString());
            lbxCaracteristicasVehiculo.Items.Add("Tipo: " + row["Tipo"].ToString());
            lbxCaracteristicasVehiculo.Items.Add("Kilometraje Actual: " + row["Kilometraje_Actual"].ToString());



            foreach (DataRow row1 in datalleMantenimiento[2].Rows)
            {
                lbxRepuestos.Items.Add(row1["Nombre"].ToString() + "/Cantidad: " + row1["Cantidad"].ToString());
            }
            lbxRepuestos.Items.Refresh();

            foreach (DataRow row1 in datalleMantenimiento[3].Rows)
            {
                lbxServicios.Items.Add(row1["Nombre"].ToString());
            }
            lbxServicios.Items.Refresh();

            foreach (DataRow row1 in datalleMantenimiento[4].Rows)
            {
                lbxEmpleadosCargo.Items.Add(row1["Nombre"].ToString() + " hizo: " + row1["Servicio"].ToString());
            }
            lbxEmpleadosCargo.Items.Refresh();

            if (datalleMantenimiento[5].Rows.Count != 0)
            {
                foreach (DataRow row1 in datalleMantenimiento[5].Rows)
                {
                    lbxIncidentes.Items.Add(row1["Descripcion"].ToString() + " - " + row1["Fecha_reporte"].ToString());
                }
            }
            else
                lbxIncidentes.Items.Add("No existen incidentes");
            lbxIncidentes.Items.Refresh();
        }

       
    }
}
