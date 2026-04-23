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
    /// Lógica de interacción para AdmEmployees.xaml
    /// </summary>
    public partial class AdmEmployees : UserControl
    {
        public AdmEmployees()
        {
            InitializeComponent();
            cargarDataGrid();
            string[] tipos = {"Todos", "Administrativo", "Secretario", "Mecánico" };
            for (int i = 0; i < tipos.Length; i++)
            {
                cbxFiltrarEmp.Items.Add(tipos[i]);
            }
            cbxFiltrarEmp.SelectedItem = "Todos";
        }

        public void cargarDataGrid()
        {
            AdministrarEmpleadoLogic bAdempleado = new AdministrarEmpleadoLogic();
            dgListadoEmpleado.ItemsSource = bAdempleado.listaEmpleado().DefaultView;
        }

        private void cbxFiltrarEmp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string contenido = "Todos";

                if (cbxFiltrarEmp.SelectedItem != null)
                {
                    contenido = cbxFiltrarEmp.SelectedItem.ToString();
                }

                if (!contenido.Equals("Todos"))
                {
                    AdministrarEmpleadoLogic baja = new AdministrarEmpleadoLogic();
                    dgListadoEmpleado.ItemsSource = baja.filtrarempleado(contenido).DefaultView;
                }
                else
                    cargarDataGrid();
                

            }
            catch
            {
                MessageBox.Show("Error en filtrar los datos", "Seguridad");
            }
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            
            Registrar_ActualizarEmpleado registrar_Actualizar = new Registrar_ActualizarEmpleado(this);

            registrar_Actualizar.btnRegistrar.Visibility = Visibility.Visible;
            registrar_Actualizar.btnActualizar.Visibility = Visibility.Collapsed;

            registrar_Actualizar.ShowDialog();

            bool? result = registrar_Actualizar.DialogResult;

            if (result == true) {

                cargarDataGrid();
            }
            
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            Registrar_ActualizarEmpleado registrar_Actualizar = new Registrar_ActualizarEmpleado(this);


            if (dgListadoEmpleado.SelectedItem != null)
            {
                var dataRowView = dgListadoEmpleado.SelectedItem as DataRowView;

                if (dataRowView != null)
                {
                    registrar_Actualizar.txtEmpleadoActual.Text = (dataRowView["Id_Empleado"].ToString());
                    registrar_Actualizar.id = Convert.ToInt32(dataRowView["Id_Empleado"].ToString());
                    registrar_Actualizar.txtNombre.Text = dataRowView["Nombre"].ToString();
                    registrar_Actualizar.txtApellidos.Text = dataRowView["Apellido"].ToString();
                    registrar_Actualizar.txtTelefono.Text = dataRowView["Telefono"].ToString();
                    registrar_Actualizar.txtSalario.Text = dataRowView["Salario"].ToString();
                    registrar_Actualizar.txtUsuario.Text = dataRowView["Usuario"].ToString();
                    registrar_Actualizar.txtPassword.Text = dataRowView["Password"].ToString();
                    registrar_Actualizar.cbxFiltrarEmp.Text = dataRowView["Tipo"].ToString();
                }

                registrar_Actualizar.btnRegistrar.Visibility = Visibility.Collapsed;
                registrar_Actualizar.btnActualizar.Visibility = Visibility.Visible;

                registrar_Actualizar.ShowDialog();

                bool? result = registrar_Actualizar.DialogResult;

                if (result == true)
                {

                    cargarDataGrid();
                }
            }

            else
            {
                MessageBox.Show("Seleccione un empleado a actualizar", "Seguridad");
            }
        }


    }

}
