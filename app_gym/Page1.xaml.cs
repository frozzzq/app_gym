using DPFP;
using DPFP.Capture;
using DPFP.Processing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace app_gym
{
    /// <summary>
    /// Lógica de interacción para Page1.xaml
    /// </summary>
    public partial class Page1 : Page, DPFP.Capture.EventHandler
    {
        private DPFP.Capture.Capture captura;
        private DPFP.Processing.Enrollment enrollment;
        private DPFP.Template Template;
        private bool modoEnrolamiento = true;

        public static void EsconderVentanaPrincipal()
        {
            Application.Current.MainWindow.Hide();
        }
        public Page1()
        {
            InitializeComponent();
            EsconderVentanaPrincipal();
            
            
            captura = new DPFP.Capture.Capture();
            if (captura != null)
            {
                captura.EventHandler = this;
                try
                {
                    enrollment = new DPFP.Processing.Enrollment();
                    captura.StartCapture();
                }
                catch (Exception e)
                {
                    //MessageBox.Show("No se pudo iniciar la captura de huellas dactilares. " + e.Message);
                }
            }
            else
            {
                MessageBox.Show("No se pudo iniciar la captura de huellas dactilares.");
            }


        }

        private byte[] convertirTemplateByte(DPFP.Template template)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                template.Serialize(ms);
                return ms.ToArray();
            }
        }

        private FeatureSet ExtraerCaracteristicas(Sample sample, DataPurpose purpose)
        {
            FeatureExtraction extractor = new FeatureExtraction();
            CaptureFeedback feedback = CaptureFeedback.None;
            FeatureSet features = new FeatureSet();

            extractor.CreateFeatureSet(sample, purpose, ref feedback, ref features);
            return (feedback == CaptureFeedback.Good) ? features : null;
        }


        private DPFP.Template ConvertirBytesATemplate(byte[] huellaBytes)
        {
            using (MemoryStream ms = new MemoryStream(huellaBytes))
            {
                return new DPFP.Template(ms);
            }
        }
        private FeatureSet ExtraerCaracteristicasVerificacion(Sample sample)
        {
            FeatureExtraction extractor = new FeatureExtraction();
            CaptureFeedback feedback = CaptureFeedback.None;
            FeatureSet features = new FeatureSet();

            extractor.CreateFeatureSet(sample, DataPurpose.Verification, ref feedback, ref features);

            return (feedback == CaptureFeedback.Good) ? features : null;
        }
        private bool VerificarHuella(FeatureSet features, DPFP.Template template)
        {
            DPFP.Verification.Verification verificador = new DPFP.Verification.Verification();
            DPFP.Verification.Verification.Result resultado = new DPFP.Verification.Verification.Result();

            verificador.Verify(features, template, ref resultado);

            return resultado.Verified;
        }
        private void IdentificarCliente(Sample sample)
        {
            FeatureSet features = ExtraerCaracteristicasVerificacion(sample);

            if (features == null)
            {
                Dispatcher.Invoke(() => verificar.Text = "La muestra no es válida.");
                return;
            }

            // 1. Leer todas las huellas de SQL
            string conexion = app_gym.Properties.Settings.Default.gymConnectionString1;
            //string conexion = null;
            //var cs = ConfigurationManager.ConnectionStrings["app_gym.Properties.Settings.gymConnectionString1"];
            //if (cs != null)
            //{
            //    conexion = cs.ConnectionString;
            //}
            //else
            //{
            //    conexion = app_gym.Properties.Settings.Default.gymConnectionString1;
            //}

            //if (string.IsNullOrWhiteSpace(conexion))
            //{
            //    MessageBox.Show("Cadena de conexión no encontrada. Revise app.config o Settings.");
            //    return;
            //}

            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();

                string query = "SELECT id, nombre, apellidos, huella FROM clientes WHERE huella IS NOT NULL";

                SqlCommand cmd = new SqlCommand(query, cn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string nombre = reader.GetString(1);
                    string apellidos = reader.GetString(2);

                    byte[] huellaBytes = (byte[])reader["huella"];

                    // Convertir bytes → Template
                    DPFP.Template template = ConvertirBytesATemplate(huellaBytes);

                    // 2. Verificar
                    if (VerificarHuella(features, template))
                    {
                        Dispatcher.Invoke(() =>
                        {
                            verificar.Text = "Huella encontrada";
                            MessageBox.Show("Cliente identificado: " + nombre + " " + apellidos);
                        });

                        return;
                    }
                }
            }

            Dispatcher.Invoke(() =>
            {
                verificar.Text = "Huella NO encontrada en la base de datos.";
            });
        }

        private void registro_Click(object sender, RoutedEventArgs e)
        {


            string n = nombre.Text;
            string ap = ap1.Text;
            string em = correo.Text;

            if (string.IsNullOrEmpty(n) || string.IsNullOrEmpty(ap) || string.IsNullOrEmpty(em))
            {
                MessageBox.Show("Por favor ingrese un nombre.");
                return;
            }

            if (Template == null)
            {
                MessageBox.Show("Por favor registre la huella dactilar.");
                return;
            }

            byte[] huellaB = convertirTemplateByte(Template);


            //string conexion = ConfigurationManager.ConnectionStrings["app_gym.Properties.Settings.gymConnectionString1"].ConnectionString;
            string conexion = app_gym.Properties.Settings.Default.gymConnectionString1;
            miconexion = new SqlConnection(conexion);

            string insertar = "INSERT INTO clientes (nombre, apellidos, correo, huella, fecha_inscripcion) VALUES (@nombre, @apellidos, @correo, @huella, GETDATE())";
            SqlCommand insertar2 = new SqlCommand(insertar, miconexion);

            try
            {
                miconexion.Open();
                insertar2.Parameters.AddWithValue("@nombre", n);
                insertar2.Parameters.AddWithValue("@apellidos", ap);
                insertar2.Parameters.AddWithValue("@correo", em);
                insertar2.Parameters.AddWithValue("@huella", huellaB);



                insertar2.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            miconexion.Close();
            MessageBox.Show("Usuario registrado exitosamente.");
        }






        public void OnComplete(object Capture, string ReaderSerialNumber, Sample Sample)
        {
            if (modoEnrolamiento)
            {
                // → Enrolar
                FeatureSet features = ExtraerCaracteristicas(Sample, DataPurpose.Enrollment);
                if (features != null)
                {
                    enrollment.AddFeatures(features);

                    Dispatcher.Invoke(() =>
                    {
                        verificar.Text = "Muestras faltantes: " + enrollment.FeaturesNeeded;
                    });

                    if (enrollment.TemplateStatus == Enrollment.Status.Ready)
                    {
                        Template = enrollment.Template;
                        enrollment.Clear();

                        Dispatcher.Invoke(() =>
                        {
                            verificar.Text = "Enrolamiento completado ";
                        });
                    }
                }
            }
            else
            {
                // → Verificar / Identificar
                IdentificarCliente(Sample);
            }
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
        
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            Dispatcher.Invoke(() =>
            {
                estatus_sensor.Text = "Sensor Detectado";
            });
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            Dispatcher.Invoke(() =>
            {
                estatus_sensor.Text = "Sensor No Detectado";
            });
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, CaptureFeedback CaptureFeedback)
        {
            
        }

        SqlConnection miconexion;

        private void verificar_sensor_Click(object sender, RoutedEventArgs e)
        {
            verificar.Text = " ";
            modoEnrolamiento = false;
            estado.Text = "Modo Verificación/Identificación";
        }

        private void volver_Click(object sender, RoutedEventArgs e)
        {
            verificar.Text = " ";
            modoEnrolamiento = true;
            estado.Text = "Modo Enrolamiento";
        }
    }
}
