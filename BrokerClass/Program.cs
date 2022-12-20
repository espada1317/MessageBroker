﻿using System;
using System.Threading.Tasks;
using Common;

namespace BrokerClass
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Broker#");

            BrokerSocket socket = new BrokerSocket();
            socket.Start(Settings.BROKER_IP, Settings.BROKER_PORT);

            var worker = new MessageWorker();
            Task.Factory.StartNew(worker.DoSendMessageWork, TaskCreationOptions.LongRunning);

            Console.ReadLine();
        }
    }
}
