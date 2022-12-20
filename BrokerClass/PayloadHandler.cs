using Common;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrokerClass
{
    class PayloadHandler
    {
        static object locker = new object();

        public static void Handle(byte[] payloadBytes, ConnectionInfo connectionInfo)
        {
            var payloadString = Encoding.UTF8.GetString(payloadBytes);

            lock (locker)
            {
                if (payloadString.StartsWith("subscribe#"))
                {
                    connectionInfo.Topic = payloadString.Split("subscribe#").LastOrDefault();
                    ConnectionsStorage.Add(connectionInfo);

                    Serialization.LoadPayloadList();
                    if (PayloadHistory.GetPayloadHistory().Count != 0)
                    {
                        foreach (var x in PayloadHistory.GetPayloadHistory())
                        {
                            if(x.Topic.Equals(connectionInfo.Topic))
                            {
                                MessageWorker.SendMessagesToConnection(connectionInfo, x);
                            }
                            Task.Delay(2000);
                        }
                    }
                }
                else
                {
                    Payload payload = JsonConvert.DeserializeObject<Payload>(payloadString);
                    PayloadStorage.Add(payload);
                    PayloadHistory.AddPayloadHistory(payload);
                }
            }
        }
    }
}
