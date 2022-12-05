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
                    SendInternal();
                }
                // avvio la riproduzione sugli schermi
                StartDisplays();
            }
        }
    }
}

