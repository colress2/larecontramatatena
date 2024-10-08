using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Threading;

namespace laRECONTRAmatatena
{
    /// <summary>
    /// Lógica de interacción para PantallaDeCarga.xaml
    /// </summary>
    public partial class PantallaDeCarga : Window
    {
        #region Variables varias
        public string ResultadoBonito2 = string.Empty;
        public string Clima = string.Empty;
        public DispatcherTimer timer;
        public bool existeclima = true;
        
        public double volumenvar = -1;
        #endregion
        public PantallaDeCarga()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Start();
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Incrementa el valor de la ProgressBar
            BarraDeCarga.Value += 5;


            if (BarraDeCarga.Value == 300)
            {
                if (LlamarApi())
                {
                    Clima = ResultadoBonito2;
                }
                else
                {
                    //Se pone predefinido si llega a fallar la conexión con la API
                    Clima = "Predefinido";
                }
            }

            // Si la ProgressBar está completamente llena, detiene el timer
            if (BarraDeCarga.Value >= BarraDeCarga.Maximum)
            {
                timer.Stop();
                CambiarPersonaje cambiarpersonaje = new CambiarPersonaje(Clima, existeclima, volumenvar);
                this.Close();
                cambiarpersonaje.Show();
            }
        }

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
    }
}
