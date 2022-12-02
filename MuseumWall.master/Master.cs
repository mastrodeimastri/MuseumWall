﻿using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace MuseumWall
{
    // Questa classe implementa l'oggetto master cui identifica
    // il raspeberry al qualce gli slave dovranno andare a far riferimento
    // per l'inizio della riproduzione
    public class Master : Common
    {
        // inizializzo gli indici per scorrere l'array
        int nConnected = 0;
        int nRunning = 0;

        // inizializzo le variabili che mi servono per far funzionare la logica
        Socket master;
        Socket[] connections = new Socket[100];
        Thread timer;
        Thread listener;
        Thread controller;
        IPEndPoint serverEndPoint;
        SemaphoreSlim sem = new(1);

        public Master()
        {
            try
            {
                // inizializzo tutte le variabili necessarie per creare il server
                CreateServer();

                // starto il server
                Start();

                // aspetto che il timer finisca per poter iniziare
                // l'invio del segnale e la riproduzione
                timer.Join();

                Console.WriteLine("timer finito");
                
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Si è verificato un errore o nella creazione" +
                    " del socket o nel binding della porta: {0}", ex.ErrorCode);
            }
        }

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
            timer = new Thread(Timer);

            //creo il thread che rimarrà in ascolto di nuove possibili connessioni
            listener = new(AcceptConn);

            //creo un thread che mi permette di comandare i rasp connessi
            //attraverso degli input da tastiera
            controller = new(Controller);
        }

        private void Controller()
        {
            while(true)
            {
                char k = Console.ReadKey().KeyChar;
                SendAction(k);
            }
        }

        private void Start()
        {
            // avvio i thread
            timer.Start();
            listener.Start();
            controller.Start();
        }

        // Questa funzione crea l'oggetto endpoint
        // sul quale il master rimarrà in ascolto delle connessioni
        private void CreateEndPoint()
        {
            IPAddress ip;
            ip = Dns.GetHostAddresses("192.168.1.112")[0];

            Console.WriteLine("questo è il mio indirizzo ip: {0}", ip.ToString());
            serverEndPoint = new(ip, 65011);

        }

        // Entry point per l'eseguibile che andrà a finire sul master
        static void Main(string[] args)
        {
            Master rasp = new Master();
            rasp.Run();
        }

        // Questa funzione mi permette di accettare
        // tutte le connessioni ricevute entro un dato lasso di tempo
        private void AcceptConn()
        {
            while(true)
            {
                Console.WriteLine("sono in attesa");
                Socket newConn = master.Accept();

                // aspetto di entrare nel semaforo se occupato
                sem.Wait();

                Console.WriteLine("ho ricevuto una connessione");

                connections[nConnected] = newConn;

                nConnected++;

                Console.WriteLine("ho rilasciato il semaforo");

                // esco dal semaforo
                sem.Release();
            }
        }

        private void SendAction(char k)
        {
            try
            {
                // inizializzo il messaggio
                byte[] msg = Encoding.UTF8.GetBytes(k.ToString());

                // aspetto di entrare nel semaforo se occupato
                sem.Wait();

                // se ho raspberry connessi all'endpoint,
                // invio il segnale di riproduzione
                if (nConnected != 0)
                {
                    for (int i = (0); i < nConnected; i++)
                    {
                        // invio il messaggio
                        _ = connections[i].Send(msg, 0, msg.Length, SocketFlags.None);
                    }
                }
                // esco dal semaforo
                sem.Release();
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Si è verificato un errore durante l'invio del messaggio: {0}", ex.ErrorCode);
            }
        }

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
                {
                    for (int i = (nRunning); i < nConnected; i++, nRunning++)
                    {
                        // invio il messaggio
                        _ = connections[i].Send(msg, 0, msg.Length, SocketFlags.None);
                    }
                }
                // esco dal semaforo
                sem.Release();
            }
            catch(SocketException ex)
            {
                Console.WriteLine("Si è verificato un errore durante l'invio del messaggio: {0}", ex.ErrorCode);
            }
        }

        public void Run()
        {
            while(true)
            {
                // invio il segnale di riproduzione ai rasp che sono in attesa
                if( nRunning != nConnected)
                {
                    Console.WriteLine("sono quiiii");
                    SendInternal();
                }
                // avvio la riproduzione sugli schermi
                for (int i = 0; i < nScreens; i++)
                {
                    //Console.WriteLine("dovrei riprodurre il video {0}", i);
                    PlayBack(i);
                }
            }
        }
    }
}

