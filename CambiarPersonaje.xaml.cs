using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
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

namespace laRECONTRAmatatena
{
    /// <summary>
    /// Lógica de interacción para CambiarPersonaje.xaml
    /// </summary>
    public partial class CambiarPersonaje : Window
    {
        public string personaje;
        public string nombre;
        public string ResultadoBonito2 = string.Empty;
        public string Clima = string.Empty;
        public MediaPlayer mediaPlayer;
        public bool existeclima;
        
        public double volumenvar;
        public CambiarPersonaje(string Climas, bool personalizarclima,  double Volumen)
        {
            InitializeComponent();
            Clima = Climas;
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
            personaje = "Imagenes/Cordero.gif";
            nombre = "El Cordero";
            stopSoundtrack();
            MainWindow mainwindow = new MainWindow(personaje, nombre, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajePenitente(object sender, RoutedEventArgs e)
        {
            personaje = "Imagenes/Penitente.gif";
            nombre = "El Penitente";
            stopSoundtrack();
            MainWindow mainwindow = new MainWindow(personaje, nombre, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajeShovelKnight(object sender, RoutedEventArgs e)
        {
            personaje = "Imagenes/ShovelKnight.gif";
            nombre = "Shovel Knight";
            stopSoundtrack();
            MainWindow mainwindow = new MainWindow(personaje, nombre, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajeFlowey(object sender, RoutedEventArgs e)
        {
            personaje = "Imagenes/Flowey.gif";
            nombre = "Flowey";
            stopSoundtrack();
            MainWindow mainwindow = new MainWindow(personaje, nombre, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajeBadeline(object sender, RoutedEventArgs e)
        {
            personaje = "Imagenes/Badeline.gif";
            nombre = "Badeline";
            stopSoundtrack();
            MainWindow mainwindow = new MainWindow(personaje, nombre, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajeCaptainViridian(object sender, RoutedEventArgs e)
        {
            personaje = "Imagenes/CaptainViridian.gif";
            nombre = "Capitan Viridian";
            stopSoundtrack();
            MainWindow mainwindow = new MainWindow(personaje, nombre, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajeIsaac(object sender, RoutedEventArgs e)
        {
            personaje = "Imagenes/Isaac.gif";
            nombre = "Isaac";
            stopSoundtrack();
            MainWindow mainwindow = new MainWindow(personaje, nombre, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajeClubPenguin(object sender, RoutedEventArgs e)
        {
            personaje = "Imagenes/ClubPenguin.gif";
            nombre = "Pingüino";
            stopSoundtrack();
            MainWindow mainwindow = new MainWindow(personaje, nombre, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajeDrFetus(object sender, RoutedEventArgs e)
        {
            personaje = "Imagenes/DrFetus.gif";
            nombre = "Dr Fetus";
            stopSoundtrack();
            MainWindow mainwindow = new MainWindow(personaje, nombre, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void PersonajeTacaXeraca(object sender, RoutedEventArgs e)
        {
            personaje = "Imagenes/TacaXeraca.gif";
            nombre = "Zero";
            stopSoundtrack();
            MainWindow mainwindow = new MainWindow(personaje, nombre, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }


        //Método para Traspasar el gif y el nombre del personaje
        private void Personajeruben(object sender, RoutedEventArgs e)
        {
            personaje = "Imagenes/ruben.jpg";
            nombre = "SENIOR BETATESTER";
            stopSoundtrack();
            MainWindow mainwindow = new MainWindow(personaje, nombre, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainwindow.Show();
        }
    }
}