using Common;
using Newtonsoft.Json;
using System;
using System.Text;

namespace SubscriberClass
{
    class PayloadHandler
    {
        public static void Handle(byte[] payloadBytes)
        {
            var payloadString = Encoding.UTF8.GetString(payloadBytes);
            var payload = JsonConvert.DeserializeObject<Payload>(payloadString);

            Console.WriteLine("----\n" + payload.Message + "\n----");
        }
    }
}
