using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void guardar_plan_Click(object sender, RoutedEventArgs e)
        {
            string NP = nombre_plan.Text;
            string C = costo_plan.Text;
            string D = duracion_plan.Text;

            if (string.IsNullOrEmpty(NP) || string.IsNullOrEmpty(C) || string.IsNullOrEmpty(D))
            {
                MessageBox.Show("Por favor, llene los datos.");
                return;
            }

            string conexion = app_gym.Properties.Settings.Default.gymConnectionString1;
            miconexion = new SqlConnection(conexion);

            string insertar = "INSERT INTO membresias (nombre_membresia, costo, duracion_dias) VALUES (@nombre_membresia, @costo, @duracion_dias)";

            SqlCommand insertar2 = new SqlCommand(insertar, miconexion);

            




            try
            {
                miconexion.Open();
                insertar2.Parameters.AddWithValue("@nombre_membresia", NP);
                insertar2.Parameters.AddWithValue("@costo", C);
                insertar2.Parameters.AddWithValue("@duracion_dias", D);
                insertar2.ExecuteNonQuery();
                MessageBox.Show("Plan guardado exitosamente.");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            miconexion.Close();
        }
        SqlConnection miconexion;
    }
}
