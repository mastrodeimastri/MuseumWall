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
        Socket master;
        Socket[] connections;
        Thread timer;
        IPEndPoint serverEndPoint;

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

                // metto il server in ascolto per le connessioni degli slave
                master.Listen(100);

                // creo il thread che mi consente di avere un timer entro il
                // quale vengono accettate le connessioni
                timer = new Thread(Timer);

                timer.Start();
                AcceptConn();
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
            string host = Dns.GetHostName();
            IPAddress ip = Dns.GetHostByName(host).AddressList[0];
            serverEndPoint = new(ip, 65011);

        }

        // Entry point per l'eseguibile che andrà a finire sul master
        static void Main(string[] args)
        {
            Master rasp = new Master();
            rasp.Run();
        }

        // Questa funzione mi permette di accettare
        // tutte le connessioni ricevute entro un dato lasso di tempo
        private void AcceptConn()
        {
            while(timer.IsAlive)
            {
                Socket newConn = master.Accept();
                connections.Append(newConn);
            }
        }

        // Invia ad ogni slave il segnale di inizio
        private void SendInternal()
        {
            try
            {
                // inizializzo il messaggio
                byte[] msg = Encoding.UTF8.GetBytes("1");

                foreach(Socket conn in connections)
                {
                    // invio il messaggio
                    _ = conn.Send(msg, 0, msg.Length, SocketFlags.None);
                }
            }
            catch(SocketException ex)
            {
                Console.WriteLine("Si è verificato un errore durante l'invio del messaggio: {0}", ex.ErrorCode);
            }
        }

        public void Run()
        {
            // invio il segnale di inizio riproduzione all'endpoint
            SendInternal();

            while(true)
            {
                // avvio la riproduzione sugli schermi
                for (int i = 0; i < nScreens; i++)
                {
                    PlayBack(i);
                }
            }
        }
    }
}

