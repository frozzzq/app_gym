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

namespace app_gym
{
    /// <summary>
    /// Lógica de interacción para Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
            EsconderVentanaPrincipal();
            

        }

        public static void EsconderVentanaPrincipal()
        {
            Application.Current.MainWindow.Hide();
        }



        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (RB1.IsChecked == true)
            {

                registrar_cliente.Navigate(new Page1());
            }
        }

        private void RB2_Checked(object sender, RoutedEventArgs e)
        {
            if (RB3.IsChecked == true)
            {
                registrar_cliente.Navigate(new Page2());
            }
        }



        private void RB4_Checked(object sender, RoutedEventArgs e)
        {
            if (RB4.IsChecked == true)
            {
                registrar_cliente.Navigate(new Page3());
            }
        }

        private void RBINICIO_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void RB5_Checked(object sender, RoutedEventArgs e)
        {
            if (RB5.IsChecked == true)
            {
                registrar_cliente.Navigate(new Pinicio());
            }
        }
    }
}
