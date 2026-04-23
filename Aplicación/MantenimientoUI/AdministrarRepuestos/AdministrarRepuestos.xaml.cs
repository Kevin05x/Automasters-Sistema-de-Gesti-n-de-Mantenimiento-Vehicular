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
    /// Lógica de interacción para AdmRepuestos.xaml
    /// </summary>
    public partial class AdmRepuestos : UserControl
    {

        List<ComboBox> cmbProveedores = new();

        public AdmRepuestos()
        {
            InitializeComponent();
            llenarComboBox();
            llenarGrid();
            cbxFiltrarProveedor.SelectedItem = "Todos";
        }

        public void llenarComboBox()
        {
            cbxFiltrarProveedor.ItemsSource = new AdministrarRepuestosLogic().proveedores();
            cmbProveedores.Add(cbxFiltrarProveedor);
        }

        public void llenarGrid()
        {
            AdministrarRepuestosLogic repuestos = new AdministrarRepuestosLogic();
            dgListadoRepuestos.ItemsSource = repuestos.repuestos().DefaultView;
        }

        private void cbxFiltrarProveedor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = -1;

            if (cbxFiltrarProveedor.SelectedItem != null)
            {

                string selectedText = cbxFiltrarProveedor.SelectedItem.ToString();

                if (selectedText.Equals("Todos"))
                {
                    llenarGrid();
                    return;
                }


                foreach (char c in selectedText)
                {
                    if (char.IsDigit(c))
                    {
                        id = int.Parse(c.ToString());
                        break;
                    }
                }
            }

            AdministrarRepuestosLogic baja = new AdministrarRepuestosLogic();
            dgListadoRepuestos.ItemsSource = baja.filtrar(id).DefaultView;


        }

        private void btnAñadirRepuesto_Click(object sender, RoutedEventArgs e)
        {
            Registrar_ActualizarRpuesto regisrepuesto = new Registrar_ActualizarRpuesto(this);
            regisrepuesto.btnRegistrar.Visibility = Visibility.Visible;
            regisrepuesto.btnActualizar.Visibility = Visibility.Collapsed;

            regisrepuesto.ShowDialog();

            bool? resul = regisrepuesto.DialogResult;

            if (resul == true)
                llenarGrid();

        }

        private void btnActualizarRepuesto_Click(object sender, RoutedEventArgs e)
        {
            Registrar_ActualizarRpuesto regisrepuesto = new Registrar_ActualizarRpuesto(this);


            if (dgListadoRepuestos.SelectedItem != null)
            {
                var dataRowView = dgListadoRepuestos.SelectedItem as DataRowView;

                if (dataRowView != null)
                {
                    regisrepuesto.idre = Convert.ToInt32(dataRowView["Id_Repuesto"].ToString());
                    regisrepuesto.txtRepuestoActual.Text = (dataRowView["Id_Repuesto"].ToString());
                    regisrepuesto.idpro = Convert.ToInt32(dataRowView["Id_Proveedor"].ToString());
                    regisrepuesto.txtIdPro.Text = dataRowView["Id_Proveedor"].ToString();
                    regisrepuesto.txtNombre.Text = dataRowView["Nombre"].ToString();
                    regisrepuesto.txtDescripcion.Text = dataRowView["Descripcion"].ToString();
                    regisrepuesto.txtPrecio.Text = dataRowView["Precio_Unitario"].ToString();
                    regisrepuesto.txtStock.Text = dataRowView["Stock"].ToString();
                }

                regisrepuesto.btnRegistrar.Visibility = Visibility.Collapsed;
                regisrepuesto.btnActualizar.Visibility = Visibility.Visible;

                regisrepuesto.ShowDialog();

                bool? resul = regisrepuesto.DialogResult;

                if (resul == true)
                    llenarGrid();
            }

            else
            {
                MessageBox.Show("Seleccione un empleado a actualizar", "Seguridad");
            }
        }

        private void txtNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            AdministrarRepuestosLogic repuestos = new AdministrarRepuestosLogic();
            dgListadoRepuestos.ItemsSource = repuestos.buscar(txtNombre.Text).DefaultView;
        }

        
    }
}
