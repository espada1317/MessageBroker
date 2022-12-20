using Common;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SubscriberClass
{
    class SubscriberSocket
    {
        private Socket _socket;
        private string _topic;

        public SubscriberSocket(string topic)
        {
            _topic = topic;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string ipAddress, int port)
        {
            _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAddress), port), ConnectedCallback, null);
            Console.WriteLine("Waiting for connection...");
        }

        private void ConnectedCallback(IAsyncResult asyncResult)
        {
            if(_socket.Connected)
            {
                Console.WriteLine("Subscriber conected to broker.");
                Subscribe();
                StartReceive();
            }
            else
            {
                Console.WriteLine("Error: Subscriber couldn't connect to broker");
            }
        }

        private void Subscribe()
        {
            var data = Encoding.UTF8.GetBytes("subscribe#" + _topic);
            Console.WriteLine("Loading precedent messages...");
            Send(data);
        }

        private void Send(byte[] data)
        {
            try
            {
                _socket.Send(data);
            }
            catch(Exception e)
            {
                Console.WriteLine("Couldn't send data: " + e.Message);
            }
        }

        private void StartReceive()
        {
            ConnectionInfo connection = new ConnectionInfo();
            connection.Socket = _socket;

            _socket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None,
                ReceiveCallback, connection);
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connectedInfo = asyncResult.AsyncState as ConnectionInfo;

            try
            {
                SocketError response;
                int buffSize = _socket.EndReceive(asyncResult, out response);

                if (response == SocketError.Success)
                {
                    byte[] payloadBytes = new byte[buffSize];
                    Array.Copy(connectedInfo.Buffer, payloadBytes, payloadBytes.Length);

                    PayloadHandler.Handle(payloadBytes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't receive data. " + e.Message);
            }
            finally
            {
                try
                {
                    connectedInfo.Socket.BeginReceive(connectedInfo.Buffer, 0, connectedInfo.Buffer.Length,
                        SocketFlags.None, ReceiveCallback, connectedInfo);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    connectedInfo.Socket.Close();
                }
            }
        }
    }
}
