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

namespace MantenimientoUI
{
    /// <summary>
    /// Lógica de interacción para IniciarSesion.xaml
    /// </summary>
    public partial class IniciarSesion : Window
    {

        private int quienEntro = -1;

        public IniciarSesion()
        {
            InitializeComponent();
            TBUser.Text = "RodriguezJR54321";
            PBUser.Password = "UVWccDef78901za";

        }

        public int QuienEntro { get => quienEntro; set => quienEntro = value; }

        private void BtnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
           InicioSesionLogic loginLogic  = new();

            switch (loginLogic.detectarUsuario(TBUser.Text,PBUser.Password)) {


                case -1:
                    MessageBox.Show("Usuario Incorrecto", "Seguridad", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    quienEntro = -1;
                    return;
                case 0:
                    MessageBox.Show("Rellene el formulario porfavor", "Seguridad", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    quienEntro = 0;
                    return;

                case 1:
                    MessageBox.Show("Contraseña incorrecta", "Seguridad", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    quienEntro = 1;
                    return;

                case 2:
                    MessageBox.Show("Accediste como Administrativo", "Seguridad", MessageBoxButton.OK, MessageBoxImage.Information);
                    quienEntro = 2;
                    this.DialogResult = true;
                    return;

                case 3:
                    MessageBox.Show("Accediste como Secretario", "Seguridad", MessageBoxButton.OK, MessageBoxImage.Information);
                    quienEntro = 3;
                    this.DialogResult = true;
                    return;

                default:

                    return;
            }
        }
    }
}
