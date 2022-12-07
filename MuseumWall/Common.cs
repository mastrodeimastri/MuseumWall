﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
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
            try
            {
                if (display == 0)
                    System.Diagnostics.Process.Start("/bin/omxplayer", "--display 0 /home/pi/video/1.mp4").WaitForExit();
                    //Console.WriteLine("1");
                else
                    System.Diagnostics.Process.Start("/bin/omxplayer", "--display 0 /home/pi/video/2.mp4").WaitForExit();
                    //Console.WriteLine("2");
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Console.WriteLine("non sono riuscito a riprodurre il video {0}", ex.Message);
            }    
        }

        public void PlayBack(int display)
        {
            PlaybackInternal(display);
        }
    }
}
