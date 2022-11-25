using System;
using System.Net.Sockets;
using System.Text;

namespace MuseumWall
{
	public class Handler
	{
        int conned;
        int runn;

        Socket[] conn;
        SemaphoreSlim semaph;

		public Handler(ref Socket[] connections,ref SemaphoreSlim semaphore, ref int connected, ref int running)
		{
			conn = connections;
			conned = connected;
			runn = running;
            semaph = semaphore;
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

                // aspetto di entrare nel semaforo se occupato
                semaph.Wait();

                // se ho raspberry connessi all'endpoint,
                // invio il segnale di riproduzione
                if (conned != 0)
                {
                    for (int i = (runn); i < conned; i++, runn++)
                    {
                        // invio il messaggio
                        _ = conn[i].Send(msg, 0, msg.Length, SocketFlags.None);
                    }
                }
                // esco dal semaforo
                semaph.Release();
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Si è verificato un errore durante l'invio del messaggio: {0}", ex.ErrorCode);
            }
        }
    }
}

