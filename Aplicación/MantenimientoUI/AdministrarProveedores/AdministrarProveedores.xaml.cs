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
    /// Lógica de interacción para AdmProveedores.xaml
    /// </summary>
    public partial class AdmSuppliers : UserControl
    {
        public AdmSuppliers()
        {
            InitializeComponent();
            cargarDataGrid();
            
        }

        public void cargarDataGrid()
        {
            AdministrarProveedorLogic bAdProveedor = new AdministrarProveedorLogic();
            ProveedoresDataGrid.ItemsSource = bAdProveedor.listaProveedores().DefaultView;
        }

        

        private void btnAñadirProveedor_Click(object sender, RoutedEventArgs e)
        {
            
    
            Registrar_ActualizarProveedor registrar_Actualizar = new Registrar_ActualizarProveedor();
            
            registrar_Actualizar.btnRegistrar.Visibility = Visibility.Visible;
            registrar_Actualizar.btnActualizar.Visibility = Visibility.Collapsed;
            registrar_Actualizar.ShowDialog();

            bool? result = registrar_Actualizar.DialogResult;

            if (result == true) {

                cargarDataGrid();
            }
        }

        private void btnActualizarProveedor_Click(object sender, RoutedEventArgs e)
        {
            if (ProveedoresDataGrid.SelectedItem == null) { MessageBox.Show("Por favor seleccione una fila"); return; }
            
            var dataRowView = ProveedoresDataGrid.SelectedItem as DataRowView;

            int id = Convert.ToUInt16(dataRowView["Id_Proveedor"].ToString());

            Registrar_ActualizarProveedor registrar_Actualizar = new Registrar_ActualizarProveedor(id);

            registrar_Actualizar.btnRegistrar.Visibility = Visibility.Collapsed;
            registrar_Actualizar.btnActualizar.Visibility = Visibility.Visible;

            registrar_Actualizar.txtNombre.Text = dataRowView["Nombre"].ToString();
            registrar_Actualizar.txtCorreo.Text = dataRowView["Correo"].ToString();
            registrar_Actualizar.txtDireccion.Text = dataRowView["Direccion"].ToString();
            registrar_Actualizar.txtTelefono.Text = dataRowView["Telefono"].ToString();
            registrar_Actualizar.txtRUC.Text = dataRowView["RUC"].ToString();
            registrar_Actualizar.ShowDialog();

            bool? result = registrar_Actualizar.DialogResult;

            if (result == true)
            {
                cargarDataGrid();
            }
        }

        private void txtNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            AdministrarProveedorLogic bAdProveedor = new AdministrarProveedorLogic();
            ProveedoresDataGrid.ItemsSource = bAdProveedor.BuscarProv(txtNombre.Text).DefaultView;
        }
    }
}