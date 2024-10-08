using System;
using System.IO;
using System.Media;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace laRECONTRAmatatena
{
    /// <summary>
    /// Lógica de interacción para Opciones.xaml
    /// </summary>
    public partial class Opciones : Window
    {
        #region Variables varias
        public string ResultadoBonito2 = string.Empty;
        public string QUEPERSONAJEES;
        public string PERSONAJENOMBRE;
        public string Clima;
        public MediaPlayer mediaPlayer;
        public bool climareal;

        public double volumenvar;
        #endregion
        public Opciones(string personaje, string nombre, string clima, bool personalizarclima,  double Volumen, MediaPlayer mediaPlayer2)
        {
            InitializeComponent();
            QUEPERSONAJEES = personaje;
            PERSONAJENOMBRE = nombre;
            Clima = clima;
            climareal = personalizarclima;
            mediaPlayer = mediaPlayer2;
            //playSoundtrack();
            Check_Rdbtn();

            if (Volumen > -1)
            {
                mediaPlayer.Volume = Volumen;
            }
            SliderVolumen.Value = mediaPlayer.Volume;
            if (climareal)
            {
                DesactivarRdbtn();
                BtnActivar_DesactivarClima.Source = new BitmapImage(new Uri("Imagenes/botonEnabled.png", UriKind.Relative));
                LblDesactivadoBlanco.Visibility = Visibility.Collapsed;
                LblDesactivadoRojo.Visibility = Visibility.Collapsed;
                LblActivadoBlanco.Visibility = Visibility.Visible;
                LblActivadoRojo.Visibility = Visibility.Visible;
            }
            else
            {
                ActivarRdbtn();                
                BtnActivar_DesactivarClima.Source = new BitmapImage(new Uri("Imagenes/botonDisabled.png", UriKind.Relative));
                LblDesactivadoBlanco.Visibility = Visibility.Visible;
                LblDesactivadoRojo.Visibility = Visibility.Visible;
                LblActivadoBlanco.Visibility = Visibility.Collapsed;
                LblActivadoRojo.Visibility = Visibility.Collapsed;
            }
        }

        //Método para seleccionar el radiobuttons correspondiente al clima actual.
        public void Check_Rdbtn()
        {
            if (Clima.Contains("Nieve") || Clima.Contains("nieve") || Clima.Contains("granizo") || Clima.Contains("Granizo"))
            {
                RdbtnClimaNieve.IsChecked = true;
            }
            else if (Clima.Contains("Tormenta") || Clima.Contains("tormenta") || Clima.Contains("lluvia") || Clima.Contains("Lluvia"))
            {
                RdbtnClimaTormenta.IsChecked = true;
            }
            else if (Clima.Contains("Soleado") || Clima.Contains("soleado"))
            {
                RdbtnClimaSoleado.IsChecked = true;
            }
            else
            {
                RdbtnClimaN_A.IsChecked = true;
            }
        }

        //Método para descativar todos los radiosbuttons
        public void DesactivarRdbtn() 
        {
            RdbtnClimaN_A.IsEnabled = false;
            RdbtnClimaN_A.IsChecked = false;
            RdbtnClimaSoleado.IsEnabled = false;
            RdbtnClimaSoleado.IsChecked = false;
            RdbtnClimaNieve.IsEnabled = false;
            RdbtnClimaNieve.IsChecked = false;
            RdbtnClimaTormenta.IsEnabled = false;
            RdbtnClimaTormenta.IsChecked = false;
        }

        //Método para Activar todos los radiosbuttons
        public void ActivarRdbtn()
        {
            RdbtnClimaN_A.IsEnabled = true;
            RdbtnClimaSoleado.IsEnabled = true;            
            RdbtnClimaNieve.IsEnabled = true;
            RdbtnClimaTormenta.IsEnabled = true;
        }

        //Método para poner musica de fondo
        private void playSoundtrack()
        {
            mediaPlayer = new MediaPlayer();
            string rutarelativa = "Soundtrack\\Pizza Tower OST - Hip to be Italian (Peppino's Final Judgement).mp3";
            string rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
            mediaPlayer.Open(new Uri(rutaabsoluta));            
        }

        //Detener el soundtrack cuando se salga de la ventana
        private void stopSoundtrack()
        {
            mediaPlayer.Stop();
            mediaPlayer.Close();
            mediaPlayer = null;
        }

        //Botón para volver al menú de inicio
        private void Salir(object sender, RoutedEventArgs e)
        {
            playSfx(2);
            stopSoundtrack();
            MainWindow mainWindow = new MainWindow(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, climareal, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainWindow.Show();
        }

        private void Activar_DesactivarClima(object sender, RoutedEventArgs e)
        {
            if (climareal)
            {
                ActivarRdbtn();
                BtnActivar_DesactivarClima.Source = new BitmapImage(new Uri("Imagenes/botonDisabled.png", UriKind.Relative));
                climareal = false;
                LblDesactivadoBlanco.Visibility = Visibility.Visible;
                LblDesactivadoRojo.Visibility = Visibility.Visible;
                LblActivadoBlanco.Visibility = Visibility.Collapsed;
                LblActivadoRojo.Visibility = Visibility.Collapsed;
            }
            else
            {
                DesactivarRdbtn();
                BtnActivar_DesactivarClima.Source = new BitmapImage(new Uri("Imagenes/botonEnabled.png", UriKind.Relative));
                climareal = true;
                LblDesactivadoBlanco.Visibility = Visibility.Collapsed;
                LblDesactivadoRojo.Visibility = Visibility.Collapsed;
                LblActivadoBlanco.Visibility = Visibility.Visible;
                LblActivadoRojo.Visibility = Visibility.Visible;
                if (LlamarApi())
                {
                    Clima = ResultadoBonito2;
                }
                else
                {
                    //Se pone predefinido si llega a fallar la conexión con la API
                    Clima = "Predefinido";
                }
                Thread.Sleep(2000);
            }
        }

        //Método para rellenar la variable con el tablero seleccionado.
        private void RellenarClima(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton Rdbtn)
            {
                Clima = Rdbtn.Content.ToString();
            }
        }
        
        //Método para obtener el clima.
        public bool LlamarApi()
        {
            try
            {
                //Hace la conexión hacia la página para pedir el Json
                string url = "https://opendata.aemet.es/opendata/api/prediccion/especifica/municipio/horaria/28017/?api_key=eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ5bGJlcnRib3NjYW5AZ21haWwuY29tIiwianRpIjoiYTQ1NTIxZmMtOTI1OC00YmIwLTllOWMtMzAzYTUxODZiYTVlIiwiaXNzIjoiQUVNRVQiLCJpYXQiOjE3MDgzNjAxNzgsInVzZXJJZCI6ImE0NTUyMWZjLTkyNTgtNGJiMC05ZTljLTMwM2E1MTg2YmE1ZSIsInJvbGUiOiIifQ.pn_QOAENu2sWuXlPZ1XPGVRq0jTuOz2WTL76SEmeWCg";
                WebRequest webRequest = WebRequest.Create(url);
                HttpWebResponse httpWebResponse = null;
                httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
                string result = string.Empty;
                string resultadobonito = string.Empty;
                using (Stream stream = httpWebResponse.GetResponseStream())
                {
                    StreamReader streamReader = new StreamReader(stream);
                    while ((result = streamReader.ReadLine()) != null)
                    {
                        if (result.Contains("datos") && !result.Contains("metadatos"))
                        {
                            string[] ResultadoSeparado = result.Split('\"');
                            resultadobonito = ResultadoSeparado[3];
                            break;
                        }
                    }
                    streamReader.Close();
                }

                //Hace la conexión hacia la página para obtener el Json

                DateTime hour = DateTime.Now;
                int horaenint = hour.Hour;
                string hora = hour.Hour.ToString();

                if (horaenint < 10)
                {
                    hora = "0" + hora;
                }

                string url2 = resultadobonito;
                WebRequest webRequest2 = WebRequest.Create(url2);
                HttpWebResponse httpWebResponse2 = null;
                httpWebResponse2 = (HttpWebResponse)webRequest2.GetResponse();
                string result2 = string.Empty;
                using (Stream stream = httpWebResponse2.GetResponseStream())
                {
                    StreamReader streamReader = new StreamReader(stream);
                    while ((result2 = streamReader.ReadLine()) != null)
                    {
                        if (result2.Contains("\"periodo\" : \"" + hora + "\""))
                        {
                            while ((result2 = streamReader.ReadLine()) != null)
                            {
                                string[] ResultadoSeparado = result2.Split('\"');
                                ResultadoBonito2 = ResultadoSeparado[3];
                                if (!ResultadoBonito2.Equals(""))
                                {
                                    streamReader.Close();
                                    return true;
                                }
                            }
                        }
                    }
                    streamReader.Close();
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void CambiarVolumen(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            volumenvar = SliderVolumen.Value;
            mediaPlayer.Volume = volumenvar;
        }

        //Método para poner efectos de sonido
        private void playSfx(int n)
        {
            SoundPlayer player;
            string rutarelativa;
            string rutaabsoluta;
            switch (n)
            {
                case 1:
                    rutarelativa = "Sfx\\sfxOk.wav";
                    rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
                    player = new SoundPlayer(rutaabsoluta);
                    player.Play();
                    break;
                case 2:
                    rutarelativa = "Sfx\\sfxVolver.wav";
                    rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
                    player = new SoundPlayer(rutaabsoluta);
                    player.Play();
                    break;
            }
        }
    }
}
