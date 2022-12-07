using System;
using System.Net;
using System.Net.Sockets;

namespace MuseumWall
{ 
	public partial class Master
	{
        private void CreateServer()
        {
            // Creo l'endpoint
            CreateEndPoint();

            // inizializzo il socket master che mi fa da server
            master = new(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // bindo il socket master all'endpoint prestabilito
            master.Bind(serverEndPoint);

            // metto il server in ascolto per le connessioni degli slave
            master.Listen(100);

            // creo il thread che mi consente di avere un timer entro il
            // quale vengono accettate le connessioni
            timer = new(Timer);

            //creo il thread che rimarrà in ascolto di nuove possibili connessioni
            listener = new(AcceptConn);
        }

        // Questa funzione crea l'oggetto endpoint
        // sul quale il master rimarrà in ascolto delle connessioni
        private void CreateEndPoint()
        {
            IPAddress ip = Dns.GetHostAddresses("192.168.1.112")[0];

            Console.WriteLine("questo è il mio indirizzo ip: {0}", ip.ToString());
            serverEndPoint = new(ip, 65011);

        }

        private void Start()
        {
            // avvio i thread
            timer.Start();
            listener.Start();
        }
    }
}