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
using MantenimientoUI.VentanasDer;
using Microsoft.IdentityModel.Tokens;

namespace MantenimientoUI.VentanasEmer
{
    /// <summary>
    /// Lógica de interacción para Registrar_ActualizarRpuesto.xaml
    /// </summary>
    public partial class Registrar_ActualizarRpuesto : Window
    {
        private AdmRepuestos refevenori;
        List<ComboBox> cmbProveedores = new();
        public int idpro;
        public int idre;
        public Registrar_ActualizarRpuesto()
        {
            InitializeComponent();
            llenarComboBox();
        }

        public void llenarComboBox()
        {
            cbxProveedor.ItemsSource = new AdministrarRepuestosLogic().proveedores();
            cmbProveedores.Add(cbxProveedor);
        }

        public Registrar_ActualizarRpuesto(AdmRepuestos admRepuestos)
        {
            InitializeComponent();
            refevenori = admRepuestos;
            llenarComboBox();
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (cbxProveedor.Text != string.Empty && txtNombre.Text != string.Empty && txtPrecio.Text != string.Empty && txtStock.Text != string.Empty)
            {
                try
                {
                    AdministrarRepuestosLogic bRepuestos = new AdministrarRepuestosLogic();
                    string textoSeleccionado = cbxProveedor.SelectedItem.ToString();
                    string primerValor = textoSeleccionado.Split('-')[0];
                    int idpro = Convert.ToInt32(primerValor);

                    string descripcion = txtDescripcion.Text.IsNullOrEmpty() ? "No especificado": txtDescripcion.Text;

                    AdmRepuestos admRepuestos = new AdmRepuestos();
                    admRepuestos.dgListadoRepuestos.ItemsSource = bRepuestos.insertar(idpro, txtNombre.Text, descripcion, Convert.ToDecimal(txtPrecio.Text), Convert.ToInt32(txtStock.Text)).DefaultView;

                    MessageBox.Show("Se realizó correctamente la insercción", "Seguridad");
                    refevenori.llenarGrid();
                    this.Close();
                    
                }
                catch
                {
                    MessageBox.Show("Falló la insercción", "Seguridad");
                }
            }
            else
                MessageBox.Show("Llena todas las casillas", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);



            

        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (cbxProveedor.Text != string.Empty && txtNombre.Text != string.Empty && txtPrecio.Text != string.Empty && txtStock.Text != string.Empty)
            {
                try
                {
                    AdmRepuestos admRepuestos = new AdmRepuestos();
                    AdministrarRepuestosLogic bRepuestos = new AdministrarRepuestosLogic();

                    string descripcion = txtDescripcion.Text.IsNullOrEmpty() ? "No especificado" : txtDescripcion.Text;

                    admRepuestos.dgListadoRepuestos.ItemsSource = bRepuestos.actualizar(idpro, txtNombre.Text, descripcion, Convert.ToDecimal(txtPrecio.Text), Convert.ToInt32(txtStock.Text), idre).DefaultView;

                    MessageBox.Show("Se realizó correctamente la actualización del Repuesto", "Seguridad");
                    refevenori.llenarGrid();
                    this.Close();
                    
                }

                catch
                {
                    MessageBox.Show("Falló la actualización del Repuesto", "Seguridad");
                }
            }
            else
                MessageBox.Show("Llena todas las casillas", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

            
        }
    }
}
