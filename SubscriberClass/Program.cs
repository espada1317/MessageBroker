using Common;
using System;

namespace SubscriberClass
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Subsriber#");

            string topic;

            Console.Write("Enter the topic: ");
            topic = Console.ReadLine().ToLower();

            var subsriberSocket = new SubscriberSocket(topic);
            subsriberSocket.Connect(Settings.BROKER_IP, Settings.BROKER_PORT);

            Console.ReadLine();
        }
    }
}
