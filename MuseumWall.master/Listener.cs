using System;
using System.Net.Sockets;

namespace MuseumWall
{
	public partial class Master
	{
        Thread listener;

        // Questa funzione mi permette di accettare
        // tutte le connessioni ricevute entro un dato lasso di tempo
        private void AcceptConn()
        {
            while (true)
            {
                Console.WriteLine("sono in attesa");
                Socket newConn = master.Accept();

                // aspetto di entrare nel semaforo se occupato
                sem.Wait();

                Console.WriteLine("ho ricevuto una connessione");

                connections[nConnected] = newConn;

                nConnected++;

                Console.WriteLine("ho rilasciato il semaforo");

                // esco dal semaforo
                sem.Release();
            }
        }
    }
}

