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
    /// Lógica de interacción para Registrar_ActualizarProveedor.xaml
    /// </summary>
    public partial class Registrar_ActualizarProveedor : Window
    {
        public Registrar_ActualizarProveedor()
        {
            InitializeComponent();
        }

        public Registrar_ActualizarProveedor(int id)
        {
            InitializeComponent();
            txtId.Text = Convert.ToString(id);
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (txtNombre.Text != string.Empty && txtDireccion.Text != string.Empty && txtTelefono.Text != string.Empty && txtCorreo.Text != string.Empty && txtRUC.Text != string.Empty)
            {
                try
                {
                    AdministrarProveedorLogic bAdProveedor = new AdministrarProveedorLogic();
                    AdmSuppliers admProveedores = new AdmSuppliers();
                    admProveedores.ProveedoresDataGrid.ItemsSource = bAdProveedor.insertar(txtNombre.Text, txtDireccion.Text, txtTelefono.Text, txtCorreo.Text, txtRUC.Text).DefaultView;
                    admProveedores.ProveedoresDataGrid.Items.Refresh();
                    MessageBox.Show("Se guardó el Registro ","Seguridad");
                    this.DialogResult = true;
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
            if (txtNombre.Text != string.Empty && txtDireccion.Text != string.Empty && txtTelefono.Text != string.Empty && txtCorreo.Text != string.Empty && txtRUC.Text != string.Empty)
            {
                try
                {
                    AdministrarProveedorLogic bAdProveedor = new AdministrarProveedorLogic();
                    AdmSuppliers admProveedores = new AdmSuppliers();
                    admProveedores.ProveedoresDataGrid.ItemsSource = bAdProveedor.actualizar(int.Parse(txtId.Text), txtNombre.Text, txtDireccion.Text, txtTelefono.Text, txtCorreo.Text, txtRUC.Text).DefaultView;
                    admProveedores.ProveedoresDataGrid.Items.Refresh();
                    MessageBox.Show("Se actualizó el Registro ",  "Seguridad");
                    this.DialogResult = true;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Falló la actualización ","Seguridad"  + ex.Message);
                }
            }
            else
                MessageBox.Show("Llena todas las casillas", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

        }
    }
}