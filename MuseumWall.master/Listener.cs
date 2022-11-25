using System;
using System.Net.Sockets;

namespace MuseumWall
{
	public class Listener
	{
		Socket soc;
        Socket[] conn;
        SemaphoreSlim semaph;
        int i;

		public Listener(ref Socket masterSoc, ref Socket[] connections, ref SemaphoreSlim semaphore, ref int index)
		{
			soc = masterSoc;
            conn = connections;
            semaph = semaphore;
            i = index;
		}

		public void startListening()
		{
			Thread listener = new(AcceptConn);
            listener.Start();
		}


        // Questa funzione mi permette di accettare
        // tutte le connessioni ricevute entro un dato lasso di tempo
        private void AcceptConn()
        {
            while (true)
            {
                Console.WriteLine("sono in attesa");
                Socket newConn = soc.Accept();

                // aspetto di entrare nel semaforo se occupato
                semaph.Wait();

                Console.WriteLine("ho ricevuto una connessione");

                conn[i] = newConn;

                i++;

                Console.WriteLine("ho rilasciato il semaforo");

                // esco dal semaforo
                semaph.Release();
            }
        }
    }
}

