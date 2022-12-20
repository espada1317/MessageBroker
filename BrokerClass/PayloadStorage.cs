using Common;
using System.Collections.Concurrent;

namespace BrokerClass
{
    static class PayloadStorage
    {
        private static ConcurrentQueue<Payload> _payloadsQueue;

        static PayloadStorage()
        {
            _payloadsQueue = new ConcurrentQueue<Payload>();
        }

        public static void Add(Payload payload)
        {
            _payloadsQueue.Enqueue(payload);
        }

        public static Payload GetNext()
        {
            Payload payload = null;

            _payloadsQueue.TryDequeue(out payload);

            return payload;
        }

        public static bool IsEmpty()
        {
            return _payloadsQueue.IsEmpty;
        }
    }
}
