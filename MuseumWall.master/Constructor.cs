using System;
using System.Net.Sockets;

namespace MuseumWall
{
	public partial class Master
	{
        public Master()
        {
            try
            {
                // inizializzo tutte le variabili necessarie per creare il server
                CreateServer();

                CreateSubProcess();

                // starto il server
                Start();

                // aspetto che il timer finisca per poter iniziare
                // l'invio del segnale e la riproduzione
                timer.Join();

                Console.WriteLine("timer finito");
                
                if (listener.IsAlive)
                    Console.WriteLine("il listener è vivo");

            }
            catch (SocketException ex)
            {
                Console.WriteLine("Si è verificato un errore o nella creazione" +
                    " del socket o nel binding della porta: {0}", ex.ErrorCode);
            }
        }
    }
}

