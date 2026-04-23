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
    /// Lógica de interacción para Registrar_ActualizarEmpleado.xaml
    /// </summary>
    public partial class Registrar_ActualizarEmpleado : Window
    {
        public int id;
        private AdmEmployees refvenori;

        public Registrar_ActualizarEmpleado()
        {
            InitializeComponent();

            string[] tipos = { "Administrativo", "Secretario", "Mecánico" };
            for (int i = 0; i < tipos.Length; i++)
            {
                cbxFiltrarEmp.Items.Add(tipos[i]);
            }
        }

        public Registrar_ActualizarEmpleado(AdmEmployees admEmployees)
        {
            InitializeComponent();
            refvenori = admEmployees;
            string[] tipos = { "Administrativo", "Secretario", "Mecánico" };
            for (int i = 0; i < tipos.Length; i++)
            {
                cbxFiltrarEmp.Items.Add(tipos[i]);
            }

        }

        

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (txtNombre.Text != string.Empty && txtApellidos.Text != string.Empty && txtTelefono.Text != string.Empty && txtSalario.Text != string.Empty && txtUsuario.Text != string.Empty && txtPassword.Text != string.Empty && cbxFiltrarEmp.Text != string.Empty)
            {
                try
                {
                    AdministrarEmpleadoLogic bAdempleado = new AdministrarEmpleadoLogic();
                    AdmEmployees adEmpleados = new AdmEmployees();
                    adEmpleados.dgListadoEmpleado.ItemsSource = bAdempleado.insertar(txtNombre.Text, txtApellidos.Text, txtTelefono.Text, txtSalario.Text, txtUsuario.Text, txtPassword.Text, cbxFiltrarEmp.Text).DefaultView;
                    refvenori.cargarDataGrid();
                    this.Close();

                    MessageBox.Show("Se guardó el Registro", "Seguridad");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Fallo el Registro", "Seguridad" + ex.ToString());
                }
            }
            else
                MessageBox.Show("Llena todas las casillas", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);


        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (txtNombre.Text != string.Empty && txtApellidos.Text != string.Empty && txtTelefono.Text != string.Empty && txtSalario.Text != string.Empty && txtUsuario.Text != string.Empty && txtPassword.Text != string.Empty && cbxFiltrarEmp.Text != string.Empty)
            {
                try
                {

                    AdministrarEmpleadoLogic actualizar = new AdministrarEmpleadoLogic();
                    AdmEmployees admEmployees = new AdmEmployees();
                    actualizar.actualizar(id, txtNombre.Text, txtApellidos.Text, txtTelefono.Text, Convert.ToDecimal(txtSalario.Text), txtUsuario.Text, txtPassword.Text, cbxFiltrarEmp.Text);
                    refvenori.cargarDataGrid();
                    this.Close();

                    MessageBox.Show("Se actualizó exitosamente", "Seguridad" + txtApellidos.Text + id);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error en la actualización", "Seguridad" + ex.ToString());
                }
            }
            else
                MessageBox.Show("Llena todas las casillas", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);



        }

        private void cbxFiltrarEmp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBoxItem = sender as ComboBox;
            string seleccionado = comboBoxItem.SelectedItem.ToString();

            if (seleccionado.Equals("Mecánico"))
            {
                txtUsuario.Text = txtPassword.Text = "Ninguno";
                txtUsuario.IsEnabled = txtPassword.IsEnabled = false;
            }
            else
            {
                txtUsuario.Text = txtPassword.Text = string.Empty;
                txtUsuario.IsEnabled = txtPassword.IsEnabled = true;
            }
        }
    }
}
