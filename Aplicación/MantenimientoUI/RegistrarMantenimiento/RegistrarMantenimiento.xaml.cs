using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace MantenimientoUI
{
    /// <summary>
    /// Lógica de interacción para RegMantenimiento.xaml
    /// </summary>
    public partial class RegMantenimiento : UserControl
    {
        List<ComboBox> cmbRepuestos = new();
        List<ComboBox> cmbServices = new();
        List<TextBox> txtCantRepuestos = new();
        List<string> nameRep; 
        List<string> nameServices;

        public RegMantenimiento()
        {
            InitializeComponent();
            nameRep = new GetRegManteLogic().nameRepuestos();
            nameServices = new GetRegManteLogic().nameServicios();
            LlenarInicioDatos();

        }

        private void AddPago_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddrep_Click(object sender, RoutedEventArgs e)
        {
            GetRegManteLogic dataGet = new GetRegManteLogic();

            ComboBox cb = new()
            {

                Width = 250,
                Height = 40,
                Margin = new Thickness(5),


            };

            TextBox txt = new()
            {
                Width = 100,
                Height = 40,
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };

            Button btn = new()
            {
                Width = 40,
                Height = 40,
                Content = "-",
                Margin = new Thickness(5),
                Foreground = Brushes.Red
            };

            btn.Click += (s, ev) =>
            {
                StackRep.Children.Remove(cb);
                StackCantidadRep.Children.Remove(txt);
                cmbRepuestos.Remove(cb);
                txtCantRepuestos.Remove(txt);
                StackEliminarCmb.Children.Remove(btn);
                lblTotal.Content = calcularPreciotot().ToString();
            };

            StackEliminarCmb.Children.Add(btn);
            StackRep.Children.Add(cb);
            StackCantidadRep.Children.Add(txt);
            cb.ItemsSource = nameRep;

            cmbRepuestos.Add(cb);
            txtCantRepuestos.Add(txt);
        }

        private void BtnAddServices_Click(object sender, RoutedEventArgs e)
        {
            GetRegManteLogic dataGet = new GetRegManteLogic();

            ComboBox cb = new()
            {

                Width = 150,
                Height = 40,
                Margin = new Thickness(5),


            };

            Button btn = new()
            {
                Width = 40,
                Height = 40,
                Content = "-",
                Margin = new Thickness(5),
                Foreground = Brushes.Red
            };

            btn.Click += (s, ev) =>
            {
                StackServices.Children.Remove(cb);
                cmbServices.Remove(cb);
                btnBorrarServicios.Children.Remove(btn);
                lblTotal.Content = calcularPreciotot().ToString();

            };

            btnBorrarServicios.Children.Add(btn);

            cb.SelectionChanged += Cb1Services_SelectionChanged;

            StackServices.Children.Add(cb);

            cb.ItemsSource = dataGet.nameServicios();
            cmbServices.Add(cb);



        }


        private void LlenarInicioDatos() {

            Cb1Services.ItemsSource = nameServices;
            Cb1Repuestos.ItemsSource = nameRep;

            cmbMecanicos.ItemsSource = new GetRegManteLogic().NombresMecanicos();
            cmbRepuestos.Add(Cb1Repuestos);
            txtCantRepuestos.Add(Tb1CantRep);
            cmbServices.Add(Cb1Services);


            int startYear = 1950;
            int endYear = DateTime.Now.Year;
            cmbAño.ItemsSource = Enumerable.Range(startYear, endYear - startYear + 1).Reverse();

            cmbCOlor.ItemsSource = new string[] { "Rojo", "Azul", "Verde", "Amarillo", "Blanco", "Negro", "Gris" };

            cmbTipo.ItemsSource = new string[] { "Camioneta", "Sedán", "SUV", "Hatchback", "Coupé", "Convertible", "Pickup", "Furgoneta", "Minivan", "Deportivo", "Todoterreno", "Moto", "Autobús", "Camión", "Tractor", "Remolque", "Bicicleta eléctrica", "Cuatrimoto", "Caravana", "Buggy" };
        }

        private void Cb1Services_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblTotal.Content = calcularPreciotot();
            ActualizarCmb();
        }


        private double calcularPreciotot() {

            double preciotot = 0;

            foreach (ComboBox combo in StackServices.Children)
            {
                if (combo.SelectedItem != null)
                {
                    preciotot += new GetRegManteLogic().precioServicio(combo.SelectedItem.ToString());
                }
            }


            int i= 0;
            foreach (ComboBox combo in StackRep.Children)
            {
                TextBox cant = ((TextBox)(StackCantidadRep.Children[i]));

                if (int.TryParse(cant.Text, out int cantidad) && combo.SelectedItem != null)
                {
                    preciotot += new GetRegManteLogic().precioRepuesto(combo.SelectedItem.ToString()) * cantidad;
                }

                i++;
            }


            return preciotot;

        }

        private async void btnRegistrarMant_Click(object sender, RoutedEventArgs e)
        {

            if (cmbMecanicos.SelectedItem == null || TbClientName.Text == string.Empty || TbTelProp.Text == string.Empty ||
                TbMarca.Text ==string.Empty || TbModelo.Text == string.Empty || TbPlaca.Text == string.Empty || 
                TbKilometraje.Text == string.Empty || cmbCOlor.SelectedItem == null || cmbAño.SelectedItem == null|| 
                cmbTipo.SelectedItem == null || DPFechaInicio.SelectedDate == null || TbDuracion.Text == string.Empty) { MessageBox.Show("Error, rellene los campos obligatorios"); return;  }
       
            string cliente = TbClientName.Text;
            string numeroCliente = TbTelProp.Text;

            
            string mecanico = cmbMecanicos.SelectedItem.ToString();

            string marca = TbMarca.Text;
            string modelo = TbModelo.Text;
            int año = int.Parse(cmbAño.SelectedItem.ToString());
            string placa = TbPlaca.Text;
            string color = cmbCOlor.SelectedItem.ToString();
            int kilometraje = int.Parse(TbKilometraje.Text);
            string tipo = cmbTipo.SelectedItem.ToString();



            Dictionary<string, int> repuestos = new();
            foreach (ComboBox cb in StackRep.Children) {


                int cantidad;
                if (int.TryParse(txtCantRepuestos[StackRep.Children.IndexOf(cb)].Text, out cantidad))
                {
                    if (!repuestos.TryAdd(cb.SelectedItem.ToString(), cantidad))
                    {
                        MessageBox.Show("Repuesto repetido, seleccione otro");
                    }
                }
                else
                {
                    MessageBox.Show("Cantidad inválida, por favor ingrese un número válido");
                }

            }

            DateTime fechaInicio = DPFechaInicio.SelectedDate.Value;
            int duracion = int.Parse(TbDuracion.Text);
            string estado = "En proceso";
            string observaciones = TbOservaciones.Text;
            double costoto = double.Parse(lblTotal.Content.ToString());

            List<string> servicios = new();
            foreach (ComboBox cb in StackServices.Children)
            {
                if (!servicios.Contains(cb.SelectedItem.ToString()))
                    servicios.Add(cb.SelectedItem.ToString());
            }

            RegistrarMantenimientoLogic regMantenimientoLogic = new();

            regMantenimientoLogic.RegAuto(cliente, numeroCliente, marca, modelo, año, placa, color, kilometraje, tipo);

            await regMantenimientoLogic.RegistrarMantenimiento(fechaInicio, duracion, estado, observaciones, costoto, servicios, mecanico, repuestos);

            MessageBox.Show("Registro completado satisfactoriamente");

            Limpiar();
        }


        // Permitir solo números en el TextBox
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Usar una expresión regular para verificar si la entrada es un número
            e.Handled = !EsNumero(e.Text);
        }

        // Manejar teclas especiales como retroceso
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Permitir teclas como Backspace, Delete, Tab, Enter, flechas, etc.
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab || e.Key == Key.Enter ||
                e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Escape)
            {
                e.Handled = false;
            }
        }

        // Validar si la entrada es un número
        private bool EsNumero(string texto)
        {
            return Regex.IsMatch(texto, @"^\d+$"); // Solo dígitos
        }

        private void Cb1Repuestos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizarCmb();

            lblTotal.Content = calcularPreciotot();
        }

        private void Tb1CantRep_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblTotal.Content = calcularPreciotot();
        }


        private void ActualizarCmb() {

            // SOLO FUNCIONA PARA CUANDO SE INSERTA EN ORDEN
            nameRep = new GetRegManteLogic().nameRepuestos();
            nameServices = new GetRegManteLogic().nameServicios();

            List<string> usadosRep = new List<string>();
            List<string> usadosServ = new List<string>();

            foreach (ComboBox cb in StackRep.Children) {

                if (cb.SelectedItem != null) usadosRep.Add(cb.SelectedItem.ToString());
            }
            foreach (ComboBox cb in StackServices.Children)
            {

                if (cb.SelectedItem != null) usadosServ.Add(cb.SelectedItem.ToString());
            }

            foreach (string st in usadosRep)
            {
                 nameRep.Remove(st);
            }

            foreach (string st in usadosServ)
            {
                nameServices.Remove(st);
            }
        }

        private void Limpiar()
        {
            TbClientName.Text = string.Empty;
            TbTelProp.Text = string.Empty;
            TbMarca.Text = string.Empty;
            TbModelo.Text = string.Empty;
            TbPlaca.Text = string.Empty;
            TbKilometraje.Text = string.Empty;
            TbDuracion.Text = string.Empty;
            TbOservaciones.Text = string.Empty;
            lblTotal.Content = "0";

            TbClientName.Focus();

            cmbMecanicos.SelectedIndex = -1;
            cmbAño.SelectedIndex = -1;
            cmbCOlor.SelectedIndex = -1;
            cmbTipo.SelectedIndex = -1;
            Cb1Repuestos.SelectedIndex = -1;
            Cb1Services.SelectedIndex = -1;
            DPFechaInicio.SelectedDate = null;

            Tb1CantRep.Text = string.Empty;

            while (cmbRepuestos.Count > 1)
            {
                StackRep.Children.Remove(cmbRepuestos.Last());
                cmbRepuestos.RemoveAt(cmbRepuestos.Count - 1);
            }

            while (txtCantRepuestos.Count > 1)
            {
                StackCantidadRep.Children.Remove(txtCantRepuestos.Last());
                txtCantRepuestos.RemoveAt(txtCantRepuestos.Count - 1);
            }

            while (cmbServices.Count > 1)
            {
                StackServices.Children.Remove(cmbServices.Last());
                cmbServices.RemoveAt(cmbServices.Count - 1);
            }

            while (StackEliminarCmb.Children.Count > 1)
            {
                StackEliminarCmb.Children.RemoveAt(StackEliminarCmb.Children.Count - 1);
            }


            while (btnBorrarServicios.Children.Count > 1) 
            {
                btnBorrarServicios.Children.RemoveAt(btnBorrarServicios.Children.Count - 1);
            }
        }

    }

}
