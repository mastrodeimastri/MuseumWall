using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MuseumWall
{
    public class Common
    {
        protected int nScreens = 1;

        public Common()
        {
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
                //Console.WriteLine("1");
            else
                System.Diagnostics.Process.Start("omxplayer --no-osd --display 7 2.mp4 > /dev/null/ &");
                //Console.WriteLine("2");
        }

        public void PlayBack(int display)
        {
            PlaybackInternal(display);
        }
    }
}

