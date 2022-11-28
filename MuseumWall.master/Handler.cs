using System;
using System.Net.Sockets;
using System.Text;

namespace MuseumWall
{

    public class Handler
	{
        Vars v;

		public Handler(ref Vars variables)
		{
           v = (Vars) variables.ShallowCopy();
		}

		public void Send()
		{
            SendInternal();
		}

        // Invia ad ogni slave il segnale di inizio
        private void SendInternal()
        {
            try
            {
                // inizializzo il messaggio
                byte[] msg = Encoding.UTF8.GetBytes("1");

                Console.WriteLine("Handler: aspetto il semaforo");
                // aspetto di entrare nel semaforo se occupato
                v.sem.Wait();

                // se ho raspberry connessi all'endpoint,
                // invio il segnale di riproduzione
                if (v.nConnected != 0)
                {
                    for (int i = (v.nRunning); i < v.nConnected; i++, v.nRunning++)
                    {
                        // invio il messaggio
                        _ = v.connections[i].Send(msg, 0, msg.Length, SocketFlags.None);
                        Console.WriteLine("ho inviato il messaggio");
                    }
                }
                // esco dal semaforo
                v.sem.Release();
                Console.WriteLine("Handler: ho rilasciato il semaforo");
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Si è verificato un errore durante l'invio del messaggio: {0}", ex.ErrorCode);
            }
        }
    }
}

