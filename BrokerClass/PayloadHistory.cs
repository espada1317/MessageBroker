using Common;
using System.Collections.Generic;
using System.Threading;

namespace BrokerClass
{
    static class PayloadHistory
    {
        private static List<Payload> _payloadHistory = null;

        public static void AddPayloadHistory(Payload payload)
        {
            _payloadHistory.Add(payload);
            Thread.Sleep(1000);
            Serialization.SavePayloadList();
        }

        public static List<Payload> GetPayloadHistory()
        {
            return _payloadHistory;
        }

        public static void SetPayloadHistory(List<Payload> payloadHistory)
        {
            _payloadHistory = payloadHistory;
        }
    }
}
