using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BrokerClass
{
    class Serialization
    {
        public static void LoadPayloadList()
        {
            string readText = File.ReadAllText(@"C:\Users\trifa\OneDrive\Desktop\serial.txt");
            var payload = (List<Payload>)JsonConvert.DeserializeObject<List<Payload>>(readText.Trim());
            PayloadHistory.SetPayloadHistory(payload);
        }

        public static void SavePayloadList()
        {
            var Json = JsonConvert.SerializeObject(PayloadHistory.GetPayloadHistory());
            File.WriteAllText(@"C:\Users\trifa\OneDrive\Desktop\serial.txt", Json.Trim());
        }
    }
}
