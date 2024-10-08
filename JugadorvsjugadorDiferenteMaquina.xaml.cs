using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace laRECONTRAmatatena
{
    /// <summary>
    /// Lógica de interacción para Jugar.xaml
    /// </summary>
    ///

    public partial class JugadorvsjugadorDiferenteMaquina : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region Variables varias
        public int NumRandom;
        public bool turno = true;
        public string QUEPERSONAJEES;
        public string PERSONAJENOMBRE;
        public string ENEMIGONOMBRE;
        public string Clima;
        public DispatcherTimer MyTimer = new DispatcherTimer();
        public int Seconds;
        public int HaPuestoNumero;
        public int CondicionDeVictoria = 0;
        public volatile int contador = 0;
        public volatile bool juegoTerminado = false;
        public volatile bool turnoActual = true;
        public int valorDadoSanto = 1;
        public bool dobleturno = false;
        public bool ActivarMaldito = false;
        public string climaActual;
        public int timerTormentaElectrica = 3;
        public int timerSol = 4;
        public int timerNieve = 4;
        public string TipoDeDado = "";
        public Thread sistemaTurnos;
        public bool desconexion = false;
        public bool existeclima;
        
        public double volumenvar;
        //Tablero 1
        public int PuntosTablero1 = 0;
        public int NumerosIgualesTablero1 = 0;
        public volatile int Tablero1Estrella = 0;
        public int ValorFila1Tablero1 = 0;
        public int ValorFila2Tablero1 = 0;
        public int ValorFila3Tablero1 = 0;
        public int ValorFila4Tablero1 = 0;
        public int ValorFila5Tablero1 = 0;
        //Tablero 2
        public int PuntosTablero2 = 0;
        public int NumerosIgualesTablero2 = 0;
        public volatile int Tablero2Estrella = 0;
        public int ValorFila1Tablero2 = 0;
        public int ValorFila2Tablero2 = 0;
        public int ValorFila3Tablero2 = 0;
        public int ValorFila4Tablero2 = 0;
        public int ValorFila5Tablero2 = 0;
        public MediaPlayer mediaPlayer;
        public Socket ClienteSocket;
        public string mensajeRecibido;
        public bool PrimerJugador;

        #endregion

        #region Propiedades del tablero 2
        public int Tablero2Columnas { get; set; } = 5;
        private int Tablero2Filas { get; set; } = 5;
        private List<Tablero2Celda> _Tablero2 { get; set; }
        public List<Tablero2Celda> Tablero2
        {
            get { return _Tablero2; }
            set
            {
                _Tablero2 = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Propiedades del tablero 1
        public int Tablero1Columnas { get; set; } = 5;
        private int Tablero1Filas { get; set; } = 5;
        private List<Tablero1Celda> _Tablero1 { get; set; }
        public List<Tablero1Celda> Tablero1
        {
            get { return _Tablero1; }
            set
            {
                _Tablero1 = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Propiedades del entorno
        public int EntornoColumnas { get; set; } = 1;
        public int EntornoFilas { get; set; } = 1;
        private List<Entorno> _ListaEntorno { get; set; }
        public List<Entorno> ListaEntorno
        {
            get { return _ListaEntorno; }
            set
            {
                _ListaEntorno = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Propiedades del personaje
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

        #region Propiedades del enemigo
        public int EnemigoColumnas { get; set; } = 1;
        private int EnemigoFilas { get; set; } = 1;
        private List<Enemigo> _ListaEnemigo { get; set; }
        public List<Enemigo> ListaEnemigo
        {
            get { return _ListaEnemigo; }
            set
            {
                _ListaEnemigo = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Modelos
        public class JuegoEstado
        {
            public List<Tablero1Celda> Tablero1 { get; set; }
            public int PuntosTablero1 { get; set; }
            public int Tablero1Estrella { get; set; }

            public List<Tablero2Celda> Tablero2 { get; set; }
            public int PuntosTablero2 { get; set; }
            public int Tablero2Estrella { get; set; }

            public bool desconexion;
        }

        public class Entorno : INotifyPropertyChanged
        {
            public int EntornoPosicionEjeX { get; set; }
            public int EntornoPosicionEjey { get; set; }
            public string _entorno { get; set; }
            public string entorno
            {
                get { return _entorno; }
                set
                {
                    _entorno = value;
                    OnPropertyChanged();
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
        public class Enemigo : INotifyPropertyChanged
        {
            public int EnemigoPosicionEjeX { get; set; }
            public int EnemigoPosicionEjey { get; set; }
            public string _enemigo { get; set; }
            public string enemigo
            {
                get { return _enemigo; }
                set
                {
                    _enemigo = value;
                    OnPropertyChanged();
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
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
        public class Tablero2Celda : INotifyPropertyChanged
        {
            public int Tablero2PosicionEjeX { get; set; }
            public int Tablero2PosicionEjeY { get; set; }
            public bool _Tablero2IsEscudo { get; set; }
            public bool Tablero2IsEscudo
            {
                get { return _Tablero2IsEscudo; }
                set
                {
                    _Tablero2IsEscudo = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero2IsMaldito { get; set; }
            public bool Tablero2IsMaldito
            {
                get { return _Tablero2IsMaldito; }
                set
                {
                    _Tablero2IsMaldito = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero2IsInverso { get; set; }
            public bool Tablero2IsInverso
            {
                get { return _Tablero2IsInverso; }
                set
                {
                    _Tablero2IsInverso = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero2IsCancelador { get; set; }
            public bool Tablero2IsCancelador
            {
                get { return _Tablero2IsCancelador; }
                set
                {
                    _Tablero2IsCancelador = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero2IsBendito { get; set; }
            public bool Tablero2IsBendito
            {
                get { return _Tablero2IsBendito; }
                set
                {
                    _Tablero2IsBendito = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero2IsExplosivo { get; set; }
            public bool Tablero2IsExplosivo
            {
                get { return _Tablero2IsExplosivo; }
                set
                {
                    _Tablero2IsExplosivo = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero2IsCongelado { get; set; }
            public bool Tablero2IsCongelado
            {
                get { return _Tablero2IsCongelado; }
                set
                {
                    _Tablero2IsCongelado = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero2IsHielo { get; set; }
            public bool Tablero2IsHielo
            {
                get { return _Tablero2IsHielo; }
                set
                {
                    _Tablero2IsHielo = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero2IsFuego { get; set; }
            public bool Tablero2IsFuego
            {
                get { return _Tablero2IsFuego; }
                set
                {
                    _Tablero2IsFuego = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero2TieneNum { get; set; }
            public bool Tablero2TieneNum
            {
                get { return _Tablero2TieneNum; }
                set
                {
                    _Tablero2TieneNum = value;
                    OnPropertyChanged();
                }
            }
            public string _Tablero2ImagenDeEjemplo { get; set; }
            public string Tablero2ImagenDeEjemplo
            {
                get { return _Tablero2ImagenDeEjemplo; }
                set
                {
                    _Tablero2ImagenDeEjemplo = value;
                    OnPropertyChanged();
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        public class Tablero1Celda : INotifyPropertyChanged
        {
            public int Tablero1PosicionEjeX { get; set; }
            public int Tablero1PosicionEjeY { get; set; }
            public bool _Tablero1IsEscudo { get; set; }
            public bool Tablero1IsEscudo
            {
                get { return _Tablero1IsEscudo; }
                set
                {
                    _Tablero1IsEscudo = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero1IsMaldito { get; set; }
            public bool Tablero1IsMaldito
            {
                get { return _Tablero1IsMaldito; }
                set
                {
                    _Tablero1IsMaldito = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero1IsInverso { get; set; }
            public bool Tablero1IsInverso
            {
                get { return _Tablero1IsInverso; }
                set
                {
                    _Tablero1IsInverso = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero1IsCancelador { get; set; }
            public bool Tablero1IsCancelador
            {
                get { return _Tablero1IsCancelador; }
                set
                {
                    _Tablero1IsCancelador = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero1IsBendito { get; set; }
            public bool Tablero1IsBendito
            {
                get { return _Tablero1IsBendito; }
                set
                {
                    _Tablero1IsBendito = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero1IsExplosivo { get; set; }
            public bool Tablero1IsExplosivo
            {
                get { return _Tablero1IsExplosivo; }
                set
                {
                    _Tablero1IsExplosivo = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero1IsCongelado { get; set; }
            public bool Tablero1IsCongelado
            {
                get { return _Tablero1IsCongelado; }
                set
                {
                    _Tablero1IsCongelado = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero1IsHielo { get; set; }
            public bool Tablero1IsHielo
            {
                get { return _Tablero1IsHielo; }
                set
                {
                    _Tablero1IsHielo = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero1IsFuego { get; set; }
            public bool Tablero1IsFuego
            {
                get { return _Tablero1IsFuego; }
                set
                {
                    _Tablero1IsFuego = value;
                    OnPropertyChanged();
                }
            }
            public bool _Tablero1TieneNum { get; set; }
            public bool Tablero1TieneNum
            {
                get { return _Tablero1TieneNum; }
                set
                {
                    _Tablero1TieneNum = value;
                    OnPropertyChanged();
                }
            }
            public string _Tablero1ImagenDeEjemplo { get; set; }
            public string Tablero1ImagenDeEjemplo
            {
                get { return _Tablero1ImagenDeEjemplo; }
                set
                {
                    _Tablero1ImagenDeEjemplo = value;
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

        

        public JugadorvsjugadorDiferenteMaquina(string personaje, string personajenombre, string clima, string enemigonombre, Socket clienteSocket, bool primerjugador, bool personalizarclima,  double Volumen, string climaoponente)
        {
            InitializeComponent();
            DataContext = this;
            PERSONAJENOMBRE = personajenombre;
            QUEPERSONAJEES = personaje;
            ENEMIGONOMBRE = enemigonombre;
            Clima = clima;
            if (!PrimerJugador)
            {
                Clima = climaoponente;
            }
            existeclima = personalizarclima;            
            volumenvar = Volumen;
            CrearEntorno();
            CrearTablero();
            CrearPersonaje();
            CrearEnemigo();
            actualizarDado(0);
            habilitarDados(false);
            botonCancelar(false);
            visibilidadDadoSanto(false);
            playSoundtrack();
            lblNombreEnemigo.Content = ENEMIGONOMBRE;
            lblNombrePersonaje.Content = PERSONAJENOMBRE;
            ClienteSocket = clienteSocket;
            PrimerJugador = primerjugador;
            
            if (PrimerJugador)
            {
                iniciarSistemaTurnos();
            }
            else
            {
                relojgif.Visibility = Visibility.Collapsed;
                tempo.Visibility = Visibility.Collapsed;
                tempo.Foreground = Brushes.White;
                contador = 11;
                actualizarDado(0);
                btnLanzarDado.Visibility = Visibility.Visible;
                imagenDadoGIF.Visibility = Visibility.Visible;
                lblTurnoJugador1.Visibility = Visibility.Collapsed;
                lblTurnoJugador2.Visibility = Visibility.Visible;

                btnLanzarDado.IsEnabled = false;
                turno = false;
            }

            Task.Run(() => RecibirMensajes());
        }

        //Método para recibir el mensaje del otro jugador.
        private async void RecibirMensajes()
        {
            try
            {
                while (true)
                {
                    byte[] tamanoMensajeBytes = new byte[4];
                    int bytesRecibidos = await Task.Factory.FromAsync(
                        (callback, state) => ClienteSocket.BeginReceive(tamanoMensajeBytes, 0, 4, SocketFlags.None, callback, state),
                        ClienteSocket.EndReceive,
                        null);

                    if (bytesRecibidos != 4)
                    {
                        throw new Exception("Error al recibir el tamaño del mensaje.");
                    }

                    int tamanoMensaje = BitConverter.ToInt32(tamanoMensajeBytes, 0);
                    byte[] buffer = new byte[tamanoMensaje];
                    int totalBytesRecibidos = 0;

                    while (totalBytesRecibidos < tamanoMensaje)
                    {
                        bytesRecibidos = await Task.Factory.FromAsync(
                            (callback, state) => ClienteSocket.BeginReceive(buffer, totalBytesRecibidos, tamanoMensaje - totalBytesRecibidos, SocketFlags.None, callback, state),
                            ClienteSocket.EndReceive,
                            null);

                        if (bytesRecibidos == 0)
                        {
                            break;
                        }
                        totalBytesRecibidos += bytesRecibidos;
                    }

                    if (totalBytesRecibidos != tamanoMensaje)
                    {
                        throw new Exception("Error al recibir el mensaje completo");
                    }

                    string jsonString = Encoding.UTF8.GetString(buffer);

                    try
                    {
                        JuegoEstado estado = JsonConvert.DeserializeObject<JuegoEstado>(jsonString);

                        if (estado != null)
                        {
                            Application.Current.Dispatcher.Invoke(() => ProcesarMensaje(estado));
                        }
                    }
                    catch (JsonException ex)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                            MessageBox.Show("Error de deserialización JSON: " + ex.Message)
                        );
                    }
                }
            }
            catch (SocketException)
            {
                // Manejar error de conexión
                Application.Current.Dispatcher.Invoke(() => { 
                    MessageBox.Show("Se ha perdido la conexión con el servidor.");
                    ClienteSocket.Close();
                    stopSoundtrack();
                    MainWindow mainWindow = new MainWindow(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, existeclima, volumenvar);
                    this.Close();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    mainWindow.Show();
                });
            }
            catch (JsonException ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                    MessageBox.Show("Error de deserialización JSON: " + ex.Message)
                );
            }
            catch (Exception ex)
            {
                if (!desconexion)
                {
                    /*Application.Current.Dispatcher.Invoke(() =>
                        MessageBox.Show("Error: " + ex.Message)
                    );*/
                }
            }
        }

        //Método para realizar los cambios necesarios con el mensaje recibido.
        private void ProcesarMensaje(JuegoEstado mensaje)
        {
            if (mensaje.desconexion)
            {
                stopSoundtrack();
                ClienteSocket.Close();                
                MainWindow mainWindow = new MainWindow(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, existeclima, volumenvar);
                this.Close();
                mainWindow.Show();
                return;
            }

            Tablero2 = mensaje.Tablero1.Select(c => new Tablero2Celda 
            {
                Tablero2PosicionEjeX = c.Tablero1PosicionEjeX,
                Tablero2PosicionEjeY = c.Tablero1PosicionEjeY,
                Tablero2TieneNum = c.Tablero1TieneNum,
                Tablero2ImagenDeEjemplo = c._Tablero1ImagenDeEjemplo,
                Tablero2IsEscudo = c.Tablero1IsEscudo,
                Tablero2IsMaldito = c.Tablero1IsMaldito,
                Tablero2IsInverso = c.Tablero1IsInverso,
                Tablero2IsExplosivo = c.Tablero1IsExplosivo,
                Tablero2IsCancelador = c.Tablero1IsCancelador,
                Tablero2IsBendito = c.Tablero1IsBendito,
                Tablero2IsCongelado = c.Tablero1IsCongelado,
                Tablero2IsHielo = c.Tablero1IsHielo,
                Tablero2IsFuego = c.Tablero1IsFuego
            }).ToList();
                

            PuntosTablero2 = mensaje.PuntosTablero1;
            Tablero2Estrella = mensaje.Tablero1Estrella;

            // Intercambiar propiedades entre Tablero2 y Tablero1
            Tablero1 = mensaje.Tablero2.Select(c => new Tablero1Celda
            {
                Tablero1PosicionEjeX = c.Tablero2PosicionEjeX,
                Tablero1PosicionEjeY = c.Tablero2PosicionEjeY,
                Tablero1TieneNum = c.Tablero2TieneNum,
                Tablero1ImagenDeEjemplo = c.Tablero2ImagenDeEjemplo,
                Tablero1IsEscudo = c.Tablero2IsEscudo,
                Tablero1IsMaldito = c.Tablero2IsMaldito,
                Tablero1IsInverso = c.Tablero2IsInverso,
                Tablero1IsExplosivo = c.Tablero2IsExplosivo,
                Tablero1IsCancelador = c.Tablero2IsCancelador,
                Tablero1IsBendito = c.Tablero2IsBendito,
                Tablero1IsCongelado = c.Tablero2IsCongelado,
                Tablero1IsHielo = c.Tablero2IsHielo,
                Tablero1IsFuego = c.Tablero2IsFuego
            }).ToList();

            PuntosTablero1 = mensaje.PuntosTablero2;
            Tablero1Estrella = mensaje.Tablero2Estrella;

            // Actualizar la interfaz de usuario con los nuevos valores
            lblPuntosTablero2.Content = PuntosTablero2.ToString();

            lblPuntosTablero1.Content = PuntosTablero1.ToString();

            RellenarEstrellasTablero1();
            RellenarEstrellasTablero2();

            tempo.Foreground = Brushes.White;
            contador = 11;
            actualizarDado(0);
            btnLanzarDado.Visibility = Visibility.Visible;
            imagenDadoGIF.Visibility = Visibility.Visible;
            btnLanzarDado.IsEnabled = true;
            turno = true;
            if (PrimerJugador)
            {
                switch (climaActual)
                {
                    case "Sol":
                        Sol();
                        break;
                    case "Nieve":
                        Nieve();
                        break;
                    default:
                        break;
                }
            }
            iniciarSistemaTurnos();
        }

        //Método para cuando nieve.
        public async void Nieve()
        {
            if (timerNieve == 0)
            {
                timerNieve = 4;
                foreach (Tablero1Celda Tablero1celda in Tablero1)
                {
                    if (Tablero1celda.Tablero1IsCongelado)
                    {
                        playSfx(8);
                        for (int i = 0; i < 5; i++)
                        {
                            Tablero1celda.Tablero1IsCongelado = true;
                            Tablero1celda.Tablero1IsHielo = false;
                            await Task.Delay(20);
                            Tablero1celda.Tablero1IsCongelado = false;
                            Tablero1celda.Tablero1IsHielo = true;
                            await Task.Delay(20);
                        }
                        break;
                    }
                }
                foreach (Tablero2Celda Tablero2celda in Tablero2)
                {
                    if (Tablero2celda.Tablero2IsCongelado)
                    {
                        playSfx(8);
                        for (int i = 0; i < 5; i++)
                        {
                            Tablero2celda.Tablero2IsCongelado = true;
                            Tablero2celda.Tablero2IsHielo = false;
                            await Task.Delay(20);
                            Tablero2celda.Tablero2IsCongelado = false;
                            Tablero2celda.Tablero2IsHielo = true;
                            await Task.Delay(20);
                        }
                        break;
                    }
                }
            }
            timerNieve--;
            if (timerNieve == 3)
            {
                Random rnd = new Random();
                int dadoCongelado = 0;
                do
                {
                    foreach (Tablero1Celda Tablero1celda in Tablero1)
                    {
                        int NumCelda = rnd.Next(1, 26);
                        if (NumCelda * 100 / 26 >= 90 && dadoCongelado < 1 && !Tablero1celda.Tablero1TieneNum == false
                            && !Tablero1celda.Tablero1ImagenDeEjemplo.Equals("🗲")
                            && !Tablero1celda.Tablero1ImagenDeEjemplo.Equals(" ")
                            && !Tablero1celda.Tablero1IsHielo == true)
                        {
                            if (Tablero1celda.Tablero1IsCancelador)
                            {
                                Tablero1celda.Tablero1IsCancelador = false;
                                CalcularPuntuacionJugador2();
                                lblPuntosTablero2.Content = PuntosTablero2.ToString();
                                playSfx(4);
                            }
                            else
                            {
                                BorrarTipoDeDadoTablero1(Tablero1celda);
                            }
                            playSfx(9);
                            for (int i = 0; i < 5; i++)
                            {
                                Tablero1celda.Tablero1IsCongelado = false;
                                await Task.Delay(20);
                                Tablero1celda.Tablero1IsCongelado = true;
                                await Task.Delay(20);
                            }
                            dadoCongelado = 2;
                        }
                    }
                } while (dadoCongelado < 1);
                dadoCongelado = 0;
                do
                {
                    foreach (Tablero2Celda Tablero2celda in Tablero2)
                    {
                        int NumCelda = rnd.Next(1, 26);
                        if (NumCelda * 100 / 26 >= 90 && dadoCongelado < 1 && !Tablero2celda.Tablero2TieneNum == false
                            && !Tablero2celda.Tablero2ImagenDeEjemplo.Equals("🗲")
                            && !Tablero2celda.Tablero2ImagenDeEjemplo.Equals(" ")
                            && !Tablero2celda.Tablero2IsHielo == true)
                        {
                            if (Tablero2celda.Tablero2IsCancelador)
                            {
                                Tablero2celda.Tablero2IsCancelador = false;
                                CalcularPuntuacionJugador1();
                                lblPuntosTablero1.Content = PuntosTablero1.ToString();
                                playSfx(4);
                            }
                            else
                            {
                                BorrarTipoDeDadoTablero2(Tablero2celda);
                            }
                            playSfx(9);
                            for (int i = 0; i < 5; i++)
                            {
                                Tablero2celda.Tablero2IsCongelado = false;
                                await Task.Delay(20);
                                Tablero2celda.Tablero2IsCongelado = true;
                                await Task.Delay(20);
                            }
                            dadoCongelado = 2;
                        }
                    }
                } while (dadoCongelado < 1);
            }
        }

        //Método para cuando haga sol.
        public async void Sol()
        {
            if (timerSol == 0)
            {
                timerSol = 4;
                foreach (Tablero1Celda Tablero1celda in Tablero1)
                {
                    if (Tablero1celda.Tablero1IsFuego)
                    {
                        Tablero1celda.Tablero1IsFuego = false;
                        dadoBorradoTablero1(Tablero1celda);
                        Tablero1celda.Tablero1TieneNum = false;
                        CalcularPuntuacionJugador1();
                        lblPuntosTablero1.Content = PuntosTablero1.ToString();
                        break;
                    }
                }
                foreach (Tablero2Celda Tablero2celda in Tablero2)
                {
                    if (Tablero2celda.Tablero2IsFuego)
                    {
                        Tablero2celda.Tablero2IsFuego = false;
                        dadoBorradoTablero2(Tablero2celda);
                        Tablero2celda.Tablero2TieneNum = false;
                        CalcularPuntuacionJugador2();
                        lblPuntosTablero2.Content = PuntosTablero2.ToString();
                        break;
                    }
                }
            }
            timerSol--;
            if (timerSol == 3)
            {
                Random rnd = new Random();
                int dadoQuemado = 0;
                do
                {
                    foreach (Tablero1Celda Tablero1celda in Tablero1)
                    {
                        int NumCelda = rnd.Next(1, 26);
                        if (NumCelda * 100 / 26 >= 90 && dadoQuemado < 1 && !Tablero1celda.Tablero1TieneNum == false
                            && !Tablero1celda.Tablero1ImagenDeEjemplo.Equals("🗲")
                            && !Tablero1celda.Tablero1ImagenDeEjemplo.Equals(" "))
                        {
                            if (Tablero1celda.Tablero1IsCancelador)
                            {
                                Tablero1celda.Tablero1IsCancelador = false;
                                CalcularPuntuacionJugador2();
                                lblPuntosTablero2.Content = PuntosTablero2.ToString();
                                playSfx(4);
                            }
                            else if (Tablero1celda.Tablero1IsExplosivo)
                            {
                                dadoBorradoTablero1(Tablero1celda);
                                Tablero1celda.Tablero1IsExplosivo = false;
                                Tablero1celda.Tablero1TieneNum = false;
                                BorrarLineaTablero2(Tablero1celda);
                                CalcularPuntuacionJugador1();
                                lblPuntosTablero1.Content = PuntosTablero1.ToString();
                                dadoQuemado = 2;
                                break;
                            }
                            else
                            {
                                BorrarTipoDeDadoTablero1(Tablero1celda);
                            }
                            playSfx(10);
                            for (int i = 0; i < 5; i++)
                            {
                                Tablero1celda.Tablero1IsFuego = false;
                                await Task.Delay(20);
                                Tablero1celda.Tablero1IsFuego = true;
                                await Task.Delay(20);
                            }
                            dadoQuemado = 2;
                        }
                    }
                } while (dadoQuemado < 1);
                dadoQuemado = 0;
                do
                {
                    foreach (Tablero2Celda Tablero2celda in Tablero2)
                    {
                        int NumCelda = rnd.Next(1, 26);
                        if (NumCelda * 100 / 26 >= 90 && dadoQuemado < 1 && !Tablero2celda.Tablero2TieneNum == false
                            && !Tablero2celda.Tablero2ImagenDeEjemplo.Equals("🗲")
                            && !Tablero2celda.Tablero2ImagenDeEjemplo.Equals(" "))
                        {
                            if (Tablero2celda.Tablero2IsCancelador)
                            {
                                Tablero2celda.Tablero2IsCancelador = false;
                                CalcularPuntuacionJugador1();
                                lblPuntosTablero1.Content = PuntosTablero1.ToString();
                                playSfx(4);
                            }
                            else if (Tablero2celda.Tablero2IsExplosivo)
                            {
                                dadoBorradoTablero2(Tablero2celda);
                                Tablero2celda.Tablero2IsExplosivo = false;
                                Tablero2celda.Tablero2TieneNum = false;
                                BorrarLineaTablero1(Tablero2celda);
                                CalcularPuntuacionJugador2();
                                lblPuntosTablero2.Content = PuntosTablero2.ToString();
                                dadoQuemado = 2;
                                break;
                            }
                            else
                            {
                                BorrarTipoDeDadoTablero2(Tablero2celda);
                            }
                            playSfx(10);
                            for (int i = 0; i < 5; i++)
                            {
                                Tablero2celda.Tablero2IsFuego = false;
                                await Task.Delay(20);
                                Tablero2celda.Tablero2IsFuego = true;
                                await Task.Delay(20);
                            }
                            dadoQuemado = 2;
                        }
                    }
                } while (dadoQuemado < 1);
            }
        }

        // Método para enviar mensajes al oponente.
        private void EnviarMensaje()
        {
            try
            {
                if (ClienteSocket != null && ClienteSocket.Connected)
                {
                    relojgif.Visibility = Visibility.Collapsed;
                    tempo.Visibility = Visibility.Collapsed;
                    lblTurnoJugador1.Visibility = Visibility.Collapsed;
                    lblTurnoJugador2.Visibility = Visibility.Visible;
                    var estado = new JuegoEstado
                    {
                        Tablero1 = Tablero1,
                        PuntosTablero1 = PuntosTablero1,
                        Tablero1Estrella = Tablero1Estrella,
                        Tablero2 = Tablero2,
                        PuntosTablero2 = PuntosTablero2,
                        Tablero2Estrella = Tablero2Estrella,
                        desconexion = desconexion
                    };

                    string jsonString = JsonConvert.SerializeObject(estado);
                    var mensajeBytes = Encoding.UTF8.GetBytes(jsonString);
                    byte[] tamanoMensajeBytes = BitConverter.GetBytes(mensajeBytes.Length);
                    btnLanzarDado.IsEnabled = false;
                    turno = false;

                    ClienteSocket.Send(tamanoMensajeBytes);
                    ClienteSocket.Send(mensajeBytes);
                }
                else
                {
                    //MessageBox.Show("El socket no está conectado.");
                    //salir();
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message);
                //salir();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el mensaje: " + ex.Message);
                //salir();
            }
        }

        //Método para crear el entorno.
        public void CrearEntorno()
        {
            List<Entorno> MyList = new List<Entorno>();
            for (int i = 0; i < PersonajeFilas; i++)
            {
                for (int o = 0; o < PersonajeColumnas; o++)
                {
                    if (Clima.Contains("Nieve") || Clima.Contains("nieve") || Clima.Contains("granizo") || Clima.Contains("Granizo"))
                    {
                        MyList.Add(new Entorno { EntornoPosicionEjeX = i, EntornoPosicionEjey = o, entorno = "Imagenes/invierno.gif" });
                        climaActual = "Nieve";
                    }
                    else if (Clima.Contains("Soleado") || Clima.Contains("soleado"))
                    {
                        MyList.Add(new Entorno { EntornoPosicionEjeX = i, EntornoPosicionEjey = o, entorno = "Imagenes/desierto.gif" });
                        climaActual = "Sol";
                    }
                    else
                    {
                        MyList.Add(new Entorno { EntornoPosicionEjeX = i, EntornoPosicionEjey = o, entorno = "Imagenes/Pradera.gif" });
                    }
                }
            }
            ListaEntorno = MyList;
        }


        //Método para crear al enemigo.
        public void CrearEnemigo()
        {

            List<Enemigo> MyList = new List<Enemigo>();

            for (int i = 0; i < EnemigoFilas; i++)
            {
                for (int o = 0; o < EnemigoColumnas; o++)
                {
                    switch (ENEMIGONOMBRE)
                    {
                        case "El Cordero":
                            MyList.Add(new Enemigo { EnemigoPosicionEjeX = i, EnemigoPosicionEjey = o, enemigo = "Imagenes/Cordero.gif" });
                            lblNombreEnemigo.Content = "El Cordero";
                            break;
                        case "El Penitente":
                            MyList.Add(new Enemigo { EnemigoPosicionEjeX = i, EnemigoPosicionEjey = o, enemigo = "Imagenes/Penitente.gif" });
                            lblNombreEnemigo.Content = "El Penitente";
                            break;
                        case "Shovel Knight":
                            MyList.Add(new Enemigo { EnemigoPosicionEjeX = i, EnemigoPosicionEjey = o, enemigo = "Imagenes/ShovelKnight.gif" });
                            lblNombreEnemigo.Content = "Shovel Knight";
                            break;
                        case "Flowey":
                            MyList.Add(new Enemigo { EnemigoPosicionEjeX = i, EnemigoPosicionEjey = o, enemigo = "Imagenes/Flowey.gif" });
                            lblNombreEnemigo.Content = "Flowey";
                            break;
                        case "Badeline":
                            MyList.Add(new Enemigo { EnemigoPosicionEjeX = i, EnemigoPosicionEjey = o, enemigo = "Imagenes/Badeline.gif" });
                            lblNombreEnemigo.Content = "Badeline";
                            break;
                        case "Capitan Viridian":
                            MyList.Add(new Enemigo { EnemigoPosicionEjeX = i, EnemigoPosicionEjey = o, enemigo = "Imagenes/CaptainViridian.gif" });
                            lblNombreEnemigo.Content = "Capitan Viridian";
                            break;
                        case "Isaac":
                            MyList.Add(new Enemigo { EnemigoPosicionEjeX = i, EnemigoPosicionEjey = o, enemigo = "Imagenes/Isaac.gif" });
                            lblNombreEnemigo.Content = "Isaac";
                            break;
                        case "Pingüino":
                            MyList.Add(new Enemigo { EnemigoPosicionEjeX = i, EnemigoPosicionEjey = o, enemigo = "Imagenes/ClubPenguin.gif" });
                            lblNombreEnemigo.Content = "Pingüino";
                            break;
                        case "Dr Fetus":
                            MyList.Add(new Enemigo { EnemigoPosicionEjeX = i, EnemigoPosicionEjey = o, enemigo = "Imagenes/DrFetus.gif" });
                            lblNombreEnemigo.Content = "Dr Fetus";
                            break;
                        case "Zero":
                            MyList.Add(new Enemigo { EnemigoPosicionEjeX = i, EnemigoPosicionEjey = o, enemigo = "Imagenes/TacaXeraca.gif" });
                            lblNombreEnemigo.Content = "Zero";
                            break;
                    }
                }
                ListaEnemigo = MyList;
            }
        }

        //Método para crear al personaje.
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


        //Método para crear el tablero.
        public void CrearTablero()
        {
            List<Tablero1Celda> MyList = new List<Tablero1Celda>();
            List<Tablero2Celda> MyList2 = new List<Tablero2Celda>();
            for (int i = 0; i < Tablero1Filas; i++)
            {
                for (int o = 0; o < Tablero1Columnas; o++)
                {
                    MyList.Add(new Tablero1Celda { Tablero1PosicionEjeX = i, Tablero1PosicionEjeY = o, Tablero1ImagenDeEjemplo = " " });
                    MyList2.Add(new Tablero2Celda { Tablero2PosicionEjeX = i, Tablero2PosicionEjeY = o, Tablero2ImagenDeEjemplo = " " });
                }
            }
            Tablero1 = MyList;
            Tablero2 = MyList2;
        }

        //Iniciar la ejecucion del sistema de turnos.
        private void iniciarSistemaTurnos()
        {
            sistemaTurnos = new Thread(turnoJugador);
            sistemaTurnos.Start();
        }

        //Sistema de turnos.
        void turnoJugador()
        {
            try
            {
                turnoActual = turno;
                if (turno)
                {
                    Dispatcher.Invoke(() =>
                    {
                        relojgif.Visibility = Visibility.Visible;
                        tempo.Visibility = Visibility.Visible;
                        lblTurnoJugador1.Visibility = Visibility.Visible;
                        lblTurnoJugador2.Visibility = Visibility.Collapsed;
                    });
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        relojgif.Visibility = Visibility.Collapsed;
                        tempo.Visibility = Visibility.Collapsed;
                        lblTurnoJugador1.Visibility = Visibility.Collapsed;
                        lblTurnoJugador2.Visibility = Visibility.Visible;
                    });
                }
                Dispatcher.Invoke(() =>
                {
                    tempo.Content = "10";
                    tempo.Foreground = Brushes.White;
                    btnLanzarDado.IsEnabled = turno;
                    btnLanzarDado.Visibility = Visibility.Visible;
                    imagenDadoGIF.Visibility = Visibility.Visible;
                });
                for (contador = 10; contador > 0; contador--)
                {
                    Dispatcher.Invoke(() =>
                    {
                        tempo.Content = contador.ToString();
                        if (contador <= 3)
                        {
                            playSfx(1);
                            tempo.Foreground = Brushes.Red;
                        }
                    });
                    Thread.Sleep(1000);
                    if ((contador >= 0 && turnoActual != turno) || !ClienteSocket.Connected)
                    {
                        break;
                    }
                }
                if (contador == 0)
                {
                    Random rnd = new Random();
                    Dispatcher.Invoke(() =>
                    {
                        btnLanzarDado.IsEnabled = false;
                        tempo.Content = "0";
                        botonCancelar(false);
                        visibilidadDadoSanto(false);
                        habilitarDados(false);
                    });
                    if (NumRandom == 0)
                    {
                        NumRandom = rnd.Next(1, 17);
                        Dispatcher.Invoke(() =>
                        {
                            actualizarDado(NumRandom);
                        });
                        Thread.Sleep(1000);
                    }
                    if (turnoActual)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            autoDado(false, rnd);
                            CalcularPuntuacionJugador1();
                            lblPuntosTablero1.Content = PuntosTablero1.ToString();
                            btnLanzarDado.Visibility = Visibility.Collapsed;
                            NumRandom = 0;
                            actualizarDado(NumRandom);
                            RellenarEstrellasTablero1();
                            CambiarColorBtn();
                            foreach (Tablero1Celda tablero1Celda in Tablero1)
                            {
                                if (!tablero1Celda.Tablero1ImagenDeEjemplo.Equals(" "))
                                {
                                    CondicionDeVictoria++;
                                }
                            }
                            comprobacionVictoria();
                            if (!(CondicionDeVictoria == 25) && dobleturno == false)
                            {
                                CondicionDeVictoria = 0;
                                turno = false;
                                EnviarMensaje();
                            }
                        });
                    }
                    else
                    {
                        Dispatcher.Invoke(() =>
                        {
                            autoDado(true, rnd);
                            CalcularPuntuacionJugador2();
                            lblPuntosTablero2.Content = PuntosTablero2.ToString();
                            btnLanzarDado.Visibility = Visibility.Collapsed;
                            NumRandom = 0;
                            actualizarDado(NumRandom);
                            RellenarEstrellasTablero2();
                            CambiarColorBtn();
                            foreach (Tablero2Celda tablero2Celda in Tablero2)
                            {
                                if (!tablero2Celda.Tablero2ImagenDeEjemplo.Equals(" "))
                                {
                                    CondicionDeVictoria++;
                                }
                            }
                            comprobacionVictoria();
                            turno = true;
                        });
                    }
                }
            }
            catch (Exception e)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        //Método para colocar el dado en un punto del tablero de forma automática.
        private void autoDado(Boolean IA, Random rnd)
        {
            if (IA)
            {
                do
                {
                    foreach (Tablero2Celda tablero2celda in Tablero2)
                    {
                        int NumCelda = rnd.Next(1, 26);
                        if (NumCelda * 100 / 26 >= 90 && tablero2celda.Tablero2TieneNum == false && HaPuestoNumero < 1)
                        {
                            tablero2celda.Tablero2ImagenDeEjemplo = NumRandom.ToString();
                            tablero2celda.Tablero2TieneNum = true;
                            foreach (Tablero1Celda tablero1Celda in Tablero1)
                            {
                                if (tablero2celda.Tablero2PosicionEjeX == tablero1Celda.Tablero1PosicionEjeX && tablero1Celda.Tablero1ImagenDeEjemplo.Equals(tablero2celda.Tablero2ImagenDeEjemplo)
                                    && !(tablero1Celda.Tablero1TieneNum == false || tablero2celda.Tablero2TieneNum == false) &&
                                    (tablero1Celda.Tablero1IsEscudo || tablero1Celda.Tablero1IsCancelador || tablero1Celda.Tablero1IsExplosivo || tablero1Celda.Tablero1IsHielo))
                                {
                                    //aqui va lo que sucedera cada vez que borre un dado
                                    if (tablero1Celda.Tablero1IsEscudo)
                                    {
                                        dadoBorradoTablero1(tablero1Celda);
                                    }
                                    else if (tablero1Celda.Tablero1IsCancelador)
                                    {
                                        dadoBorradoTablero1(tablero1Celda);
                                        tablero1Celda.Tablero1IsCancelador = false;
                                        tablero1Celda.Tablero1TieneNum = false;
                                        playSfx(4);
                                    }
                                    else if (tablero1Celda.Tablero1IsExplosivo)
                                    {
                                        dadoBorradoTablero1(tablero1Celda);
                                        tablero1Celda.Tablero1IsExplosivo = false;
                                        tablero1Celda.Tablero1TieneNum = false;
                                        CalcularPuntuacionJugador1();
                                        lblPuntosTablero1.Content = PuntosTablero1.ToString();
                                        BorrarLineaTablero2(tablero1Celda);
                                    }
                                }
                                else if (tablero2celda.Tablero2PosicionEjeX == tablero1Celda.Tablero1PosicionEjeX && tablero1Celda.Tablero1ImagenDeEjemplo.Equals(tablero2celda.Tablero2ImagenDeEjemplo)
                                    && !(tablero1Celda.Tablero1TieneNum == false || tablero2celda.Tablero2TieneNum == false))
                                {
                                    BorrarTipoDeDadoTablero1(tablero1Celda);
                                    dadoBorradoTablero1(tablero1Celda);
                                    tablero1Celda.Tablero1TieneNum = false;
                                    CalcularPuntuacionJugador1();
                                    lblPuntosTablero1.Content = PuntosTablero1.ToString();
                                    playSfx(11);
                                    if (Tablero2Estrella != 10)
                                    {
                                        Tablero2Estrella++;
                                    }
                                }
                            }
                            HaPuestoNumero = 2;
                            TipoDeDado = "";
                        }
                    }
                } while (HaPuestoNumero < 1);
                HaPuestoNumero = 0;
            }
            else
            {
                do
                {
                    foreach (Tablero1Celda tablero1Celda in Tablero1)
                    {
                        int NumCelda = rnd.Next(1, 26);
                        if (NumCelda * 100 / 26 >= 90 && tablero1Celda.Tablero1TieneNum == false && HaPuestoNumero < 1)
                        {
                            tablero1Celda.Tablero1ImagenDeEjemplo = NumRandom.ToString();
                            tablero1Celda.Tablero1TieneNum = true;
                            if (TipoDeDado.Equals("Santo"))
                            {
                                Santo(tablero1Celda);
                            }
                            foreach (Tablero2Celda tablero2Celda in Tablero2)
                            {
                                if (tablero1Celda.Tablero1PosicionEjeX == tablero2Celda.Tablero2PosicionEjeX && tablero2Celda.Tablero2ImagenDeEjemplo.Equals(tablero1Celda.Tablero1ImagenDeEjemplo)
                                    && !(tablero1Celda.Tablero1TieneNum == false || tablero2Celda.Tablero2TieneNum == false) &&
                                    (tablero2Celda.Tablero2IsEscudo || tablero2Celda.Tablero2IsCancelador || tablero2Celda.Tablero2IsExplosivo || tablero2Celda.Tablero2IsHielo))
                                {
                                    //aqui va lo que sucedera cada vez que borre un dado
                                    if (tablero2Celda.Tablero2IsEscudo)
                                    {
                                        dadoBorradoTablero2(tablero2Celda);
                                    }
                                    else if (tablero2Celda.Tablero2IsCancelador)
                                    {
                                        dadoBorradoTablero2(tablero2Celda);
                                        tablero2Celda.Tablero2IsCancelador = false;
                                        tablero2Celda.Tablero2TieneNum = false;
                                        playSfx(4);
                                    }
                                    else if (tablero2Celda.Tablero2IsExplosivo)
                                    {
                                        dadoBorradoTablero2(tablero2Celda);
                                        tablero2Celda.Tablero2IsExplosivo = false;
                                        tablero2Celda.Tablero2TieneNum = false;
                                        CalcularPuntuacionJugador2();
                                        lblPuntosTablero2.Content = PuntosTablero2.ToString();
                                        BorrarLineaTablero1(tablero2Celda);
                                    }
                                }
                                else if (tablero1Celda.Tablero1PosicionEjeX == tablero2Celda.Tablero2PosicionEjeX && tablero2Celda.Tablero2ImagenDeEjemplo.Equals(tablero1Celda.Tablero1ImagenDeEjemplo)
                                    && !(tablero1Celda.Tablero1TieneNum == false || tablero2Celda.Tablero2TieneNum == false))
                                {
                                    BorrarTipoDeDadoTablero2(tablero2Celda);
                                    dadoBorradoTablero2(tablero2Celda);
                                    tablero2Celda.Tablero2TieneNum = false;
                                    CalcularPuntuacionJugador2();
                                    lblPuntosTablero2.Content = PuntosTablero2.ToString();
                                    playSfx(11);
                                    if (Tablero1Estrella != 10)
                                    {
                                        Tablero1Estrella++;
                                    }
                                }
                            }
                            HaPuestoNumero = 2;
                            TipoDeDado = "";
                        }
                    }
                } while (HaPuestoNumero < 1);
                HaPuestoNumero = 0;
            }
        }
        
        //Método para que el jugador haga todo lo que tenga que hacer en su tablero.
        private void ClicIzqTablero1(object sender, MouseButtonEventArgs e)
        {
            Button BtnIzq = (Button)sender;
            Tablero1Celda tablero1celda = (Tablero1Celda)BtnIzq.DataContext;
            dobleturno = false;
            if (turno == true && tablero1celda.Tablero1ImagenDeEjemplo == " " && !(NumRandom == 0)
                && ActivarMaldito == false && tablero1celda.Tablero1TieneNum == false)
            {
                btnLanzarDado.IsEnabled = false;
                CambiarColorBtn();
                switch (TipoDeDado)
                {
                    case "Escudo":
                        Escudo(tablero1celda);
                        break;
                    case "Invertido":
                        Invertido(tablero1celda);
                        HacerInverso(tablero1celda);
                        playSfx(2);
                        break;
                    case "Doble":
                        Doble();
                        break;
                    case "Cancelador":
                        Cancelador(tablero1celda);
                        playSfx(3);
                        break;
                    case "Santo":
                        Santo(tablero1celda);
                        break;
                    case "Explosivo":
                        Explosivo(tablero1celda);
                        break;
                }
                TipoDeDado = "";
                tablero1celda.Tablero1ImagenDeEjemplo = NumRandom.ToString();
                tablero1celda.Tablero1TieneNum = true;
                botonCancelar(false);
                visibilidadDadoSanto(false);
                habilitarDados(false);
                //Comprobar dados y restar puntuación del enemigo.
                foreach (Tablero2Celda tablero2celda in Tablero2)
                {
                    if (tablero1celda.Tablero1PosicionEjeX == tablero2celda.Tablero2PosicionEjeX && tablero2celda.Tablero2ImagenDeEjemplo.Equals(tablero1celda.Tablero1ImagenDeEjemplo) &&
                        !(tablero1celda.Tablero1TieneNum == false || tablero2celda.Tablero2TieneNum == false) &&
                        (tablero2celda.Tablero2IsEscudo || tablero2celda.Tablero2IsCancelador || tablero2celda.Tablero2IsExplosivo || tablero2celda.Tablero2IsHielo))
                    {
                        //aqui va lo que sucedera cada vez que borre un dado
                        if (tablero2celda.Tablero2IsEscudo)
                        {
                            dadoBorradoTablero2(tablero2celda);
                        }
                        else if (tablero2celda.Tablero2IsCancelador)
                        {
                            dadoBorradoTablero2(tablero2celda);
                            tablero2celda.Tablero2IsCancelador = false;
                            tablero2celda.Tablero2TieneNum = false;
                            playSfx(4);
                        }
                        else if (tablero2celda.Tablero2IsExplosivo)
                        {
                            dadoBorradoTablero2(tablero2celda);
                            tablero2celda.Tablero2IsExplosivo = false;
                            tablero2celda.Tablero2TieneNum = false;
                            CalcularPuntuacionJugador2();
                            lblPuntosTablero2.Content = PuntosTablero2.ToString();
                            BorrarLineaTablero1(tablero2celda);
                        }
                    }
                    else if (tablero1celda.Tablero1PosicionEjeX == tablero2celda.Tablero2PosicionEjeX && tablero2celda.Tablero2ImagenDeEjemplo.Equals(tablero1celda.Tablero1ImagenDeEjemplo)
                        && !(tablero1celda.Tablero1TieneNum == false || tablero2celda.Tablero2TieneNum == false))
                    {
                        BorrarTipoDeDadoTablero2(tablero2celda);
                        dadoBorradoTablero2(tablero2celda);
                        tablero2celda.Tablero2TieneNum = false;
                        tablero2celda.Tablero2ImagenDeEjemplo = " ";
                        CalcularPuntuacionJugador2();
                        lblPuntosTablero2.Content = PuntosTablero2.ToString();
                        playSfx(11);
                        if (Tablero1Estrella != 10)
                        {
                            Tablero1Estrella++;
                        }
                    }
                }
                CalcularPuntuacionJugador1();
                RellenarEstrellasTablero1();
                lblPuntosTablero1.Content = PuntosTablero1.ToString();
                if (tablero1celda.Tablero1IsCancelador)
                {
                    CalcularPuntuacionJugador2();
                    lblPuntosTablero2.Content = PuntosTablero2.ToString();
                }
                btnLanzarDado.Visibility = Visibility.Collapsed;
                NumRandom = 0;
                actualizarDado(NumRandom);
                foreach (Tablero1Celda tablero1Celda in Tablero1)
                {
                    if (!tablero1Celda.Tablero1ImagenDeEjemplo.Equals(" "))
                    {
                        CondicionDeVictoria++;
                    }
                }
                comprobacionVictoria();
                CondicionDeVictoria = 0;
                if (!(CondicionDeVictoria == 25) && dobleturno == false)
                {
                    CondicionDeVictoria = 0;
                    turno = false;
                    EnviarMensaje();
                }
                else if (!(CondicionDeVictoria == 25) && dobleturno == true)
                {
                    tempo.Foreground = Brushes.White;
                    contador = 11;
                    actualizarDado(0);
                    btnLanzarDado.Visibility = Visibility.Visible;
                    imagenDadoGIF.Visibility = Visibility.Visible;
                    btnLanzarDado.IsEnabled = true;
                    dobleturno = false;
                }
            }
        }

        //Método para que el jugador coloque un número dentro del tablero del enemigo.
        public void ClicIzqTablero2(object sender, MouseButtonEventArgs e)
        {
            Button BtnIzq = (Button)sender;
            Tablero2Celda tablero2celda = (Tablero2Celda)BtnIzq.DataContext;
            if (ActivarMaldito && tablero2celda.Tablero2ImagenDeEjemplo.Equals(" ")
                && !NumRandom.Equals(0) && tablero2celda.Tablero2TieneNum == false)
            {
                btnLanzarDado.IsEnabled = false;
                CambiarColorBtn();
                tablero2celda.Tablero2ImagenDeEjemplo = NumRandom.ToString();
                tablero2celda.Tablero2TieneNum = true;
                Maldito(tablero2celda);
                RellenarEstrellasTablero1();
                botonCancelar(false);
                habilitarDados(false);
                foreach (Tablero1Celda tablero1Celda in Tablero1)
                {
                    if (tablero2celda.Tablero2PosicionEjeX == tablero1Celda.Tablero1PosicionEjeX && tablero1Celda.Tablero1ImagenDeEjemplo.Equals(tablero2celda.Tablero2ImagenDeEjemplo)
                        && !(tablero1Celda.Tablero1TieneNum == false || tablero2celda.Tablero2TieneNum == false) &&
                        (tablero1Celda.Tablero1IsEscudo || tablero1Celda.Tablero1IsCancelador || tablero1Celda.Tablero1IsExplosivo || tablero1Celda.Tablero1IsHielo))
                    {
                        //aqui va lo que sucedera cada vez que borre un dado
                        if (tablero1Celda.Tablero1IsEscudo)
                        {
                            dadoBorradoTablero1(tablero1Celda);
                        }
                        else if (tablero1Celda.Tablero1IsCancelador)
                        {
                            dadoBorradoTablero1(tablero1Celda);
                            tablero1Celda.Tablero1IsCancelador = false;
                            tablero1Celda.Tablero1TieneNum = false;
                            playSfx(4);
                        }
                        else if (tablero1Celda.Tablero1IsExplosivo)
                        {
                            dadoBorradoTablero1(tablero1Celda);
                            tablero1Celda.Tablero1IsExplosivo = false;
                            tablero1Celda.Tablero1TieneNum = false;
                            CalcularPuntuacionJugador1();
                            lblPuntosTablero1.Content = PuntosTablero1.ToString();
                            BorrarLineaTablero2(tablero1Celda);
                        }
                    }
                    else if (tablero2celda.Tablero2PosicionEjeX == tablero1Celda.Tablero1PosicionEjeX && tablero1Celda.Tablero1ImagenDeEjemplo.Equals(tablero2celda.Tablero2ImagenDeEjemplo)
                        && !(tablero1Celda.Tablero1TieneNum == false || tablero2celda.Tablero2TieneNum == false))
                    {
                        BorrarTipoDeDadoTablero1(tablero1Celda);
                        dadoBorradoTablero1(tablero1Celda);
                        tablero1Celda.Tablero1TieneNum = false;
                        CalcularPuntuacionJugador1();
                        lblPuntosTablero1.Content = PuntosTablero1.ToString();
                        playSfx(11);
                        if (Tablero2Estrella != 10)
                        {
                            Tablero2Estrella++;
                        }
                    }
                }
                CalcularPuntuacionJugador2();
                RellenarEstrellasTablero2();
                lblPuntosTablero2.Content = PuntosTablero2.ToString();
                if (tablero2celda.Tablero2IsCancelador)
                {
                    CalcularPuntuacionJugador1();
                    lblPuntosTablero1.Content = PuntosTablero1.ToString();
                }
                btnLanzarDado.Visibility = Visibility.Collapsed;
                NumRandom = 0;
                actualizarDado(NumRandom);
                foreach (Tablero2Celda tablero2 in Tablero2)
                {
                    if (!tablero2.Tablero2ImagenDeEjemplo.Equals(" "))
                    {
                        CondicionDeVictoria++;
                    }
                }
                comprobacionVictoria();
                CondicionDeVictoria = 0;
                ActivarMaldito = false;
                turno = false;
                EnviarMensaje();
            }
        }

        //Botón para utilizar el dado Santo
        private void DadoSanto(object sender, RoutedEventArgs e)
        {
            ActivarMaldito = false;
            TipoDeDado = "";
            CambiarColorBtn();
            if (Tablero1Estrella >= 5)
            {
                botonCancelar(true);
                visibilidadDadoSanto(true);
                habilitarBotonesDadoSanto();
                BtnSanto.Background = Brushes.Green;
            }
            else
            {
                BtnSanto.Background = Brushes.Red;
            }
        }

        //Botón para utilizar el dado Explosivo
        private void DadoExplosivo(object sender, RoutedEventArgs e)
        {
            ActivarMaldito = false;
            TipoDeDado = "";
            CambiarColorBtn();
            visibilidadDadoSanto(false);
            if (Tablero1Estrella >= 3)
            {
                TipoDeDado = "Explosivo";
                botonCancelar(true);
                BtnExplosivo.Background = Brushes.Green;
            }
            else
            {
                BtnExplosivo.Background = Brushes.Red;
            }
        }

        //Boton para confirmar la seleccion del dado santo
        private void ConfirmarSeleccion(object sender, RoutedEventArgs e)
        {
            int n = 0;
            Tablero1Estrella -= 5;
            int.TryParse(lblNumeroDadoSanto.Content.ToString(), out n);
            NumRandom = n;
            actualizarDado(NumRandom);
            TipoDeDado = "Santo";
            RellenarEstrellasTablero1();
            visibilidadDadoSanto(false);
            botonCancelar(false);
            habilitarDados(false);
            playSfx(6);
        }

        //Botón para utilizar el dado Cancelador
        private void DadoCancelador(object sender, RoutedEventArgs e)
        {
            ActivarMaldito = false;
            TipoDeDado = "";
            CambiarColorBtn();
            visibilidadDadoSanto(false);
            if (Tablero1Estrella >= 4)
            {
                TipoDeDado = "Cancelador";
                botonCancelar(true);
                BtnCancelador.Background = Brushes.Green;
            }
            else
            {
                BtnCancelador.Background = Brushes.Red;
            }
        }

        //Botón para utilizar el dado Doble
        private void DadoDoble(object sender, RoutedEventArgs e)
        {
            ActivarMaldito = false;
            TipoDeDado = "";
            CambiarColorBtn();
            visibilidadDadoSanto(false);
            if (Tablero1Estrella >= 3)
            {
                TipoDeDado = "Doble";
                botonCancelar(true);
                BtnDoble.Background = Brushes.Green;
            }
            else
            {
                BtnDoble.Background = Brushes.Red;
            }
        }

        //Botón para utilizar el dado Invertido
        private void DadoInvertido(object sender, RoutedEventArgs e)
        {
            ActivarMaldito = false;
            TipoDeDado = "";
            CambiarColorBtn();
            visibilidadDadoSanto(false);
            if (Tablero1Estrella >= 2)
            {
                TipoDeDado = "Invertido";
                botonCancelar(true);
                BtnInvertido.Background = Brushes.Green;
            }
            else
            {
                BtnInvertido.Background = Brushes.Red;
            }
        }

        //Botón para utilizar el dado Maldito
        private void DadoMaldito(object sender, RoutedEventArgs e)
        {
            TipoDeDado = "";
            CambiarColorBtn();
            visibilidadDadoSanto(false);
            if (Tablero1Estrella >= 2)
            {
                TipoDeDado = "Maldito";
                ActivarMaldito = true;
                botonCancelar(true);
                BtnMaldito.Background = Brushes.Green;
            }
            else
            {
                BtnMaldito.Background = Brushes.Red;
            }
        }

        //Botón para utilizar el dado escudo
        private void DadoEscudo(object sender, RoutedEventArgs e)
        {
            ActivarMaldito = false;
            TipoDeDado = "";
            CambiarColorBtn();
            visibilidadDadoSanto(false);
            if (Tablero1Estrella >= 1)
            {
                TipoDeDado = "Escudo";
                botonCancelar(true);
                BtnEscudo.Background = Brushes.Green;
            }
            else
            {
                BtnEscudo.Background = Brushes.Red;
            }
        }

        //Botón para lanzar el dado
        private void LanzarDado(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            NumRandom = rnd.Next(1, 17);
            actualizarDado(NumRandom);
            btnLanzarDado.IsEnabled = false;
            habilitarDados(true);
        }

        //Método para utilizar el dado maldito
        public void Maldito(Tablero2Celda BtnCelda)
        {
            Tablero1Estrella -= 2;
            BtnCelda.Tablero2IsMaldito = true;
        }

        //Método para borrar la linea entera cuando se borra un dado explosivo en el Tablero 1
        public void BorrarLineaTablero2(Tablero1Celda BtnCelda)
        {
            foreach (Tablero2Celda tablero2celda in Tablero2)
            {
                if (tablero2celda.Tablero2PosicionEjeX == BtnCelda.Tablero1PosicionEjeX)
                {
                    BorrarTipoDeDadoTablero2(tablero2celda);
                    dadoBorradoTablero2(tablero2celda);
                    tablero2celda.Tablero2TieneNum = false;
                }
            }
            playSfx(5);
            CalcularPuntuacionJugador2();
            lblPuntosTablero2.Content = PuntosTablero2.ToString();
        }

        //Método para actualizar la imagen del dado
        public void actualizarDado(int numero)
        {
            imagenDadoGIF.Visibility = Visibility.Collapsed;
            imagenDadoPNG.Visibility = Visibility.Visible;
            switch (numero)
            {
                case 0:
                    imagenDadoPNG.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    imagenDadoPNG.Source = new BitmapImage(new Uri("Imagenes/dice1.png", UriKind.Relative));
                    break;
                case 2:
                    imagenDadoPNG.Source = new BitmapImage(new Uri("Imagenes/dice2.png", UriKind.Relative));
                    break;
                case 3:
                    imagenDadoPNG.Source = new BitmapImage(new Uri("Imagenes/dice3.png", UriKind.Relative));
                    break;
                case 4:
                    imagenDadoPNG.Source = new BitmapImage(new Uri("Imagenes/dice4.png", UriKind.Relative));
                    break;
                case 5:
                    imagenDadoPNG.Source = new BitmapImage(new Uri("Imagenes/dice5.png", UriKind.Relative));
                    break;
                case 6:
                    imagenDadoPNG.Source = new BitmapImage(new Uri("Imagenes/dice6.png", UriKind.Relative));
                    break;
                case 7:
                    imagenDadoPNG.Source = new BitmapImage(new Uri("Imagenes/dice7.png", UriKind.Relative));
                    break;
                case 8:
                    imagenDadoPNG.Source = new BitmapImage(new Uri("Imagenes/dice8.png", UriKind.Relative));
                    break;
                case 9:
                    imagenDadoPNG.Source = new BitmapImage(new Uri("Imagenes/dice9.png", UriKind.Relative));
                    break;
                case 10:
                    imagenDadoPNG.Source = new BitmapImage(new Uri("Imagenes/dice10.png", UriKind.Relative));
                    break;
                case 11:
                    imagenDadoPNG.Source = new BitmapImage(new Uri("Imagenes/dice11.png", UriKind.Relative));
                    break;
                case 12:
                    imagenDadoPNG.Source = new BitmapImage(new Uri("Imagenes/dice12.png", UriKind.Relative));
                    break;
                case 13:
                    imagenDadoPNG.Source = new BitmapImage(new Uri("Imagenes/dice13.png", UriKind.Relative));
                    break;
                case 14:
                    imagenDadoPNG.Source = new BitmapImage(new Uri("Imagenes/dice14.png", UriKind.Relative));
                    break;
                case 15:
                    imagenDadoPNG.Source = new BitmapImage(new Uri("Imagenes/dice15.png", UriKind.Relative));
                    break;
                case 16:
                    imagenDadoPNG.Source = new BitmapImage(new Uri("Imagenes/dice16.png", UriKind.Relative));
                    break;
            }
        }

        //Método para comprobar si se cumplen los requisitos de victoria
        private void comprobacionVictoria()
        {
            if (CondicionDeVictoria == 25)
            {
                juegoTerminado = true;
                if (turno)
                {
                    turno = false;
                }
                else
                {
                    turno = true;
                }
                tempo.Content = "0";
                if (PuntosTablero1 > PuntosTablero2)
                {
                    MessageBox.Show("¡¡Has ganado " + PERSONAJENOMBRE + "!!");
                    //stopSoundtrack();
                    //MainWindow mainWindow = new MainWindow(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, existeclima, volumenvar);
                    //this.Close();
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();
                    //GC.Collect();
                    //mainWindow.Show();
                    salir();
                }
                else
                {
                    MessageBox.Show("¡¡Has perdido, suerte para la proxima " + PERSONAJENOMBRE + "!!");
                    //stopSoundtrack();
                    //MainWindow mainWindow = new MainWindow(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, existeclima, volumenvar);
                    //this.Close();
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();
                    //GC.Collect();
                    //mainWindow.Show();
                    salir();
                }
            }
        }

        //Método para cambiar el tipo de dado al ser borrado y dejar la celda vacia en el tablero 2.
        public void BorrarTipoDeDadoTablero2(Tablero2Celda BtnCelda)
        {
            BtnCelda.Tablero2IsEscudo = false;
            BtnCelda.Tablero2IsMaldito = false;
            BtnCelda.Tablero2IsInverso = false;
            BtnCelda.Tablero2IsCancelador = false;
            BtnCelda.Tablero2IsBendito = false;
            BtnCelda.Tablero2IsExplosivo = false;
            BtnCelda.Tablero2IsCongelado = false;
            BtnCelda.Tablero2IsHielo = false;
            BtnCelda.Tablero2IsFuego = false;
        }

        //Método para borrar la linea entera cuando se borra un dado explosivo en el Tablero 2.
        public void BorrarLineaTablero1(Tablero2Celda BtnCelda)
        {
            foreach (Tablero1Celda tablero1celda in Tablero1)
            {
                if (tablero1celda.Tablero1PosicionEjeX == BtnCelda.Tablero2PosicionEjeX)
                {
                    BorrarTipoDeDadoTablero1(tablero1celda);
                    dadoBorradoTablero1(tablero1celda);
                    tablero1celda.Tablero1TieneNum = false;
                }
            }
            playSfx(5);
            CalcularPuntuacionJugador1();
            lblPuntosTablero1.Content = PuntosTablero1.ToString();
        }

        //Calcular putuación del tablero 1.
        private void CalcularPuntuacionJugador1()
        {
            try
            {
                List<int> valoresZelda = new List<int>();
                int zelda = 0, puntuacionTotal = 0, valorMultiplicacion = 1;

                foreach (Tablero1Celda tablero1celdaPuntuacion in Tablero1)
                {
                    if (!tablero1celdaPuntuacion.Tablero1TieneNum == false && !tablero1celdaPuntuacion.Tablero1ImagenDeEjemplo.Equals("🗲")
                         && !tablero1celdaPuntuacion.Tablero1ImagenDeEjemplo.Equals(" "))
                    {
                        valoresZelda.Add(int.Parse(tablero1celdaPuntuacion.Tablero1ImagenDeEjemplo));
                    }
                    zelda++;
                    if (zelda == 5)
                    {
                        List<int> valoresYaRepetidos = new List<int>();
                        int repeticiones = 0;

                        foreach (int valor in valoresZelda)
                        {
                            if (!valoresYaRepetidos.Contains(valor))
                            {
                                foreach (int valorRepetido in valoresZelda)
                                {
                                    if (valor == valorRepetido)
                                    {
                                        repeticiones++;
                                        if (repeticiones == 2)
                                        {
                                            valoresYaRepetidos.Add(valor);
                                        }
                                    }
                                }
                                if (repeticiones < 2 || valor == 1 || filaCanceladaTablero1(tablero1celdaPuntuacion) == true)
                                {
                                    while (valoresYaRepetidos.Contains(valor))
                                    {
                                        valoresYaRepetidos.Remove(valor);
                                    }
                                    puntuacionTotal += valor;
                                }
                                else
                                {
                                    for (int i = 0; i < repeticiones; i++)
                                    {
                                        valorMultiplicacion *= valor;
                                    }
                                    puntuacionTotal += valorMultiplicacion;
                                }
                            }
                            valorMultiplicacion = 1;
                            repeticiones = 0;
                        }
                        valoresYaRepetidos.Clear();
                        valoresZelda.Clear();
                        zelda = 0;
                    }
                }
                PuntosTablero1 = puntuacionTotal;
                visualizarPuntuacion(1);
                puntuacionTotal = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error en la gestión de la memoria. La aplicación se va a cerrar ahora.");
                this.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        //Efecto de parpadeo para los dados borrados en el tablero 1.
        private async void dadoBorradoTablero1(Tablero1Celda Tablero1celda)
        {
            if (Tablero1celda.Tablero1IsEscudo)
            {
                for (int i = 0; i < 5; i++)
                {
                    Tablero1celda.Tablero1IsEscudo = false;
                    await Task.Delay(20);
                    Tablero1celda.Tablero1IsEscudo = true;
                    await Task.Delay(20);
                    Tablero1celda.Tablero1IsEscudo = false;
                }
            }
            else
            {
                string numeroActual = Tablero1celda.Tablero1ImagenDeEjemplo.ToString();
                for (int i = 0; i < 5; i++)
                {
                    Tablero1celda.Tablero1ImagenDeEjemplo = " ";
                    await Task.Delay(20);
                    Tablero1celda.Tablero1ImagenDeEjemplo = numeroActual;
                    await Task.Delay(20);
                    Tablero1celda.Tablero1ImagenDeEjemplo = " ";
                }
            }
        }

        //Método para cambiar el tipo de dado al ser borrado y dejar la celda vacia en el tablero 1.
        public void BorrarTipoDeDadoTablero1(Tablero1Celda BtnCelda)
        {
            BtnCelda.Tablero1IsEscudo = false;
            BtnCelda.Tablero1IsMaldito = false;
            BtnCelda.Tablero1IsInverso = false;
            BtnCelda.Tablero1IsCancelador = false;
            BtnCelda.Tablero1IsBendito = false;
            BtnCelda.Tablero1IsExplosivo = false;
            BtnCelda.Tablero1IsCongelado = false;
            BtnCelda.Tablero1IsHielo = false;
            BtnCelda.Tablero1IsFuego = false;
        }

        //Calcular puntuacion del tablero 2.
        private void CalcularPuntuacionJugador2()
        {
            try
            {
                List<int> valoresZelda = new List<int>();
                int zelda = 0, puntuacionTotal = 0, valorMultiplicacion = 1;

                foreach (Tablero2Celda tablero2celdaPuntuacion in Tablero2)
                {
                    if (!tablero2celdaPuntuacion.Tablero2TieneNum == false && !tablero2celdaPuntuacion.Tablero2ImagenDeEjemplo.Equals("🗲")
                         && !tablero2celdaPuntuacion.Tablero2ImagenDeEjemplo.Equals(" "))
                    {
                        valoresZelda.Add(int.Parse(tablero2celdaPuntuacion.Tablero2ImagenDeEjemplo));
                    }
                    zelda++;
                    if (zelda == 5)
                    {
                        List<int> valoresYaRepetidos = new List<int>();
                        int repeticiones = 0;

                        foreach (int valor in valoresZelda)
                        {
                            if (!valoresYaRepetidos.Contains(valor))
                            {
                                foreach (int valorRepetido in valoresZelda)
                                {
                                    if (valor == valorRepetido)
                                    {
                                        repeticiones++;
                                        if (repeticiones == 2)
                                        {
                                            valoresYaRepetidos.Add(valor);
                                        }
                                    }
                                }
                                if (repeticiones < 2 || valor == 1 || filaCanceladaTablero2(tablero2celdaPuntuacion) == true)
                                {
                                    while (valoresYaRepetidos.Contains(valor))
                                    {
                                        valoresYaRepetidos.Remove(valor);
                                    }
                                    puntuacionTotal += valor;
                                }
                                else
                                {
                                    for (int i = 0; i < repeticiones; i++)
                                    {
                                        valorMultiplicacion *= valor;
                                    }
                                    puntuacionTotal += valorMultiplicacion;
                                }
                            }
                            valorMultiplicacion = 1;
                            repeticiones = 0;
                        }
                        valoresYaRepetidos.Clear();
                        valoresZelda.Clear();
                        zelda = 0;
                    }
                }
                PuntosTablero2 = puntuacionTotal;
                visualizarPuntuacion(2);
                puntuacionTotal = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error en la gestión de la memoria. La aplicación se va a cerrar ahora.");
                this.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        //Método para cambiar el tamaño de la puntuación cuantos más puntos se obtienen.
        private void visualizarPuntuacion(int jugador)
        {
            if (jugador == 1)
            {
                switch (PuntosTablero1)
                {
                    case int p when (p < 100):
                        lblPuntosTablero1.Foreground = Brushes.White;
                        break;
                    case int p when (p >= 100 && p < 999):
                        lblPuntosTablero1.Foreground = Brushes.LightYellow;
                        break;
                    case int p when (p >= 1000 && p < 9999):
                        lblPuntosTablero1.Foreground = Brushes.Yellow;
                        break;
                    case int p when (p >= 10000 && p < 99999):
                        lblPuntosTablero1.Foreground = Brushes.Orange;
                        break;
                    case int p when (p >= 100000 && p < 999999):
                        lblPuntosTablero1.Foreground = Brushes.Red;
                        break;
                    case int p when (p >= 1000000):
                        lblPuntosTablero1.Foreground = Brushes.DarkRed;
                        break;
                }
            }
            else if (jugador == 2)
            {
                switch (PuntosTablero2)
                {
                    case int p when (p < 100):
                        lblPuntosTablero2.Foreground = Brushes.White;
                        break;
                    case int p when (p >= 100 && p < 999):
                        lblPuntosTablero2.Foreground = Brushes.LightYellow;
                        break;
                    case int p when (p >= 1000 && p < 9999):
                        lblPuntosTablero2.Foreground = Brushes.Yellow;
                        break;
                    case int p when (p >= 10000 && p < 99999):
                        lblPuntosTablero2.Foreground = Brushes.Orange;
                        break;
                    case int p when (p >= 100000 && p < 999999):
                        lblPuntosTablero2.Foreground = Brushes.Red;
                        break;
                    case int p when (p >= 1000000):
                        lblPuntosTablero2.Foreground = Brushes.DarkRed;
                        break;
                }
            }
        }

        //Método para determinar si existe un dado cancelador en la fila adyacente del Tablero 1.
        private Boolean filaCanceladaTablero1(Tablero1Celda filaTablero1)
        {
            foreach (Tablero2Celda Tablero2celda in Tablero2)
            {
                if (Tablero2celda.Tablero2PosicionEjeX == filaTablero1.Tablero1PosicionEjeX
                    && Tablero2celda.Tablero2IsCancelador)
                {
                    return true;
                }
            }
            return false;
        }

        //Método para determinar si existe un dado cancelador en la fila adyacente del Tablero 2.
        private Boolean filaCanceladaTablero2(Tablero2Celda filaTablero2)
        {
            foreach (Tablero1Celda Tablero1celda in Tablero1)
            {
                if (Tablero1celda.Tablero1PosicionEjeX == filaTablero2.Tablero2PosicionEjeX
                    && Tablero1celda.Tablero1IsCancelador)
                {
                    return true;
                }
            }
            return false;
        }

        //Efecto de parpadeo para los dados borrados en el tablero 2.
        private async void dadoBorradoTablero2(Tablero2Celda Tablero2celda)
        {
            if (Tablero2celda.Tablero2IsEscudo)
            {
                for (int i = 0; i < 5; i++)
                {
                    Tablero2celda.Tablero2IsEscudo = false;
                    await Task.Delay(20);
                    Tablero2celda.Tablero2IsEscudo = true;
                    await Task.Delay(20);
                    Tablero2celda.Tablero2IsEscudo = false;
                }
            }
            else
            {
                string numeroActual = Tablero2celda.Tablero2ImagenDeEjemplo.ToString();
                for (int i = 0; i < 5; i++)
                {
                    Tablero2celda.Tablero2ImagenDeEjemplo = " ";
                    await Task.Delay(20);
                    Tablero2celda.Tablero2ImagenDeEjemplo = numeroActual;
                    await Task.Delay(20);
                    Tablero2celda.Tablero2ImagenDeEjemplo = " ";
                }
            }
        }

        //Método para utilizar el dado Explosivo.
        public void Explosivo(Tablero1Celda BtnCelda)
        {
            Tablero1Estrella -= 3;
            BtnCelda.Tablero1IsExplosivo = true;
        }

        //Método para utilizar el dado Santo.
        public void Santo(Tablero1Celda BtnCelda)
        {
            BtnCelda.Tablero1IsBendito = true;
        }

        //Método para utilizar el dado Cancelador.
        public void Cancelador(Tablero1Celda BtnCelda)
        {
            Tablero1Estrella -= 4;
            BtnCelda.Tablero1IsCancelador = true;
        }

        //Método para utilizar el dado Doble.
        public void Doble()
        {
            Tablero1Estrella -= 3;
            dobleturno = true;
        }

        //Método para realizar el inverso del dado.
        public void HacerInverso(Tablero1Celda BtnCelda)
        {
            switch (NumRandom)
            {
                case 1:
                    NumRandom = 16;
                    break;
                case 2:
                    NumRandom = 15;
                    break;
                case 3:
                    NumRandom = 14;
                    break;
                case 4:
                    NumRandom = 13;
                    break;
                case 5:
                    NumRandom = 12;
                    break;
                case 6:
                    NumRandom = 11;
                    break;
                case 7:
                    NumRandom = 10;
                    break;
                case 8:
                    NumRandom = 9;
                    break;
                case 9:
                    NumRandom = 8;
                    break;
                case 10:
                    NumRandom = 7;
                    break;
                case 11:
                    NumRandom = 6;
                    break;
                case 12:
                    NumRandom = 5;
                    break;
                case 13:
                    NumRandom = 4;
                    break;
                case 14:
                    NumRandom = 3;
                    break;
                case 15:
                    NumRandom = 2;
                    break;
                case 16:
                    NumRandom = 1;
                    break;
            }
        }

        //Método para utilizar el dado invertido.
        public void Invertido(Tablero1Celda BtnCelda)
        {
            Tablero1Estrella -= 2;
            BtnCelda.Tablero1IsInverso = true;
        }

        //Método para utilizar el dado Escudo.
        public void Escudo(Tablero1Celda BtnCelda)
        {
            Tablero1Estrella -= 1;
            BtnCelda.Tablero1IsEscudo = true;
        }

        //Botón para salir al menú principal.
        private void Salir(object sender, RoutedEventArgs e)
        {
            salir();
        }

        //Método para salir.
        public void salir() 
        {
            if (PrimerJugador)
            {
                desconexion = true;
                EnviarMensaje();
                Thread.Sleep(2000);
            }
            else
            {
                desconexion = true;
                EnviarMensaje();
                Thread.Sleep(500);
            }
            
            ClienteSocket.Close();
            stopSoundtrack();
            MainWindow mainWindow = new MainWindow(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, existeclima, volumenvar);
            this.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainWindow.Show();
        }

        //Método para rellenar las estrellas en el tablero 2.
        public void RellenarEstrellasTablero2()
        {
            switch (Tablero2Estrella)
            {
                case 0:
                    ETablero2Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 1:
                    ETablero2Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 2:
                    ETablero2Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 3:
                    ETablero2Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 4:
                    ETablero2Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 5:
                    ETablero2Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 6:
                    ETablero2Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 7:
                    ETablero2Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 8:
                    ETablero2Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero2Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 9:
                    ETablero2Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                default:
                    ETablero2Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero2Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    break;
            }
        }

        //Boton para incrementar el valor del dado santo.
        private void IncrementarValor(object sender, RoutedEventArgs e)
        {
            valorDadoSanto++;
            lblNumeroDadoSanto.Content = valorDadoSanto.ToString();
            habilitarBotonesDadoSanto();
        }

        //Boton para decrementar el valor del dado santo.
        private void DecrementarValor(object sender, RoutedEventArgs e)
        {
            valorDadoSanto--;
            lblNumeroDadoSanto.Content = valorDadoSanto.ToString();
            habilitarBotonesDadoSanto();
        }

        //Boton para cancelar la seleccion de cualquiera de los dados especiales.
        private void CancelarSeleccion(object sender, RoutedEventArgs e)
        {
            CambiarColorBtn();
            visibilidadDadoSanto(false);
            botonCancelar(false);
            ActivarMaldito = false;
            TipoDeDado = "";
        }

        //Método para poner el color de los botones a gris.
        public void CambiarColorBtn()
        {
            //BtnEscudo.Source = new BitmapImage(new Uri("Imagenes/dadoEscudoEnabled.png", UriKind.Relative));
            BtnEscudo.Background = Brushes.Transparent;
            BtnMaldito.Background = Brushes.Transparent;
            BtnInvertido.Background = Brushes.Transparent;
            BtnDoble.Background = Brushes.Transparent;
            BtnCancelador.Background = Brushes.Transparent;
            BtnSanto.Background = Brushes.Transparent;
            BtnExplosivo.Background = Brushes.Transparent;
        }

        //Habilitar o deshabilitar los botones del dado santo
        private void habilitarBotonesDadoSanto()
        {
            if (valorDadoSanto == 16)
            {
                btnIncrementarValor.IsEnabled = false;
                btnDecrementarValor.IsEnabled = true;
                ImagenDecrementarValor.Source = new BitmapImage(new Uri("Imagenes/botonPaAbajo.png", UriKind.Relative));
                ImagenIncrementarValor.Source = new BitmapImage(new Uri("Imagenes/botonPaArribaDissabled.png", UriKind.Relative));
            }
            else if (valorDadoSanto == 1)
            {
                btnDecrementarValor.IsEnabled = false;
                btnIncrementarValor.IsEnabled = true;
                ImagenDecrementarValor.Source = new BitmapImage(new Uri("Imagenes/botonPaAbajoDissabled.png", UriKind.Relative));
                ImagenIncrementarValor.Source = new BitmapImage(new Uri("Imagenes/botonPaArriba.png", UriKind.Relative));
            }
            else
            {
                btnIncrementarValor.IsEnabled = true;
                btnDecrementarValor.IsEnabled = true;
                ImagenDecrementarValor.Source = new BitmapImage(new Uri("Imagenes/botonPaAbajo.png", UriKind.Relative));
                ImagenIncrementarValor.Source = new BitmapImage(new Uri("Imagenes/botonPaArriba.png", UriKind.Relative));
            }
        }

        //Metodo para habilitar o deshabilitar los botones
        public void habilitarDados(Boolean habilitar)
        {
            BtnEscudo.IsEnabled = habilitar;
            BtnMaldito.IsEnabled = habilitar;
            BtnInvertido.IsEnabled = habilitar;
            BtnDoble.IsEnabled = habilitar;
            BtnCancelador.IsEnabled = habilitar;
            BtnSanto.IsEnabled = habilitar;
            BtnExplosivo.IsEnabled = habilitar;
            if (habilitar)
            {
                if (turno)
                {
                    switch (Tablero1Estrella)
                    {
                        case 0:
                            //ImagenBtnEscudo.Source = new BitmapImage(new Uri("Imagenes/dadoEscudoCrossed.png", UriKind.Relative));
                            //ImagenBtnMortero.Source = new BitmapImage(new Uri("Imagenes/dadoMorteroCrossed.png", UriKind.Relative));
                            //ImagenBtnInverso.Source = new BitmapImage(new Uri("Imagenes/dadoInversoCrossed.png", UriKind.Relative));
                            //ImagenBtnDoble.Source = new BitmapImage(new Uri("Imagenes/dadoDobleCrossed.png", UriKind.Relative));
                            //ImagenBtnExplosivo.Source = new BitmapImage(new Uri("Imagenes/dadoExplosivoCrossed.png", UriKind.Relative));
                            //ImagenBtnCancelador.Source = new BitmapImage(new Uri("Imagenes/dadoCanceladorCrossed.png", UriKind.Relative));
                            //ImagenBtnSanto.Source = new BitmapImage(new Uri("Imagenes/dadoSantoCrossed.png", UriKind.Relative));
                            break;
                        case 1:
                            ImagenBtnEscudo.Source = new BitmapImage(new Uri("Imagenes/dadoEscudoEnabled.png", UriKind.Relative));
                            //ImagenBtnMortero.Source = new BitmapImage(new Uri("Imagenes/dadoMorteroCrossed.png", UriKind.Relative));
                            //ImagenBtnInverso.Source = new BitmapImage(new Uri("Imagenes/dadoInversoCrossed.png", UriKind.Relative));
                            //ImagenBtnDoble.Source = new BitmapImage(new Uri("Imagenes/dadoDobleCrossed.png", UriKind.Relative));
                            //ImagenBtnExplosivo.Source = new BitmapImage(new Uri("Imagenes/dadoExplosivoCrossed.png", UriKind.Relative));
                            //ImagenBtnCancelador.Source = new BitmapImage(new Uri("Imagenes/dadoCanceladorCrossed.png", UriKind.Relative));
                            //ImagenBtnSanto.Source = new BitmapImage(new Uri("Imagenes/dadoSantoCrossed.png", UriKind.Relative));
                            break;
                        case 2:
                            ImagenBtnEscudo.Source = new BitmapImage(new Uri("Imagenes/dadoEscudoEnabled.png", UriKind.Relative));
                            ImagenBtnMortero.Source = new BitmapImage(new Uri("Imagenes/dadoMorteroEnabled.png", UriKind.Relative));
                            ImagenBtnInverso.Source = new BitmapImage(new Uri("Imagenes/dadoInversoEnabled.png", UriKind.Relative));
                            //ImagenBtnDoble.Source = new BitmapImage(new Uri("Imagenes/dadoDobleCrossed.png", UriKind.Relative));
                            //ImagenBtnExplosivo.Source = new BitmapImage(new Uri("Imagenes/dadoExplosivoCrossed.png", UriKind.Relative));
                            //ImagenBtnCancelador.Source = new BitmapImage(new Uri("Imagenes/dadoCanceladorCrossed.png", UriKind.Relative));
                            //ImagenBtnSanto.Source = new BitmapImage(new Uri("Imagenes/dadoSantoCrossed.png", UriKind.Relative));
                            break;
                        case 3:
                            ImagenBtnEscudo.Source = new BitmapImage(new Uri("Imagenes/dadoEscudoEnabled.png", UriKind.Relative));
                            ImagenBtnMortero.Source = new BitmapImage(new Uri("Imagenes/dadoMorteroEnabled.png", UriKind.Relative));
                            ImagenBtnInverso.Source = new BitmapImage(new Uri("Imagenes/dadoInversoEnabled.png", UriKind.Relative));
                            ImagenBtnDoble.Source = new BitmapImage(new Uri("Imagenes/dadoDobleEnabled.png", UriKind.Relative));
                            ImagenBtnExplosivo.Source = new BitmapImage(new Uri("Imagenes/dadoExplosivoEnabled.png", UriKind.Relative));
                            //ImagenBtnCancelador.Source = new BitmapImage(new Uri("Imagenes/dadoCanceladorCrossed.png", UriKind.Relative));
                            //ImagenBtnSanto.Source = new BitmapImage(new Uri("Imagenes/dadoSantoCrossed.png", UriKind.Relative));
                            break;
                        case 4:
                            ImagenBtnEscudo.Source = new BitmapImage(new Uri("Imagenes/dadoEscudoEnabled.png", UriKind.Relative));
                            ImagenBtnMortero.Source = new BitmapImage(new Uri("Imagenes/dadoMorteroEnabled.png", UriKind.Relative));
                            ImagenBtnInverso.Source = new BitmapImage(new Uri("Imagenes/dadoInversoEnabled.png", UriKind.Relative));
                            ImagenBtnDoble.Source = new BitmapImage(new Uri("Imagenes/dadoDobleEnabled.png", UriKind.Relative));
                            ImagenBtnExplosivo.Source = new BitmapImage(new Uri("Imagenes/dadoExplosivoEnabled.png", UriKind.Relative));
                            ImagenBtnCancelador.Source = new BitmapImage(new Uri("Imagenes/dadoCanceladorEnabled.png", UriKind.Relative));
                            //ImagenBtnSanto.Source = new BitmapImage(new Uri("Imagenes/dadoSantoCrossed.png", UriKind.Relative));
                            break;
                        default:
                            ImagenBtnEscudo.Source = new BitmapImage(new Uri("Imagenes/dadoEscudoEnabled.png", UriKind.Relative));
                            ImagenBtnMortero.Source = new BitmapImage(new Uri("Imagenes/dadoMorteroEnabled.png", UriKind.Relative));
                            ImagenBtnInverso.Source = new BitmapImage(new Uri("Imagenes/dadoInversoEnabled.png", UriKind.Relative));
                            ImagenBtnDoble.Source = new BitmapImage(new Uri("Imagenes/dadoDobleEnabled.png", UriKind.Relative));
                            ImagenBtnExplosivo.Source = new BitmapImage(new Uri("Imagenes/dadoExplosivoEnabled.png", UriKind.Relative));
                            ImagenBtnCancelador.Source = new BitmapImage(new Uri("Imagenes/dadoCanceladorEnabled.png", UriKind.Relative));
                            ImagenBtnSanto.Source = new BitmapImage(new Uri("Imagenes/dadoSantoEnabled.png", UriKind.Relative));
                            break;
                    }
                }
                else
                {
                    switch (Tablero2Estrella)
                    {
                        case 0:
                            //ImagenBtnEscudo.Source = new BitmapImage(new Uri("Imagenes/dadoEscudoCrossed.png", UriKind.Relative));
                            //ImagenBtnMortero.Source = new BitmapImage(new Uri("Imagenes/dadoMorteroCrossed.png", UriKind.Relative));
                            //ImagenBtnInverso.Source = new BitmapImage(new Uri("Imagenes/dadoInversoCrossed.png", UriKind.Relative));
                            //ImagenBtnDoble.Source = new BitmapImage(new Uri("Imagenes/dadoDobleCrossed.png", UriKind.Relative));
                            //ImagenBtnExplosivo.Source = new BitmapImage(new Uri("Imagenes/dadoExplosivoCrossed.png", UriKind.Relative));
                            //ImagenBtnCancelador.Source = new BitmapImage(new Uri("Imagenes/dadoCanceladorCrossed.png", UriKind.Relative));
                            //ImagenBtnSanto.Source = new BitmapImage(new Uri("Imagenes/dadoSantoCrossed.png", UriKind.Relative));
                            break;
                        case 1:
                            ImagenBtnEscudo.Source = new BitmapImage(new Uri("Imagenes/dadoEscudoEnabled.png", UriKind.Relative));
                            //ImagenBtnMortero.Source = new BitmapImage(new Uri("Imagenes/dadoMorteroCrossed.png", UriKind.Relative));
                            //ImagenBtnInverso.Source = new BitmapImage(new Uri("Imagenes/dadoInversoCrossed.png", UriKind.Relative));
                            //ImagenBtnDoble.Source = new BitmapImage(new Uri("Imagenes/dadoDobleCrossed.png", UriKind.Relative));
                            //ImagenBtnExplosivo.Source = new BitmapImage(new Uri("Imagenes/dadoExplosivoCrossed.png", UriKind.Relative));
                            //ImagenBtnCancelador.Source = new BitmapImage(new Uri("Imagenes/dadoCanceladorCrossed.png", UriKind.Relative));
                            //ImagenBtnSanto.Source = new BitmapImage(new Uri("Imagenes/dadoSantoCrossed.png", UriKind.Relative));
                            break;
                        case 2:
                            ImagenBtnEscudo.Source = new BitmapImage(new Uri("Imagenes/dadoEscudoEnabled.png", UriKind.Relative));
                            ImagenBtnMortero.Source = new BitmapImage(new Uri("Imagenes/dadoMorteroEnabled.png", UriKind.Relative));
                            ImagenBtnInverso.Source = new BitmapImage(new Uri("Imagenes/dadoInversoEnabled.png", UriKind.Relative));
                            //ImagenBtnDoble.Source = new BitmapImage(new Uri("Imagenes/dadoDobleCrossed.png", UriKind.Relative));
                            //ImagenBtnExplosivo.Source = new BitmapImage(new Uri("Imagenes/dadoExplosivoCrossed.png", UriKind.Relative));
                            //ImagenBtnCancelador.Source = new BitmapImage(new Uri("Imagenes/dadoCanceladorCrossed.png", UriKind.Relative));
                            //ImagenBtnSanto.Source = new BitmapImage(new Uri("Imagenes/dadoSantoCrossed.png", UriKind.Relative));
                            break;
                        case 3:
                            ImagenBtnEscudo.Source = new BitmapImage(new Uri("Imagenes/dadoEscudoEnabled.png", UriKind.Relative));
                            ImagenBtnMortero.Source = new BitmapImage(new Uri("Imagenes/dadoMorteroEnabled.png", UriKind.Relative));
                            ImagenBtnInverso.Source = new BitmapImage(new Uri("Imagenes/dadoInversoEnabled.png", UriKind.Relative));
                            ImagenBtnDoble.Source = new BitmapImage(new Uri("Imagenes/dadoDobleEnabled.png", UriKind.Relative));
                            ImagenBtnExplosivo.Source = new BitmapImage(new Uri("Imagenes/dadoExplosivoEnabled.png", UriKind.Relative));
                            //ImagenBtnCancelador.Source = new BitmapImage(new Uri("Imagenes/dadoCanceladorCrossed.png", UriKind.Relative));
                            //ImagenBtnSanto.Source = new BitmapImage(new Uri("Imagenes/dadoSantoCrossed.png", UriKind.Relative));
                            break;
                        case 4:
                            ImagenBtnEscudo.Source = new BitmapImage(new Uri("Imagenes/dadoEscudoEnabled.png", UriKind.Relative));
                            ImagenBtnMortero.Source = new BitmapImage(new Uri("Imagenes/dadoMorteroEnabled.png", UriKind.Relative));
                            ImagenBtnInverso.Source = new BitmapImage(new Uri("Imagenes/dadoInversoEnabled.png", UriKind.Relative));
                            ImagenBtnDoble.Source = new BitmapImage(new Uri("Imagenes/dadoDobleEnabled.png", UriKind.Relative));
                            ImagenBtnExplosivo.Source = new BitmapImage(new Uri("Imagenes/dadoExplosivoEnabled.png", UriKind.Relative));
                            ImagenBtnCancelador.Source = new BitmapImage(new Uri("Imagenes/dadoCanceladorEnabled.png", UriKind.Relative));
                            //ImagenBtnSanto.Source = new BitmapImage(new Uri("Imagenes/dadoSantoCrossed.png", UriKind.Relative));
                            break;
                        default:
                            ImagenBtnEscudo.Source = new BitmapImage(new Uri("Imagenes/dadoEscudoEnabled.png", UriKind.Relative));
                            ImagenBtnMortero.Source = new BitmapImage(new Uri("Imagenes/dadoMorteroEnabled.png", UriKind.Relative));
                            ImagenBtnInverso.Source = new BitmapImage(new Uri("Imagenes/dadoInversoEnabled.png", UriKind.Relative));
                            ImagenBtnDoble.Source = new BitmapImage(new Uri("Imagenes/dadoDobleEnabled.png", UriKind.Relative));
                            ImagenBtnExplosivo.Source = new BitmapImage(new Uri("Imagenes/dadoExplosivoEnabled.png", UriKind.Relative));
                            ImagenBtnCancelador.Source = new BitmapImage(new Uri("Imagenes/dadoCanceladorEnabled.png", UriKind.Relative));
                            ImagenBtnSanto.Source = new BitmapImage(new Uri("Imagenes/dadoSantoEnabled.png", UriKind.Relative));
                            break;
                    }
                }
            }
            else
            {
                ImagenBtnEscudo.Source = new BitmapImage(new Uri("Imagenes/dadoEscudoDisabled.png", UriKind.Relative));
                ImagenBtnMortero.Source = new BitmapImage(new Uri("Imagenes/dadoMorteroDisabled.png", UriKind.Relative));
                ImagenBtnInverso.Source = new BitmapImage(new Uri("Imagenes/dadoInversoDisabled.png", UriKind.Relative));
                ImagenBtnDoble.Source = new BitmapImage(new Uri("Imagenes/dadoDobleDisabled.png", UriKind.Relative));
                ImagenBtnExplosivo.Source = new BitmapImage(new Uri("Imagenes/dadoExplosivoDisabled.png", UriKind.Relative));
                ImagenBtnCancelador.Source = new BitmapImage(new Uri("Imagenes/dadoCanceladorDisabled.png", UriKind.Relative));
                ImagenBtnSanto.Source = new BitmapImage(new Uri("Imagenes/dadoSantoDisabled.png", UriKind.Relative));
            }
        }

        //Esconder o mostrar el boton de cancelar la seleccion de los dados especiales
        private void botonCancelar(Boolean mostrar)
        {
            if (mostrar)
            {
                btnCancelarSeleccion.Visibility = Visibility.Visible;
            }
            else
            {
                btnCancelarSeleccion.Visibility = Visibility.Collapsed;
            }
        }

        //Esconder o mostrar los controles para el dado santo
        private void visibilidadDadoSanto(Boolean mostrar)
        {
            if (mostrar)
            {
                btnIncrementarValor.Visibility = Visibility.Visible;
                btnDecrementarValor.Visibility = Visibility.Visible;
                btnDadoSantoConfirmarSeleccion.Visibility = Visibility.Visible;
                lblNumeroDadoSanto.Visibility = Visibility.Visible;
                lblEleccionNumero.Visibility = Visibility.Visible;
            }
            else
            {
                btnIncrementarValor.Visibility = Visibility.Collapsed;
                btnDecrementarValor.Visibility = Visibility.Collapsed;
                btnDadoSantoConfirmarSeleccion.Visibility = Visibility.Collapsed;
                lblNumeroDadoSanto.Visibility = Visibility.Collapsed;
                lblEleccionNumero.Visibility = Visibility.Collapsed;
            }
        }

        //Método para rellenar las estrellas en el tablero 1.
        public void RellenarEstrellasTablero1()
        {
            switch (Tablero1Estrella)
            {
                case 0:
                    ETablero1Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 1:
                    ETablero1Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 2:
                    ETablero1Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 3:
                    ETablero1Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 4:
                    ETablero1Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 5:
                    ETablero1Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 6:
                    ETablero1Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 7:
                    ETablero1Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 8:
                    ETablero1Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    ETablero1Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                case 9:
                    ETablero1Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrellaVacia.png", UriKind.Relative));
                    break;
                default:
                    ETablero1Estrella1.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella2.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella3.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella4.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella5.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella6.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella7.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella8.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella9.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    ETablero1Estrella10.Source = new BitmapImage(new Uri("Imagenes/estrella.png", UriKind.Relative));
                    break;
            }
        }

        //Poner musica de fondo
        private void playSoundtrack()
        {
            mediaPlayer = new MediaPlayer();
            Random rnd = new Random();
            int eleccion = rnd.Next(1, 5);
            string rutarelativa = string.Empty;
            string rutaabsoluta = string.Empty;
            switch (eleccion)
            {
                case 1:
                    rutarelativa = "Soundtrack\\SuperMonkeyBall2OST-World8-ClockTower.mp3";
                    rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
                    mediaPlayer.Open(new Uri(rutaabsoluta));
                    break;
                case 2:
                    rutarelativa = "Soundtrack\\SonicCDStardustSpeedwayBadFutureJP-EUHD.mp3";
                    rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
                    mediaPlayer.Open(new Uri(rutaabsoluta));
                    break;
                case 3:
                    rutarelativa = "Soundtrack\\INStage6Theme-Voyage1969.mp3";
                    rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
                    mediaPlayer.Open(new Uri(rutaabsoluta));
                    break;
                case 4:
                    rutarelativa = "Soundtrack\\WEXECUTION.mp3";
                    rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
                    mediaPlayer.Open(new Uri(rutaabsoluta));
                    break;
            }
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

        //Método para poner efectos de sonido
        private void playSfx(int n)
        {
            SoundPlayer player;
            string rutarelativa;
            string rutaabsoluta;
            if (!juegoTerminado)
            {
                switch (n)
                {
                    case 1:
                        rutarelativa = "Sfx\\sfxTimeout.wav";
                        rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
                        player = new SoundPlayer(rutaabsoluta);
                        player.Play();
                        break;
                    case 2:
                        rutarelativa = "Sfx\\sfxInvertido.wav";
                        rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
                        player = new SoundPlayer(rutaabsoluta);
                        player.Play();
                        break;
                    case 3:
                        rutarelativa = "Sfx\\sfxDadoCancelador.wav";
                        rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
                        player = new SoundPlayer(rutaabsoluta);
                        player.Play();
                        break;
                    case 4:
                        rutarelativa = "Sfx\\sfxDadoCanceladorBorrado.wav";
                        rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
                        player = new SoundPlayer(rutaabsoluta);
                        player.Play();
                        break;
                    case 5:
                        rutarelativa = "Sfx\\sfxDadoExplosivo.wav";
                        rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
                        player = new SoundPlayer(rutaabsoluta);
                        player.Play();
                        break;
                    case 6:
                        rutarelativa = "Sfx\\sfxTransfDadoSanto.wav";
                        rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
                        player = new SoundPlayer(rutaabsoluta);
                        player.Play();
                        break;
                    case 7:
                        rutarelativa = "Sfx\\sfxRayo.wav";
                        rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
                        player = new SoundPlayer(rutaabsoluta);
                        player.Play();
                        break;
                    case 8:
                        rutarelativa = "Sfx\\sfxCongelado.wav";
                        rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
                        player = new SoundPlayer(rutaabsoluta);
                        player.Play();
                        break;
                    case 9:
                        rutarelativa = "Sfx\\sfxHelado.wav";
                        rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
                        player = new SoundPlayer(rutaabsoluta);
                        player.Play();
                        break;
                    case 10:
                        rutarelativa = "Sfx\\sfxFuego.wav";
                        rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
                        player = new SoundPlayer(rutaabsoluta);
                        player.Play();
                        break;
                    case 11:
                        rutarelativa = "Sfx\\sfxDadoBorrado.wav";
                        rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
                        player = new SoundPlayer(rutaabsoluta);
                        player.Play();
                        break;
                }
            }
        }

        //Detener el soundtrack cuando se salga de la ventana
        private void stopSoundtrack()
        {
            mediaPlayer.Stop();
            mediaPlayer.Close();
            mediaPlayer = null;
        }
    }
}