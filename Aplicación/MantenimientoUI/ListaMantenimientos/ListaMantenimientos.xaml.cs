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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BusinessLogic;
using MantenimientoUI.VentanasEmer;

namespace MantenimientoUI.VentanasDer
{
    /// <summary>
    /// Lógica de interacción para ListaMantenimiento.xaml
    /// </summary>
    public partial class ListaMantenimiento : UserControl
    {
        private DetalleMantenimiento detalleMantenimiento;
        private RegistrarIncidenteMantenimiento registrarIncidente;
        private RegistrarPago registrarPagoMantenimiento;
        public ListaMantenimiento()
        {
            InitializeComponent();
            cargarDataGrid();
            cmbxFiltro.SelectedItem = "Todos";
        }

        private void cargarDataGrid()
        {
            ListaMantenimientoLogic lista = new ListaMantenimientoLogic();
            DGdMantenimientos.ItemsSource = lista.mantenimientos().DefaultView;

            cmbxFiltro.SelectedIndex = 0;
        }

        private void cmbxFiltro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
            string contenido = "Todos";

            if (selectedItem != null)
            {
                contenido = selectedItem.Content.ToString();
            }
            ListaMantenimientoLogic lista = new ListaMantenimientoLogic();


            if (contenido.Equals("Todos"))
            {
                DGdMantenimientos.ItemsSource = lista.mantenimientos().DefaultView;
            }

            if (contenido.Equals("Pagado") || contenido.Equals("Pendiente de pago"))
            {
                int pagado = contenido.Equals("Pagado") ? 1 : 0;
                DGdMantenimientos.ItemsSource = lista.mantenimientos(pagado, string.Empty).DefaultView;

            }

            if (contenido.Equals("Completado") || contenido.Equals("En proceso"))
            {
                DGdMantenimientos.ItemsSource = lista.mantenimientos(-1, contenido).DefaultView;
            }
        }

        private void DGdMantenimientos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DGdMantenimientos.SelectedItem is DataRowView filaSeleccionada)
            {
                int id = int.Parse(filaSeleccionada["Id_Mantenimiento"].ToString());


                if (detalleMantenimiento != null)
                    detalleMantenimiento.Close();

                detalleMantenimiento = new DetalleMantenimiento();
                detalleMantenimiento.llenarDatos(id);
                detalleMantenimiento.Show();

            }
        }

        private void bttnRegistrarIncidente_Click(object sender, RoutedEventArgs e)
        {
            if (DGdMantenimientos.SelectedItem is DataRowView row)
            {
                int Id = int.Parse(row["Id_Mantenimiento"].ToString());

                if (registrarIncidente != null)
                    registrarIncidente.Close();

                registrarIncidente = new RegistrarIncidenteMantenimiento();

                registrarIncidente.IdMantenimiento(Id);

                registrarIncidente.Closed += (s, e) => cargarDataGrid();
                registrarIncidente.ShowDialog();
                bool? resul = registrarIncidente.DialogResult;

                if (resul == true)
                    cargarDataGrid();
            }
            else
                MessageBox.Show("Selecciona un mantenimiento", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        private void bttnRegistrarPago_Click(object sender, RoutedEventArgs e)
        {
            if (DGdMantenimientos.SelectedItem is DataRowView row)
            {
                int Id = int.Parse(row["Id_Mantenimiento"].ToString());
                double monto = double.Parse(row["Costo_Total"].ToString());

                if (row["Pagado"].ToString().Equals("Si"))
                {
                    MessageBox.Show("El mantenimiento seleccionado ya fue pagado", "Aviso", MessageBoxButton.OK);
                }
                else
                {
                    if (registrarPagoMantenimiento != null)
                        registrarPagoMantenimiento.Close();

                    registrarPagoMantenimiento = new RegistrarPago();
                    registrarPagoMantenimiento.IdMatenimiento(Id, monto);

                    registrarPagoMantenimiento.Closed += (s, e) => cargarDataGrid();
                    registrarPagoMantenimiento.ShowDialog();
                }
            }
            else
                MessageBox.Show("Selecciona un mantenimiento", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        
    }
}
