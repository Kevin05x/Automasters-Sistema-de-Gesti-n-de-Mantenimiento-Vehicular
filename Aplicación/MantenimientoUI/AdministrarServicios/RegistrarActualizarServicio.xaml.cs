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
using System.Windows.Shapes;
using BusinessLogic;
using System.Data.SqlClient;
using MantenimientoUI.VentanasDer;

namespace MantenimientoUI.VentanasEmer
{
    /// <summary>
    /// Lógica de interacción para Registrar_ActualizarServicio.xaml
    /// </summary>
    public partial class Registrar_ActualizarServicio : Window
    {
        public Registrar_ActualizarServicio()
        {
            InitializeComponent();
        }


        public Registrar_ActualizarServicio(int id)
        {
            InitializeComponent();
            txtId.Text = id.ToString();

        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (txtNombre.Text != string.Empty && txtDescripcion.Text != string.Empty && txtCosto.Text != string.Empty && txtDuracion.Text != string.Empty ) 
            {
                try
                {
                    AdministrarServicioLogic bAdServicio = new AdministrarServicioLogic();
                    AdmServices adServicios = new AdmServices();
                    adServicios.ServiciosDataGrid.ItemsSource = bAdServicio.insertar(txtNombre.Text, txtDescripcion.Text, decimal.Parse(txtCosto.Text), int.Parse(txtDuracion.Text)).DefaultView;
                    adServicios.ServiciosDataGrid.Items.Refresh();
                    MessageBox.Show("Se guardó el Registro ", "Seguridad");
                    this.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Falló el Registro ", "Seguridad" + ex.Message);
                }
            }
            else
                MessageBox.Show("Llena todas las casillas", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {


            if (txtNombre.Text != string.Empty && txtDescripcion.Text != string.Empty && txtCosto.Text != string.Empty && txtDuracion.Text != string.Empty)
            {
                try
                {
                    AdministrarServicioLogic bAdServicio = new AdministrarServicioLogic();
                    AdmServices adServicios = new AdmServices();
                    adServicios.ServiciosDataGrid.ItemsSource = bAdServicio.actualizar(int.Parse(txtId.Text), txtNombre.Text, txtDescripcion.Text, decimal.Parse(txtCosto.Text), int.Parse(txtDuracion.Text)).DefaultView;
                    adServicios.ServiciosDataGrid.Items.Refresh();
                    MessageBox.Show("Se actualizó el Registro ", "Seguridad" );
                    this.DialogResult = true;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Falló la actualización ", "Seguridad" + ex.Message);
                }
            }
            else
                MessageBox.Show("Llena todas las casillas", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
