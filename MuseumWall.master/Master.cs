using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace MuseumWall
{
    public struct Vars
    {
        public int nConnected;
        public int nRunning;

        public Socket master;
        public Socket[] connections;
        public IPEndPoint serverEndPoint;
        public SemaphoreSlim sem;

        public Vars()
        {
            nConnected = 0;
            nRunning = 0;

            connections = new Socket[100];
            sem = new(1);
        }

        public object ShallowCopy()
        {
            return this.MemberwiseClone();
        }
    }

    // Questa classe implementa l'oggetto master cui identifica
    // il raspeberry al qualce gli slave dovranno andare a far riferimento
    // per l'inizio della riproduzione
    public class Master : Common
    {
        Vars v;

        public Master()
        {
            try
            {
                v = new Vars();

                // Creo l'endpoint a cui collegarsi
                CreateEndPoint();

                // inizializzo il socket master che mi fa da server
                v.master = new(v.serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // bindo il socket master all'endpoint prestabilito
                v.master.Bind(v.serverEndPoint);

                // creo l'oggetto listener che mi permette
                // di lasciare un thread in attesa di nuove connessioni
                Listener listener = new(ref v);

                // avvio il timer
                StartTimer();

                // avvio l'ascolto
                listener.startListening();

                // aspetto che il timer finisca per poter iniziare
                // l'invio del segnale e la riproduzione
                timer.Join();
                Console.WriteLine("timer finito");
                Console.WriteLine("connessioni ricevute: {0}", v.nConnected);

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
            v.serverEndPoint = new(ip, 65011);

        }

        // Entry point per l'eseguibile che andrà a finire sul master
        static void Main(string[] args)
        {
            Master rasp = new Master();
            rasp.Run();
        }

        public void Run()
        {

            Handler handler = new(ref v);

            handler.Send();

            while(true)
            {
                //Console.WriteLine("1");

                if( v.nRunning != v.nConnected)
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

