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
    public partial class ConectarPvPDiferenteMaquina : Window
    {
        #region Variables varias
        public string QUEPERSONAJEES;
        public string PERSONAJENOMBRE;
        public string ENEMIGONOMBRE;
        public string Clima;
        public MediaPlayer mediaPlayer;
        Socket ServidorSocket;
        Socket ClienteSocket;
        IPEndPoint ep;
        Socket acceptedsocket;
        public bool PrimerJugador;
        public bool existeclima;
        string Climaoponente = "";
        public double volumenvar;
        #endregion



        public ConectarPvPDiferenteMaquina(string personaje, string nombre, string clima, bool personalizarclima,  double Volumen, MediaPlayer mediaPlayer2)
        {
            InitializeComponent();
            //playSoundtrack();
            QUEPERSONAJEES = personaje;
            PERSONAJENOMBRE = nombre;
            Clima = clima;
            existeclima = personalizarclima;
            
            volumenvar = Volumen;
            //playSoundtrack();
            mediaPlayer = mediaPlayer2;
            ep = new IPEndPoint(IPAddress.Any, 9050);
            ServidorSocket = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ClienteSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        //Método para poner musica de fondo
        private void playSoundtrack()
        {
            mediaPlayer = new MediaPlayer();
            string rutarelativa = "Soundtrack\\Pizza Tower OST - Hip to be Italian (Peppino's Final Judgement).mp3";
            string rutaabsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutarelativa);
            mediaPlayer.Open(new Uri(rutaabsoluta));
            
            if (volumenvar > -1)
            {
                mediaPlayer.Volume = volumenvar;
            }
        }

        //Detener el soundtrack cuando se salga de la ventana
        private void stopSoundtrack()
        {
            mediaPlayer.Stop();
            mediaPlayer.Close();
            mediaPlayer = null;
        }

        //Botón para irte hacia la pantalla anterior
        private void Salir(object sender, RoutedEventArgs e)
        {
            ServidorSocket.Close();
            playSfx(2);
            stopSoundtrack();
            MainWindow mainWindow = new MainWindow(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, existeclima, volumenvar);
            this.Close();
            mainWindow.Show();
        }


        //Método para conectarse a un servidor
        public bool conectarse()
        {
            if (!TbColocarIP.Text.Equals(string.Empty))
            {
                try
                {
                    ClienteSocket.Connect(TbColocarIP.Text, 9050);
                    BtnCrearServidor.IsEnabled = false;
                    ImagenBotonJugar.Source = new BitmapImage(new Uri("Imagenes/botonJugarDeshabilitado.png", UriKind.Relative));
                    BtnJugar.IsEnabled = false;

                    //Recibe el mensaje del servidor y manda uno al servidor
                    var buffer = new byte[1_024];
                    int received = ClienteSocket.Receive(buffer);
                    var message = Encoding.UTF8.GetString(buffer, 0, received);
                    string[] mensaje = message.Split(',');
                    Tbchat.Text = "Conexión realizada con exito!\nTu oponente es: " + mensaje[0];
                    Climaoponente = mensaje[1];
                    ENEMIGONOMBRE = message;

                    var EnviarMessage = PERSONAJENOMBRE;
                    var messagebytes = Encoding.UTF8.GetBytes(EnviarMessage);
                    ClienteSocket.Send(messagebytes);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }


        //Botón para conectarse a un servidor
        public async void Jugar(object sender, RoutedEventArgs e)
        {
            if (conectarse())
            {
                //Temporizador de 5 segundo antes de ser redirigido al apartado de jugar
                playSfx(1);
                await Task.Delay(5000);
                stopSoundtrack();
                PrimerJugador = false;
                JugadorvsjugadorDiferenteMaquina jugar = new JugadorvsjugadorDiferenteMaquina(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, ENEMIGONOMBRE, ClienteSocket, PrimerJugador, existeclima, volumenvar, Climaoponente);
                this.Close();
                jugar.Show();
            }
            else
            {
                MessageBox.Show("No se ha podido establecer una conexión con el servidor, asegurese de que la IP proporcionada es la correcta");
            }
        }


        //Método para crear un servidor
        public async void crear()
        {
            ServidorSocket.Bind(ep);
            try
            {
                string h_n = Dns.GetHostName();
                string my_ip = Dns.GetHostEntry(h_n).AddressList[1].ToString();

                ServidorSocket.Listen(10);
                TbCrearServidor.Text = "Se ha creado un servidor! \n Tu dirección de IP es: " + my_ip + " dale esta IP a la persona con la cual desees jugar!";
                BtnCrearServidor.IsEnabled = false;
                BtnJugar.IsEnabled = false;
                TbColocarIP.IsEnabled = false;
                Tbchat.IsEnabled = false;
                ImagenBotonJugar.Source = new BitmapImage(new Uri("Imagenes/botonJugarDeshabilitado.png", UriKind.Relative));
                ImagenBotonCrearPartida.Source = new BitmapImage(new Uri("Imagenes/botonCrearPartidaDeshabilitado.png", UriKind.Relative));

                acceptedsocket = await ServidorSocket.AcceptAsync();

                BtnSalir.IsEnabled = false;

                //Le manda al cliente un mensaje diciendo que la conexión se ha realizado con exito
                var message = PERSONAJENOMBRE + "," + Clima;
                var messagebytes = Encoding.UTF8.GetBytes(message);
                acceptedsocket.Send(messagebytes);

                var buffer = new byte[1_024];
                int received = acceptedsocket.Receive(buffer);
                var Recibirmessage = Encoding.UTF8.GetString(buffer, 0, received);
                Tbchatserv.Text = "Conexión realizada con exito!\nTu oponente es: " + Recibirmessage;
                ENEMIGONOMBRE = Recibirmessage;

                Climaoponente = Clima;

                //Temporizador de 5 segundos antes de ser redirigido al apartado de jugar
                playSfx(1);
                await Task.Delay(5000);
                stopSoundtrack();
                PrimerJugador = true;
                ServidorSocket.Close();
                JugadorvsjugadorDiferenteMaquina jugar = new JugadorvsjugadorDiferenteMaquina(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, ENEMIGONOMBRE, acceptedsocket, PrimerJugador, existeclima, volumenvar, Climaoponente);
                this.Close();
                jugar.Show();
            }
            catch (SocketException)
            {
                ServidorSocket.Close();
            }
        }


        //Botón para crear el servidor
        private void CrearServidor(object sender, RoutedEventArgs e)
        {
            crear();
        }


        //Método para evitar que el usuario pueda introducir letras o caracteres especiales en el TextBox de la IP.
        private void TbColocarIP_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*
            if (!System.Text.RegularExpressions.Regex.IsMatch(TbColocarIP.Text, "^[0-9.]*$"))
            {
                MessageBox.Show("Por favor solo números y puntos.");
                TbColocarIP.Text = string.Empty;
            }
            */
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
                    rutarelativa = "Sfx\\sfxConectado.wav";
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