using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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


namespace app_gym
{
    /// <summary>
    /// Lógica de interacción para Pinicio.xaml
    /// </summary>
    public partial class Pinicio : Page
    {
        public Pinicio()
        {
            InitializeComponent();
            conexion_api();
        }

        private void queja_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private Socket cliente;
        private void conexion_api()
        {
            string IP = "25.4.244.203";
            int puerto = 5000;

            cliente = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                cliente.Connect(new IPEndPoint(IPAddress.Parse(IP), puerto));
                estado_conexion.Text = "Conectado al servidor socket.";
            }
            catch (Exception ex)
            {
                estado_conexion.Text = "Error de conexión: " + ex.Message;
            }




        }

        private void enviar_queja_Click(object sender, RoutedEventArgs e)
        {
            string usuario = App.UsuarioActual;
            string mensaje = queja.Text;


            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string mensajeCompleto = $"[{usuario}]{fecha} -> {mensaje}";


            byte[] datos = Encoding.UTF8.GetBytes(mensajeCompleto);
            cliente.Send(datos);
        }
    }
}
