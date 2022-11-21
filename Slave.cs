using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MuseumWall
{
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
                client.Connect(serverEndPoint);
            }
			catch(SocketException ex)
			{
				// se il socket mi lancia una exception la catturo e la printo a schermo
				Console.WriteLine("the socket throws: {0}  {1}", ex.Message, ex.ErrorCode);
			}
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

