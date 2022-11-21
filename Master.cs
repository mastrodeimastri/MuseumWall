using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace MuseumWall
{
    public class Master : Player
    {
        Socket master;

        public Master(string a, int p) : base(masterAddr: a, port: p)
        {
            try
            {
                // inizializzo il socket master che mi fa da server
                master = new(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // bindo il socket master all'endpoint prestabilito
                master.Bind(serverEndPoint);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Si è verificato un errore o nella creazione del socket o nel binding della porta: {0}", ex.ErrorCode);
            }
        }

        private void SendInternal()
        {
            // inizializzo il messaggio
            byte[] msg = Encoding.UTF8.GetBytes("1");

            // invio il messaggio
            _ = master.Send(msg, 0, msg.Length, SocketFlags.None);
        }

        public void Run()
        {
            // aspetto che i client si connettano all'endpoint
            Thread.Sleep(10000);

            // invio il segnale di inizio riproduzione all'endpoint
            SendInternal();

            // aspetto che i clients ricevano l'informazione
            Thread.Sleep(100);


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

