using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
    /// Lógica de interacción para Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        public Window4()
        {
            InitializeComponent();
            CargarPlanes();
        }
        private string conexion = app_gym.Properties.Settings.Default.gymConnectionString1;

        private void CargarPlanes()
        {
            using (SqlConnection con = new SqlConnection(conexion))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT id_membresia, nombre_membresia FROM membresias", con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                seleccionar_plan.ItemsSource = dt.DefaultView;
            }
        }
        private void seleccionarPlanes(object sender, SelectionChangedEventArgs e)
        {
            if (seleccionar_plan.SelectedValue == null)
            {
                return;
            }

            int id = Convert.ToInt32(seleccionar_plan.SelectedValue);

            using (SqlConnection con = new SqlConnection(conexion))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT nombre_membresia, costo, duracion_dias FROM membresias WHERE id_membresia = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    NP.Text = reader["nombre_membresia"].ToString();
                    CP.Text = reader["costo"].ToString();
                    DP.Text = reader["duracion_dias"].ToString();
                }
            }

            
        }

        private void guardar_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(seleccionar_plan.SelectedValue);


            using (SqlConnection con = new SqlConnection(conexion))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM membresias WHERE id_membresia = @id",
                    con);

                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("plan eliminado correctamente.");
        }
    }
}
