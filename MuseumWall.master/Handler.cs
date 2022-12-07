using System;
using System.Net.Sockets;
using System.Text;

namespace MuseumWall
{
	public partial class Master
	{
        // Invia ad ogni slave il segnale di inizio
        private void SendInternal()
        {
            try
            {
                // inizializzo il messaggio
                byte[] msg = Encoding.UTF8.GetBytes("1");

                // aspetto di entrare nel semaforo se occupato
                sem.Wait();

                // se ho raspberry connessi all'endpoint,
                // invio il segnale di riproduzione
                if (nConnected != 0)
                    for (int i = (nRunning); i < nConnected; i++, nRunning++)
                        // invio il messaggio
                        _ = connections[i].Send(msg, 0, msg.Length, SocketFlags.None);

                // esco dal semaforo
                sem.Release();
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Si è verificato un errore durante l'invio del messaggio: {0}", ex.ErrorCode);
            }
        }
    }
}

