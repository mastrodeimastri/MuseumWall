using System;
namespace MuseumWall
{
	public partial class Master
	{
        public void Run()
        {
            while (true)
            {
                // invio il segnale di riproduzione ai rasp che sono in attesa
                if (nRunning != nConnected)
                {
                    Console.WriteLine("sono quiiii");
                    SendInternal();
                }
                // avvio la riproduzione sugli schermi
                for (int i = 0; i < nScreens; i++)
                {
                    //Console.WriteLine("dovrei riprodurre il video {0}", i);
                    PlayBack(i);
                }
            }
        }
    }
}

