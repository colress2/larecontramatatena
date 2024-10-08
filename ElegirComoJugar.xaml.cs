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
    /// Lógica de interacción para ElegirComoJugar.xaml
    /// </summary>
    public partial class ElegirComoJugar : Window
    {
        public string QUEPERSONAJEES;
        public string PERSONAJENOMBRE;
        public string Clima;
        public MediaPlayer mediaPlayer;
        public bool existeclima;
        
        public double volumenvar;
        public ElegirComoJugar(string personaje, string nombre, string clima, bool personalizarclima,  double Volumen, MediaPlayer mediaPlayer2)
        {
            InitializeComponent();
            QUEPERSONAJEES = personaje;
            PERSONAJENOMBRE = nombre;
            Clima = clima;
            existeclima = personalizarclima;
            mediaPlayer = mediaPlayer2;
            volumenvar = Volumen;
            //playSoundtrack();
        }

        //Poner musica de fondo
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

        private void JugarOtraMaquina(object sender, RoutedEventArgs e)
        {
            playSfx(1);
            ConectarPvPDiferenteMaquina conectarPvPDiferenteMaquina = new ConectarPvPDiferenteMaquina(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, existeclima, volumenvar, mediaPlayer);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            conectarPvPDiferenteMaquina.Show();
        }

        private void JugarMismaMaquina(object sender, RoutedEventArgs e)
        {
            playSfx(1);
            stopSoundtrack();
            Elegir2Jugador conectarPvPMismaMaquina = new Elegir2Jugador(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            conectarPvPMismaMaquina.Show();
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            playSfx(2);
            stopSoundtrack();
            MainWindow mainWindow = new MainWindow(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainWindow.Show();
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
