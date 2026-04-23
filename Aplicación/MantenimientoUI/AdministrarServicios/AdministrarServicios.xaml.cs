using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BusinessLogic;
using System.Data.SqlClient;
using MantenimientoUI.VentanasEmer;
using System.Data;

namespace MantenimientoUI.VentanasDer
{
    /// <summary>
    /// Lógica de interacción para AdmServices.xaml
    /// </summary>
    public partial class AdmServices : UserControl
    {
        public AdmServices()
        {
            InitializeComponent();
            cargarDataGrid();
        }

        public void cargarDataGrid()
        {
            AdministrarServicioLogic bAdServicio = new AdministrarServicioLogic();
            ServiciosDataGrid.ItemsSource = bAdServicio.listaServicio().DefaultView;
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            // Implementación del evento de actualización
        }

        private void btnAñadirServicio_Click(object sender, RoutedEventArgs e)
        {
            Registrar_ActualizarServicio registrar_Actualizar = new();

            registrar_Actualizar.btnRegistrar.Visibility = Visibility.Visible;
            registrar_Actualizar.btnActualizar.Visibility = Visibility.Collapsed;

            registrar_Actualizar.Closed += (s, e) => cargarDataGrid();

            registrar_Actualizar.ShowDialog();

            bool? result = registrar_Actualizar.DialogResult;

            if (result == true)
            {
                cargarDataGrid();
            }
            
        }

        private void ActualizarServicio_Click(object sender, RoutedEventArgs e)
        {
            if (ServiciosDataGrid.SelectedItem == null) { MessageBox.Show("Seleccione un servicio porfavor"); return; }
            var dataRowView = ServiciosDataGrid.SelectedItem as DataRowView;
            int id = Convert.ToUInt16(dataRowView["Id_Servicio"].ToString());

            Registrar_ActualizarServicio registrar_Actualizar = new Registrar_ActualizarServicio(id);

            registrar_Actualizar.btnRegistrar.Visibility = Visibility.Collapsed;
            registrar_Actualizar.btnActualizar.Visibility = Visibility.Visible;
            registrar_Actualizar.txtNombre.Text = dataRowView["Nombre"].ToString();
            registrar_Actualizar.txtDescripcion.Text = dataRowView["Descripcion"].ToString();
            registrar_Actualizar.txtCosto.Text = dataRowView["Costo"].ToString();
            registrar_Actualizar.txtDuracion.Text = dataRowView["Duracion_Estimada_Minutos"].ToString();


            registrar_Actualizar.Closed += (s, e) => cargarDataGrid();

            registrar_Actualizar.ShowDialog();

            bool? result = registrar_Actualizar.DialogResult;

            if (result == true) {
                cargarDataGrid();
            }
        
        }

        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            AdministrarServicioLogic ServBusc = new AdministrarServicioLogic();
            ServiciosDataGrid.ItemsSource = ServBusc.BuscarServicio(txtNombre.Text).DefaultView;
        }



    }
}