using System;
using System.Net;
namespace MuseumWall
{
    public class Player
    {

        protected int nScreens = 2;
        protected IPEndPoint serverEndPoint;

        public Player(string masterAddr, int port)
        {
            // inizializzo l'endPoint
            serverEndPoint = new(Dns.GetHostEntry(masterAddr).AddressList[0], port);
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

