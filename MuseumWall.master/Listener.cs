using System;
using System.Diagnostics.Metrics;
using System.Net.Sockets;

namespace MuseumWall
{
	public class Listener
	{
        Vars v;

		public Listener(ref Vars variables)
		{
            v = (Vars) variables.ShallowCopy();
		}

		public void startListening()
		{
            // metto il server in ascolto per le connessioni degli slave
            v.master.Listen(100);

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
                Socket newConn = v.master.Accept();

                // aspetto di entrare nel semaforo se occupato
                v.sem.Wait();

                Console.WriteLine("ho ricevuto una connessione");

                v.connections[v.nConnected] = newConn;

                v.nConnected++;

                Console.WriteLine("ho rilasciato il semaforo");

                // esco dal semaforo
                v.sem.Release();
            }
        }
    }
}

