using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Lógica de interacción para iniciar_sesion.xaml
    /// </summary>
    public partial class iniciar_sesion : Window
    {
        public iniciar_sesion()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string usuario = nombreusuario.Text;
            string email = correo.Text;
            string contra = contraseña.Password;
            string Ccontra = Ccontraseña.Password;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(contra) || string.IsNullOrEmpty(Ccontra))
            {
                MessageBox.Show("Por favor, llene todos los campos.");
                return;
            }
            if (contra != Ccontra)
            {
                MessageBox.Show("las contraseñas no coinciden, porfavor ingrese los datos correctos");
            }
            else
            {
                string conexion = app_gym.Properties.Settings.Default.gymConnectionString1;
                miconexion = new SqlConnection(conexion);

                string insertar = "INSERT INTO registros_administradores (nombre_usuario, correo_admin, contraseña) VALUES (@nombre_usuario, @correo_admin, @contraseña)";
                SqlCommand insertar2 = new SqlCommand(insertar, miconexion);

                try
                {
                    miconexion.Open();
                    insertar2.Parameters.AddWithValue("@nombre_usuario", usuario);
                    insertar2.Parameters.AddWithValue("@correo_admin", email);
                    insertar2.Parameters.AddWithValue("@contraseña", Ccontra);
                    insertar2.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                miconexion.Close();
                MessageBox.Show("Usuario registrado exitosamente.");

            }


        }
        SqlConnection miconexion;

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void True(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
