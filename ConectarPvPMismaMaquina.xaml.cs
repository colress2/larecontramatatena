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

namespace laRECONTRAmatatena
{
    public partial class ConectarPvPMismaMaquina : Window
    {
        #region Variables varias
        public string QUEPERSONAJEES;
        public string PERSONAJENOMBRE;
        public string ENEMIGONOMBRE;
        public string Clima;
        Socket ServidorSocket;
        Socket ClienteSocket;
        IPEndPoint ep;
        Socket acceptedsocket;
        #endregion



        public ConectarPvPMismaMaquina(string personaje, string nombre, string clima)
        {
            InitializeComponent();
            QUEPERSONAJEES = personaje;
            PERSONAJENOMBRE = nombre;
            Clima = clima;
            ep = new IPEndPoint(IPAddress.Any, 9050);
            ServidorSocket = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ClienteSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }
        

        //Botón para irte hacia la pantalla anterior
        private void Salir(object sender, RoutedEventArgs e)
        {
            ServidorSocket.Close();
            ElegirComoJugar mainWindow = new ElegirComoJugar(QUEPERSONAJEES, PERSONAJENOMBRE, Clima);
            this.Close();
            mainWindow.Show();
        }


        //Método para conectarse a un servidor
        public async void conectarse()
        {
            BtnCrearServidor.IsEnabled = false;
            BtnJugar.IsEnabled = false;
            ClienteSocket.Connect("127.0.0.1", 9050);

            //Recibe el mensaje del servidor y manda uno al servidor
            var buffer = new byte[1_024];
            int received = ClienteSocket.Receive(buffer);
            var message = Encoding.UTF8.GetString(buffer, 0, received);
            Tbchat.Text = message;
            ENEMIGONOMBRE = message;

            var EnviarMessage = /*"Hola! un placer yo soy " +*/PERSONAJENOMBRE;
            var messagebytes = Encoding.UTF8.GetBytes(EnviarMessage);
            ClienteSocket.Send(messagebytes);


            //Temporizador de 5 segundo antes de ser redirigido al apartado de jugar
            await Task.Delay(5000);
            JugadorvsjugadorMismaMaquina jugar = new JugadorvsjugadorMismaMaquina(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, ENEMIGONOMBRE);
            this.Close();
            jugar.Show();
        }


        //Botón para conectarse a un servidor
        public void Jugar(object sender, RoutedEventArgs e)
        {
            conectarse();
        }


        //Método para crear un servidor
        public async void crear()
        {
            ServidorSocket.Bind(ep);
            try
            {
                ServidorSocket.Listen(10);
                TbCrearServidor.Text = "Se ha creado un servidor! \n Tu dirección de IP es: " + IPAddress.Loopback + " dale esta IP a la persona con la cual desees jugar UwU";
                BtnCrearServidor.IsEnabled = false;
                BtnJugar.IsEnabled = false;
                acceptedsocket = await ServidorSocket.AcceptAsync();
                
                //Le manda al cliente un mensaje diciendo que la conexión se ha realizado con exito
                var message = /*"Conexión realizada con exito!\n" + */PERSONAJENOMBRE;
                var messagebytes = Encoding.UTF8.GetBytes(message);
                acceptedsocket.Send(messagebytes);

                var buffer = new byte[1_024];
                int received = acceptedsocket.Receive(buffer);
                var Recibirmessage = Encoding.UTF8.GetString(buffer,0,received);
                Tbchatserv.Text = Recibirmessage;
                ENEMIGONOMBRE = Recibirmessage;

                //Temporizador de 5 segundos antes de ser redirigido al apartado de jugar
                await Task.Delay(5000);
                JugadorvsjugadorMismaMaquina jugar = new JugadorvsjugadorMismaMaquina(QUEPERSONAJEES, PERSONAJENOMBRE, Clima, ENEMIGONOMBRE);
                this.Close();
                jugar.Show();
            }
            catch (SocketException)
            {
                ServidorSocket.Dispose();
            }
        }

        
        //Botón para crear el servidor
        private void CrearServidor(object sender, RoutedEventArgs e)
        {
            crear();
        }
    }
}
