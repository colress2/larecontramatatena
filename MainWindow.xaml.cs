using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace laRECONTRAmatatena
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string QUEPERSONAJEES;
        public string PERSONAJENOMBRE;
        public string Clima;
        public MediaPlayer mediaPlayer;
        public bool existeclima;
        
        public double volumenvar;
        #region Propiedad personaje
        public int PersonajeColumnas { get; set; } = 1;
        private int PersonajeFilas { get; set; } = 1;
        private List<Personaje> _ListaPersonaje { get; set; }
        public List<Personaje> ListaPersonaje
        {
            get { return _ListaPersonaje; }
            set
            {
                _ListaPersonaje = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region modelo
        public class Personaje : INotifyPropertyChanged
        {
            public int PersonajePosicionEjeX { get; set; }
            public int PersonajePosicionEjeY { get; set; }
            public string _personaje { get; set; }
            public string personaje
            {
                get { return _personaje; }
                set
                {
                    _personaje = value;
                    OnPropertyChanged();
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion

        public MainWindow(string personaje, string nombre, string clima, bool personalizarclima,  double Volumen)
        {
            InitializeComponent();
            DataContext = this;
            QUEPERSONAJEES = personaje;
            PERSONAJENOMBRE = nombre;
            Clima = clima;
            existeclima = personalizarclima;
            
            volumenvar = Volumen;            
            CrearPersonaje();
            playSoundtrack();
            lblNombrePersonaje.Content = nombre;
        }
        //Poner musica de fondo
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

        //Método para rellenar la foto del personaje
        public void CrearPersonaje()
        {
            List<Personaje> MyList = new List<Personaje>();
            for (int i = 0; i < PersonajeFilas; i++)
            {
                for (int o = 0; o < PersonajeColumnas; o++)
                {
                    MyList.Add(new Personaje { PersonajePosicionEjeX = i, PersonajePosicionEjeY = o, personaje = QUEPERSONAJEES });
                }
            }
            ListaPersonaje = MyList;
        }


        //Botón para cambiar de personaje
        private void ClicIzq(object sender, MouseButtonEventArgs e)
        {
            playSfx(1);
            stopSoundtrack();
            CambiarPersonaje cambiarpersonaje = new CambiarPersonaje(Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            cambiarpersonaje.Show();
        }


        //Botón para jugar contra la IA
        private void Jugar(object sender, RoutedEventArgs e)
        {
            playSfx(1);
            stopSoundtrack();
            Jugar jugar = new Jugar(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            jugar.Show();
        }


        //Botón para jugar contra otro jugador
        private void JugarCo_op(object sender, RoutedEventArgs e)
        {
            playSfx(1);
            ElegirComoJugar elegircomojugar = new ElegirComoJugar(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, existeclima, volumenvar, mediaPlayer);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            elegircomojugar.Show();
        }

        //Botón para saber como jugar
        private void ComoJugar(object sender, RoutedEventArgs e)
        {
            playSfx(1);
            ComoJugar comojugar = new ComoJugar(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            comojugar.Show();
        }

        //Botón para salir de la aplicación
        private void Salir(object sender, RoutedEventArgs e)
        {
            playSfx(2);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void Creditos(object sender, RoutedEventArgs e)
        {
            playSfx(1);
            stopSoundtrack();
            Creditos creditos = new Creditos(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            creditos.Show();
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

        //Método para entrar a las opciones
        private void Opciones(object sender, RoutedEventArgs e)
        {
            playSfx(1);
            Opciones creditos = new Opciones(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, existeclima, volumenvar, mediaPlayer);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            creditos.Show();
        }
    }
}
