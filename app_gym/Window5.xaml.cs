using DPFP;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace app_gym
{
    /// <summary>
    /// Lógica de interacción para Window5.xaml
    /// </summary>
    public partial class Window5 : Window
    {
    
        
        public Window5()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string usu = nombreusuario.Text;
            string con = contraseña.Password;
            

            string conexion = app_gym.Properties.Settings.Default.gymConnectionString1;
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();

                string query = "SELECT COUNT(*)  FROM registros_administradores WHERE nombre_usuario = @nombre_usuario AND contraseña = @contraseña";

                SqlCommand cmd = new SqlCommand(query, cn);


                cmd.Parameters.AddWithValue("@nombre_usuario", usu);
                cmd.Parameters.AddWithValue("@contraseña", con);
                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Inicio de sesión exitoso.");
                    App.UsuarioActual = usu;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Nombre de usuario o contraseña incorrectos.");
                }

                

            }
            


        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
