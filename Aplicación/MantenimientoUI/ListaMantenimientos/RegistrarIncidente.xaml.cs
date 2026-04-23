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

namespace MantenimientoUI.VentanasEmer
{
    /// <summary>
    /// Lógica de interacción para RegistrarIncidenteMantenimiento.xaml
    /// </summary>
    public partial class RegistrarIncidenteMantenimiento : Window
    {
        int Id = -1;
        public RegistrarIncidenteMantenimiento()
        {
            InitializeComponent();
        }

        public void IdMantenimiento(int Id)
        {
            this.Id = Id;
        }

        private void bttnRegistrarIncidente_Click(object sender, RoutedEventArgs e)
        {
            if (tbxDescripcion.Text != string.Empty && tbxSolucion.Text != string.Empty)
            {
                ListaMantenimientoLogic logic = new ListaMantenimientoLogic();
                logic.registrarIncidente(Id, tbxDescripcion.Text, tbxSolucion.Text);
                MessageBox.Show("Se registró correctamente el incidente", "Seguridad", MessageBoxButton.OK);
                this.Close();
            }
            else
                MessageBox.Show("Llena todas las casillas", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);


            
        }

    }
}
