using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MuseumWall
{
	// Questa classe implementa l'oggetto slave
	// cui mi andrà ad identificare un raspberry
	// che parte in sincronia con il master ed è sottomesso a quel'ultimo
	public class Slave : Player
	{
		Socket client;

		public Slave(string a, int p) : base(masterAddr: a, port: p)
		{
			try
			{
                // inizializzo il socket client
                client = new(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

				// connetto il socket al server
				Connect();
            }
			catch(SocketException ex)
			{
				// se il socket mi lancia una exception la catturo e la printo a schermo
				Console.WriteLine("Si è verificato un errore o durante la creazione del socket o durante la connessione con l'endpoint", ex.Message, ex.ErrorCode);
			}
        }

		// Entry point dell'eseguibile che andrà a finire sugli slave
		static void Main(string[] args)
		{
			Slave rasp = new Slave("192.168.1.228", 65011);
			rasp.Run();
		}

		private async void Connect()
		{
			await client.ConnectAsync(serverEndPoint);
		}

		public async void Run()
		{
			// inizializzo il buffer dove ricevere il messaggio
			byte[] buffer = new byte[256];

			// sto in attesa di ricevere il messaggio
			_ = await client.ReceiveAsync(buffer, SocketFlags.None);

			while(true)
			{
                // avvio la riproduzione dei video
                for (int i = 0; i < nScreens; i++)
                {
                    PlayBack(i);
                }
            }
		}
	}
}

