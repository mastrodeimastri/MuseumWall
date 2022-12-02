using System;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;

namespace MuseumWall
{
    public partial class Common
    {
        protected int nScreens = 1;

        // Questa funzione crea un timer di 10 secondi che
        // verrà sfruttato per porre un tempo limite alle
        // connessioni degli slave
        protected void Timer()
        {
            Thread.Sleep(10000);
        }
    }
}

