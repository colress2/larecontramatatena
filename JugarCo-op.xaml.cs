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
    public partial class JugarCo_op : Window
    {
        public string QUEPERSONAJEES;
        public string PERSONAJENOMBRE;
        Socket ServidorSocket;
        Socket ClienteSocket;
        IPEndPoint ep;
        Socket acceptedsocket;       
        public JugarCo_op(string personaje, string nombre)
        {
            InitializeComponent();
            QUEPERSONAJEES = personaje;
            PERSONAJENOMBRE = nombre;
            ep = new IPEndPoint(IPAddress.Any, 9050);
            ServidorSocket = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ClienteSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }
        
        private void Salir(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(QUEPERSONAJEES, PERSONAJENOMBRE);
            this.Close();
            mainWindow.Show();
        }



        public void conectarse()
        {
            if (!TbColocarIP.Text.Equals(string.Empty))
            {


                ClienteSocket.Connect(TbColocarIP.Text, 9050);

            
                var buffer = new byte[1_024];
                int received = ClienteSocket.Receive(buffer);
                var message = Encoding.UTF8.GetString(buffer, 0, received);
                Tbchat.Text = message;

                var EnviarMessage = "Hola! un placer yo soy " + PERSONAJENOMBRE;
                var messagebytes = Encoding.UTF8.GetBytes(EnviarMessage);
                ClienteSocket.Send(messagebytes);
           }
        }

        public void Jugar(object sender, RoutedEventArgs e)
        {
            conectarse();
        }

        public async void crear()
        {
            ServidorSocket.Bind(ep);
            try
            {
                ServidorSocket.Listen(10);
                TbCrearServidor.Text = "Se ha creado un servidor! \n Tu dirección de IP es: 172.18.35.7 dale esta IP a la persona con la cual desees jugar UwU";
                acceptedsocket = await ServidorSocket.AcceptAsync();
                
                var message = "Conexión realizada con exito!\nHola! Yo soy " + PERSONAJENOMBRE;
                var messagebytes = Encoding.UTF8.GetBytes(message);
                acceptedsocket.Send(messagebytes);
                //TbBuscarServidor.Text = "Se ha enviado el mensaje";

                var buffer = new byte[1_024];
                int received = acceptedsocket.Receive(buffer);
                var Recibirmessage = Encoding.UTF8.GetString(buffer,0,received);
                Tbchat.Text = Recibirmessage;
            }
            catch (SocketException)
            {
                ServidorSocket.Dispose();
            }
        }

        private void BtnCrearServidor(object sender, RoutedEventArgs e)
        {
            crear();
        }
    }
}
