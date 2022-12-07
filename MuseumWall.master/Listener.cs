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
                Socket newConn = master.Accept();

                // aspetto di entrare nel semaforo se occupato
                sem.Wait();

                connections[nConnected] = newConn;

                nConnected++;

                // esco dal semaforo
                sem.Release();
            }
        }
    }
}

