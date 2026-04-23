using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
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
using MantenimientoUI.VentanasDer;

namespace MantenimientoUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        

        public MainWindow()
        {
            InitializeComponent();

            InicioSesion();
            new LogicaGeneral().actualizarEstadoMantenimiento();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GrdVistaDer.Children.Clear();
            LogoFondoAutomasters.Opacity = 0.1;

            switch (((Button)sender).Content) {

                case "Registrar Mantenimiento":
                    RegMantenimiento regMantenimiento = new();
                    GrdVistaDer.Children.Add(regMantenimiento);
                    return;

           
                case "Lista de Mantenimientos":
                    ListaMantenimiento listaMantenimiento = new();
                    GrdVistaDer.Children.Add(listaMantenimiento);

                    return;

                case "Administrar empleados":

                    AdmEmployees admEmployees = new();
                    GrdVistaDer.Children.Add((admEmployees));

                    return;

                case "Administrar repuestos":
                    AdmRepuestos admRepuestos = new();
                    GrdVistaDer.Children.Add(admRepuestos);

                    return;


                case "Administrar servicios":
                    AdmServices admServices = new();
                    GrdVistaDer.Children.Add(admServices);
                    return;

                case "Administrar proveedores":
                    AdmSuppliers admSuppliers = new();
                    GrdVistaDer.Children.Add(admSuppliers);
                    return;
            }


        }

        private void InicioSesion() {

            this.Hide();
            IniciarSesion iniciarSesion = new();
            bool? result = iniciarSesion.ShowDialog();
            
           
            if (result==true) {

                lblUserName.Content = new InicioSesionLogic().NombreEmpl(iniciarSesion.TBUser.Text);

                switch (iniciarSesion.QuienEntro)
                {

                    case 2:
                        this.Show();
                        iniciarSesion.Close();
                        return;

                    case 3:

                        btnOcul1.Visibility = Visibility.Collapsed;
                        btnOcul2.Visibility = Visibility.Collapsed;
                        btnOcul3.Visibility = Visibility.Collapsed;
                        btnOcul4.Visibility = Visibility.Collapsed;
                        this.Show();
                        iniciarSesion.Close();
                        return;

                    case 4:
                        MessageBox.Show("El rol de mecánico no tiene acceso a esta ventana principal.");
                        iniciarSesion.Close();
                        Application.Current.Shutdown(); // Cierra la aplicación
                        return;

                    default:
                        iniciarSesion.Close();
                        Application.Current.Shutdown();
                        return;

                }

            }

            else
            {
                Application.Current.Shutdown();
            }

        }

        private void CerrarAppClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }

        private void CerrarSesClick(object sender, RoutedEventArgs e)
        {
            // Obtén el nombre del archivo ejecutable de la aplicación actual
            string nombreArchivo = Process.GetCurrentProcess().MainModule.FileName;

            // Cierra la aplicación actual
            Application.Current.Shutdown();

            // Inicia una nueva instancia de la aplicación
            Process.Start(nombreArchivo);
        }

    }
}