using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MuseumWall
{
	[System.Runtime.Versioning.SupportedOSPlatform("linux")]
	// Questa classe implementa l'oggetto slave
	// cui mi andrà ad identificare un raspberry
	// che parte in sincronia con il master ed è sottomesso a quel'ultimo
	public class Slave : Common
	{
		Socket client;
		Thread timer, connecter;
		string host;
		int port;

		public Slave(string a, int p)
		{
			try
			{
				// mi salvo i valori di host e port
				host = a;
				port = p;

				// inizializzo il socket client
				client = new(SocketType.Stream, ProtocolType.Tcp);

				CreateThreads();

				CreateSubProcess();

				timer.Join();

				connecter.Abort();

				// se non è connesso al server scrivo
				// a terminale un messaggio di errore
				Console.WriteLine(!client.Connected ? "Non solo riuscito a connettermi" : "Connesso");
			}
			catch (SocketException ex)
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
			Slave rasp = new Slave("192.168.1.112", 65011);
			rasp.Run();
		}

		private void CreateThreads() { timer = new(Timer); connecter = new(Connect); }

		private void Start() { timer.Start(); connecter.Start(); }

		private async void Connect() { await client.ConnectAsync(host, port); }

		public async void Run()
		{
			// inizializzo il buffer dove ricevere il messaggio
			byte[] buffer = new byte[256];

			Console.WriteLine("sono in attesa di ricevere il segnale");
			// sto in attesa di ricevere il messaggio
			client.Receive(buffer, SocketFlags.None);

			StartDisplays();
		}
	}
}

