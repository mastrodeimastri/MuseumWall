using System;
using System.Net;
using System.Net.Sockets;
namespace MuseumWall
{
    public class Player
    {

        protected int nScreens = 2;
        protected IPEndPoint serverEndPoint;

        public Player(string masterAddr, int port)
        {
            try
            {
                // inizializzo l'endPoint
                serverEndPoint = new(Dns.GetHostEntry(masterAddr).AddressList[0], port);
            }
            catch(SocketException ex)
            {
                Console.WriteLine("Non sono riuscito a creare l'endpoint: {0}", ex.ErrorCode);
            }
        }

        // Questa funzione crea un timer di 10 secondi che
        // verrà sfruttato per porre un tempo limite alle
        // connessioni degli slave
        protected void Timer()
        {
            Thread.Sleep(10000);
        }


        private void PlaybackInternal(int display)
        {
            // avvio la riproduzione sul display selezionato
            if (display == 0)
                System.Diagnostics.Process.Start("omxplayer --no-osd --display 0 1.mp4 > /dev/null/ &");
            else
                System.Diagnostics.Process.Start("omxplayer --no-osd --display 7 2.mp4 > /dev/null/ &");
        }

        public void PlayBack(int display)
        {
            PlaybackInternal(display);
        }
    }
}

