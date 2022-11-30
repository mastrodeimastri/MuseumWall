using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MuseumWall
{
	// Questa classe implementa l'oggetto slave
	// cui mi andrà ad identificare un raspberry
	// che parte in sincronia con il master ed è sottomesso a quel'ultimo
	public class Slave : Common
	{
		Socket client;
		Thread timer;
		string host;
		int port;

		public Slave(string a, int p)
		{
			try
			{
				host = a;
				port = p;
                // inizializzo il socket client
                client = new Socket(SocketType.Stream, ProtocolType.Tcp);

				// inizializzo il timer
				timer = new Thread(Timer);

				// starto il server
				timer.Start();

                // connetto il socket al server
                Connect();

				// controllo se il socket è connesso
				if(!client.Connected)
				{
					Console.WriteLine("il socket non è connesso all'endpoint");
					while(true)
					{

					}
				}
            }
			catch(SocketException ex)
			{
				// se il socket mi lancia una exception
				// la catturo e la printo a schermo
				Console.WriteLine("Si è verificato un errore o durante la " +
					"creazione del socket o durante la connessione con " +
					"l'endpoint", ex.Message, ex.ErrorCode);
			}
        }

		// Entry point dell'eseguibile che andrà a finire sugli slave
		static void Main(string[] args)
		{
			Slave rasp = new Slave("192.168.1.101", 65011);
			rasp.Run();
		}

		private void Connect()
		{
            while (!client.Connected && timer.IsAlive)
			{
                try
                {
					client.Connect(host, port);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("Non sono riuscito a connettermi al server : {0}", ex.ErrorCode);
                }
            }
		}

		public async void Run()
		{
			// inizializzo il buffer dove ricevere il messaggio
			byte[] buffer = new byte[256];

			Console.WriteLine("sono in attesa di ricevere il segnale");
			// sto in attesa di ricevere il messaggio
			client.Receive(buffer, SocketFlags.None);

			while(true)
			{
				Console.WriteLine("2");
                // avvio la riproduzione dei video
                //for (int i = 0; i < nScreens; i++)
                //{
                //    PlayBack(i);
                //}
            }
		}
	}
}

