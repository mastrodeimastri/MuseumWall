using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace MuseumWall
{
    // Questa classe implementa l'oggetto master cui identifica
    // il raspeberry al qualce gli slave dovranno andare a far riferimento
    // per l'inizio della riproduzione
    public class Master : Common
    {
        // inizializzo gli indici per scorrere l'array
        int nConnected = 0;
        int nRunning = 0;

        Socket master;
        Socket[] connections = new Socket[100];
        IPEndPoint serverEndPoint;
        SemaphoreSlim sem = new(1);

        public Master()
        {
            try
            {

                // Creo l'endpoint
                CreateEndPoint();

                // inizializzo il socket master che mi fa da server
                master = new(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // bindo il socket master all'endpoint prestabilito
                master.Bind(serverEndPoint);

                // creo l'oggetto listener che mi permette
                // di lasciare un thread in attesa di nuove connessioni
                Listener listener = new(ref master, ref connections, ref sem, ref nConnected);

                // avvio il timer
                StartTimer();

                // avvio l'ascolto
                listener.startListening();

                // aspetto che il timer finisca per poter iniziare
                // l'invio del segnale e la riproduzione
                timer.Join();
                Console.WriteLine("timer finito");
                
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Si è verificato un errore o nella creazione" +
                    " del socket o nel binding della porta: {0}", ex.ErrorCode);
            }
        }

        // Questa funzione crea l'oggetto endpoint
        // sul quale il master rimarrà in ascolto delle connessioni
        private void CreateEndPoint()
        {
            IPAddress ip;
            string host = Dns.GetHostName();

            ip = Dns.GetHostAddresses("192.168.1.101")[0];
            

            Console.WriteLine("questo è il mio indirizzo ip: {0}", ip.ToString());
            serverEndPoint = new(ip, 65011);

        }

        // Entry point per l'eseguibile che andrà a finire sul master
        static void Main(string[] args)
        {
            Master rasp = new Master();
            rasp.Run();
        }

        public void Run()
        {

            Handler handler = new(ref connections, ref sem, ref nConnected, ref nRunning);

            handler.Send();

            while(true)
            {
                //Console.WriteLine("1");

                if( nRunning != nConnected)
                {
                    handler.Send();
                }

                // avvio la riproduzione sugli schermi
                //for (int i = 0; i < nScreens; i++)
                //{
                //    PlayBack(i);
                //}
            }
        }
    }
}

