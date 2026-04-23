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
    /// Lógica de interacción para RegistrarPago.xaml
    /// </summary>
    public partial class RegistrarPago : Window
    {
        string[] listaTipoPago = { "Al contado", "Adelanto" };
        double montoPendiente = 0;

        int Id = -1;
        public RegistrarPago()
        {
            InitializeComponent();
            tbxMontoAsignado.Text = "0";
        }

        public void IdMatenimiento(int Id, double monto)
        {
            this.Id = Id;
            ListaMantenimientoLogic listaMantenimientoLogic = new ListaMantenimientoLogic();
            DataTable detallePago = listaMantenimientoLogic.detallePago(Id);

            tbxMonto.Text = monto.ToString();

            if (detallePago.Rows.Count == 0)
                pagoAlContadoAdelanto();
            else
            {
                double adelanto = double.Parse(detallePago.Rows[0]["Monto"].ToString());
                pagoPendiente(monto, adelanto);
            }
        }

        private void cmbxTipoPago_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string seleccionado = comboBox.SelectedItem as string;


            if (seleccionado.Equals("Adelanto"))
            {
                lbTipoPago.Visibility = Visibility.Visible;
                tbxMontoAsignado.Visibility = Visibility.Visible;
                lbTipoPago.Content = "Adelanto: S/.";
                tbxMontoAsignado.Text = string.Empty;
                tbxMontoAsignado.Focus();
            }
            else
            {
                lbTipoPago.Visibility = Visibility.Collapsed;
                tbxMontoAsignado.Visibility = Visibility.Collapsed;
                tbxMontoAsignado.Text = tbxMonto.Text;
            }

        }

        private void pagoAlContadoAdelanto()
        {
            cmbxTipoPago.ItemsSource = listaTipoPago;
            cmbxTipoPago.SelectedItem = "Al contado";

        }

        private void pagoPendiente(double monto, double adelanto)
        {
            cmbxTipoPago.Items.Add("Pago pendiente");
            cmbxTipoPago.SelectedItem = "Pago pendiente";
            lbTipoPago.Visibility = Visibility.Visible;
            tbxMontoAsignado.Visibility = Visibility.Visible;
            lbTipoPago.Content = "Pago pendiente: S/.";
            cmbxTipoPago.IsEnabled = false;
            montoPendiente = monto - adelanto;
            tbxMontoAsignado.Text = (montoPendiente).ToString();
            tbxMontoAsignado.IsEnabled = false;
        }

        private void bttnRegistrarPago_Click(object sender, RoutedEventArgs e)
        {
            if (cmbxMetodoPago.Text != string.Empty && cmbxTipoPago.Text != string.Empty && tbxMontoAsignado.Text != string.Empty)
            {
                if (cmbxTipoPago.Text.Equals("Adelanto") && (double.Parse(tbxMonto.Text) <= double.Parse(tbxMontoAsignado.Text) || double.Parse(tbxMonto.Text) / 2 > double.Parse(tbxMontoAsignado.Text)))
                {
                    MessageBox.Show("El monto asignado no es admisible (No puede ser mayor al monto total, no puede ser menor a la mitad del monto total)", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                ListaMantenimientoLogic logic = new ListaMantenimientoLogic();

                logic.registrarPago(Id, cmbxTipoPago.Text, double.Parse(tbxMontoAsignado.Text), cmbxMetodoPago.Text);

                MessageBox.Show("Pago registrado correctamente", "Aviso", MessageBoxButton.OK);
                this.Close();

            }
            else
                MessageBox.Show("Llenar todos los datos", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
