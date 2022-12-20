using System.Net.Sockets;

namespace Common
{
    public class ConnectionInfo
    {
        public const int BUFF_SIZE = 1024;
        public byte[] Buffer { get; set; }

        public Socket Socket { get; set; }

        public string Address { get; set; }

        public string Topic { get; set; }

        public ConnectionInfo()
        {
            Buffer = new byte[BUFF_SIZE];
        }
    }
}
