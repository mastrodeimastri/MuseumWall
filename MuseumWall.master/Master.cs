using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace MuseumWall
{
    // Questa classe implementa l'oggetto master cui identifica
    // il raspeberry al qualce gli slave dovranno andare a far riferimento
    // per l'inizio della riproduzione
    public partial class Master : Common
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

        // Entry point per l'eseguibile che andrà a finire sul master
        static void Main(string[] args) { Master rasp = new Master(); rasp.Run(); }
    }
}

