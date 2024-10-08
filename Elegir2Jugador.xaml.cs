using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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
using System.Media;

namespace laRECONTRAmatatena
{
    public partial class Elegir2Jugador : Window
    {
        #region Variables varias
        public string personaje2;
        public string nombre2;
        public string Clima;
        public string QUEPERSONAJEES;
        public string PERSONAJENOMBRE;
        public MediaPlayer mediaPlayer;
        public bool existeclima;
        
        public double volumenvar;
        #endregion



        public Elegir2Jugador(string personaje, string nombre, string clima, bool personalizarclima,  double Volumen)
        {
            InitializeComponent();
            Clima = clima;
            QUEPERSONAJEES = personaje;
            PERSONAJENOMBRE = nombre;
            existeclima = personalizarclima;
            
            volumenvar = Volumen;
            playSoundtrack();
        }

        //Poner musica de fondo
        private void playSoundtrack()
        {
            mediaPlayer = new MediaPlayer();
            string rutarelativa = "Soundtrack\\Metal Slug 3 - Character Select.mp3";
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


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajeCordero(object sender, RoutedEventArgs e)
        {
            personaje2 = "Imagenes/Cordero.gif";
            nombre2 = "El Cordero";
            stopSoundtrack();
            JugadorvsjugadorMismaMaquina mainwindow = new JugadorvsjugadorMismaMaquina(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, personaje2, nombre2, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajePenitente(object sender, RoutedEventArgs e)
        {
            personaje2 = "Imagenes/Penitente.gif";
            nombre2 = "El Penitente";
            stopSoundtrack();
            JugadorvsjugadorMismaMaquina mainwindow = new JugadorvsjugadorMismaMaquina(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, personaje2, nombre2, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajeShovelKnight(object sender, RoutedEventArgs e)
        {
            personaje2 = "Imagenes/ShovelKnight.gif";
            nombre2 = "Shovel Knight";
            stopSoundtrack();
            JugadorvsjugadorMismaMaquina mainwindow = new JugadorvsjugadorMismaMaquina(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, personaje2, nombre2, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajeFlowey(object sender, RoutedEventArgs e)
        {
            personaje2 = "Imagenes/Flowey.gif";
            nombre2 = "Flowey";
            stopSoundtrack();
            JugadorvsjugadorMismaMaquina mainwindow = new JugadorvsjugadorMismaMaquina(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, personaje2, nombre2, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajeBadeline(object sender, RoutedEventArgs e)
        {
            personaje2 = "Imagenes/Badeline.gif";
            nombre2 = "Badeline";
            stopSoundtrack();
            JugadorvsjugadorMismaMaquina mainwindow = new JugadorvsjugadorMismaMaquina(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, personaje2, nombre2, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajeCaptainViridian(object sender, RoutedEventArgs e)
        {
            personaje2 = "Imagenes/CaptainViridian.gif";
            nombre2 = "Capitan Viridian";
            stopSoundtrack();
            JugadorvsjugadorMismaMaquina mainwindow = new JugadorvsjugadorMismaMaquina(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, personaje2, nombre2, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajeIsaac(object sender, RoutedEventArgs e)
        {
            personaje2 = "Imagenes/Isaac.gif";
            nombre2 = "Isaac";
            stopSoundtrack();
            JugadorvsjugadorMismaMaquina mainwindow = new JugadorvsjugadorMismaMaquina(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, personaje2, nombre2, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajeClubPenguin(object sender, RoutedEventArgs e)
        {
            personaje2 = "Imagenes/ClubPenguin.gif";
            nombre2 = "Pingüino";
            stopSoundtrack();
            JugadorvsjugadorMismaMaquina mainwindow = new JugadorvsjugadorMismaMaquina(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, personaje2, nombre2, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajeDrFetus(object sender, RoutedEventArgs e)
        {
            personaje2 = "Imagenes/DrFetus.gif";
            nombre2 = "Dr Fetus";
            stopSoundtrack();
            JugadorvsjugadorMismaMaquina mainwindow = new JugadorvsjugadorMismaMaquina(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, personaje2, nombre2, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajeTacaXeraca(object sender, RoutedEventArgs e)
        {
            personaje2 = "Imagenes/TacaXeraca.gif";
            nombre2 = "Zero";
            stopSoundtrack();
            JugadorvsjugadorMismaMaquina mainwindow = new JugadorvsjugadorMismaMaquina(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, personaje2, nombre2, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void Personajeruben(object sender, RoutedEventArgs e)
        {
            personaje2 = "Imagenes/ruben.jpg";
            nombre2 = "SENIOR BETATESTER";
            stopSoundtrack();
            JugadorvsjugadorMismaMaquina mainwindow = new JugadorvsjugadorMismaMaquina(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, personaje2, nombre2, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            playSfx();
            stopSoundtrack();
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
