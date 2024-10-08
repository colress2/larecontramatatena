using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

namespace laRECONTRAmatatena
{
    /// <summary>
    /// Lógica de interacción para ComoJugar.xaml
    /// </summary>
    public partial class ComoJugar : Window
    {
        #region Variables varias
        public string QUEPERSONAJEES;
        public string PERSONAJENOMBRE;
        public string Clima;
        public MediaPlayer mediaPlayer;
        public bool existeclima;
        
        public double volumenvar;
        #endregion
        public ComoJugar(string personaje, string nombre, string clima, bool personalizarclima,  double Volumen)
        {
            InitializeComponent();
            QUEPERSONAJEES = personaje;
            PERSONAJENOMBRE = nombre;
            Clima = clima;
            existeclima = personalizarclima;
            
            volumenvar = Volumen;
            //playSoundtrack();
        }

        //Método para poner musica de fondo
        private void playSoundtrack()
        {
            mediaPlayer = new MediaPlayer();
            string rutarelativa = "Soundtrack\\Pizza Tower OST - Hip to be Italian (Peppino's Final Judgement).mp3";
            string rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
            mediaPlayer.Open(new Uri(rutaabsoluta));
            mediaPlayer.MediaEnded += (sender, e) =>
            {
                mediaPlayer.Position = TimeSpan.Zero;
                mediaPlayer.Play();
            };
            if (volumenvar > -1)
            {
                mediaPlayer.Volume = volumenvar;
            }
            mediaPlayer.Play();
        }

        //Detener el soundtrack cuando se salga de la ventana
        private void stopSoundtrack()
        {
            mediaPlayer.Stop();
            mediaPlayer.Close();
            mediaPlayer = null;
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            playSfx();
            MainWindow mainWindow = new MainWindow(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainWindow.Show();
        }

        //Método para poner efectos de sonido
        private void playSfx()
        {
            SoundPlayer player;
            string rutarelativa;
            string rutaabsoluta;
            rutarelativa = "Sfx\\sfxVolver.wav";
            rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
            player = new SoundPlayer(rutaabsoluta);
            player.Play();
        }
    }
}
