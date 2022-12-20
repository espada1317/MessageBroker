using Common;
using Newtonsoft.Json;
using System.Text;
using System.Threading;

namespace BrokerClass
{
    class MessageWorker
    {
        private const int TTS = 1000;

        public void DoSendMessageWork()
        {
            while (true)
            {
                while (!PayloadStorage.IsEmpty())
                {
                    var payload = PayloadStorage.GetNext();

                    if(payload != null)
                    {
                        var connections = ConnectionsStorage.GetConnectionsByTopic(payload.Topic);

                        foreach(var connection in connections)
                        {
                            var payloadString = JsonConvert.SerializeObject(payload);
                            byte[] data = Encoding.UTF8.GetBytes(payloadString);

                            connection.Socket.Send(data);
                        }
                    }
                }
                Thread.Sleep(500);
            }
        }

        public static void SendMessagesToConnection(ConnectionInfo connection, Payload payload)
        {
            var payloadString = JsonConvert.SerializeObject(payload);
            byte[] data = Encoding.UTF8.GetBytes(payloadString);

            connection.Socket.Send(data);
            Thread.Sleep(1000);
        }
    }
}
