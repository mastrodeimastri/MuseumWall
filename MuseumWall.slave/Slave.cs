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
<<<<<<< HEAD
<<<<<<< Updated upstream
		Thread timer;
		Thread reciver;
=======
		Thread timer, connecter;
>>>>>>> Stashed changes
=======
		Thread timer, connecter;
>>>>>>> main
		string host;
		int port;

		public Slave(string a, int p)
		{
			try
			{
				// mi salvo i valori di host e port
				host = a;
				port = p;

<<<<<<< HEAD
<<<<<<< Updated upstream
                // inizializzo il socket client
                client = new Socket(SocketType.Stream, ProtocolType.Tcp);
=======
				// inizializzo il socket client
				client = new(SocketType.Stream, ProtocolType.Tcp);
>>>>>>> main

				CreateThreads();

<<<<<<< HEAD
				reciver = new(ReciveAction);

				// avvio il timer che mi stabilisce un tempo limite entro il quale mi devo collegare al server
				timer.Start();
=======
				CreateSubProcess();
>>>>>>> main

				timer.Join();

				connecter.Abort();

				// se non è connesso al server scrivo
<<<<<<< HEAD
				// a terminale ubn messaggio di errore
				if (!client.Connected)
					Console.WriteLine("non sono riuscito a connettermi al server");
            }
			catch(SocketException ex)
=======
				// inizializzo il socket client
				client = new(SocketType.Stream, ProtocolType.Tcp);

				CreateThreads();

				CreateSubProcess();

				timer.Join();

				connecter.Abort();

				// se non è connesso al server scrivo
=======
>>>>>>> main
				// a terminale un messaggio di errore
				Console.WriteLine(!client.Connected ? "Non solo riuscito a connettermi" : "Connesso");
			}
			catch (SocketException ex)
<<<<<<< HEAD
>>>>>>> Stashed changes
=======
>>>>>>> main
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

<<<<<<< HEAD
<<<<<<< Updated upstream
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

                }
            }
		}
=======
		private void CreateThreads() { timer = new(Timer); connecter = new(Connect); }

		private void Start() { timer.Start(); connecter.Start(); }

		private async void Connect() { await client.ConnectAsync(host, port); }
>>>>>>> Stashed changes

		private void ReciveAction()
		{
			byte[] buffer = new byte[100];

			// sto in attesa di ricevere il messaggio
			client.ReceiveAsync(buffer, SocketFlags.None);

			char k = buffer[0].ToString()[0];

			switch(k)
			{
				case 'e':
                    // System.Diagnostics.Process.Start("/bin/omxplayer", "--display 0 /home/pi/video/2.mp4").WaitForExit();
                    break;
				case 'r':
                    System.Diagnostics.Process.Start("sudo reboot", "-h now").WaitForExit();
                    break;
			}
        }
=======
		private void CreateThreads() { timer = new(Timer); connecter = new(Connect); }

		private void Start() { timer.Start(); connecter.Start(); }

		private async void Connect() { await client.ConnectAsync(host, port); }
>>>>>>> main

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

