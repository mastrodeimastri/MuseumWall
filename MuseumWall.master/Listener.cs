using System;
using System.Diagnostics.Metrics;
using System.Net.Sockets;

namespace MuseumWall
{
	public class Listener
	{
        int i;

        Socket soc;
        Socket[] conn;
        SemaphoreSlim semaph;

		public Listener(ref Socket masterSoc, ref Socket[] connections, ref SemaphoreSlim semaphore, ref int index)
		{
			soc = masterSoc;
            conn = connections;
            semaph = semaphore;
            i = index;
		}

		public void startListening()
		{
            // metto il server in ascolto per le connessioni degli slave
            soc.Listen(100);

            // creo il thread che starà in ascolto
            Thread listener = new(AcceptConn);

            // avvio il thread
            listener.Start();
		}


        // Questa funzione mi permette di accettare
        // tutte le connessioni ricevute
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

