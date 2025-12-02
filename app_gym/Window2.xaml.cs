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
        
        public static string usuarioActual1 = "";
        
        public Window2()
        {
            InitializeComponent();
            EsconderVentanaPrincipal();

            Window5 loginWindow = new Window5();
            loginWindow.ShowDialog();


            usuarioactual.Text = $"Usuario: {App.UsuarioActual}";
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

        private void RB5_Checked(object sender, RoutedEventArgs e)
        {
            if (RB5.IsChecked == true)
            {
                registrar_cliente.Navigate(new Pinicio());
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            iniciar_sesion registro = new iniciar_sesion();
            registro.Show();
        }

        private void RBINICIO_Checked_1(object sender, RoutedEventArgs e)
        {
            if (RBINICIO.IsChecked == true)
            {
                registrar_cliente.Navigate(new VentanaInicio());
            }

        }

        private void inicio_sesion_Click(object sender, RoutedEventArgs e)
        {
            Window5 LogIn = new Window5();
            LogIn.Show();
            
        }
    }
}
