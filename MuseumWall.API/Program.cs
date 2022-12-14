using System; // Namespace for Console output
using System.Configuration; // Namespace for ConfigurationManager
using System.Threading.Tasks; // Namespace for Task
using Azure.Storage.Queues; // Namespace for Queue storage types
using Azure.Storage.Queues.Models; // Namespace for PeekedMessage


namespace MuseumWall
{
    class Client
    {
        QueueClient client;

        Client(string connString, string queue)
        {
            client = new QueueClient(connString, queue);
            client.CreateIfNotExists();
        }

        public string GetMessage()
        {
            return client.PeekMessage().ToString();
        }

        public void Send(string status)
        {
            client.SendMessage(status);
        }
    }
}

