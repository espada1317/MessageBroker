using System;
using Common;
using System.Net;
using System.Net.Sockets;

namespace BrokerClass
{
    class BrokerSocket
    {
        private Socket _socket;
        private const int CONNECTIONS_LIMIT = 8;

        public BrokerSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start(string ip, int port)
        {
            _socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            _socket.Listen(CONNECTIONS_LIMIT);
            Accept();
        }

        private void Accept()
        {
            _socket.BeginAccept(AcceptedCallback, null);
        }

        private void AcceptedCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connection = new ConnectionInfo();

            try
            {
                connection.Socket = _socket.EndAccept(asyncResult);
                connection.Address = connection.Socket.RemoteEndPoint.ToString();
                connection.Socket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, 
                    SocketFlags.None, ReceiveCallback, connection);
            } 
            catch(Exception e)
            {
                Console.WriteLine("Can't accept# " + e.Message);
            }
            finally
            {
                Accept();
            }
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connection = asyncResult.AsyncState as ConnectionInfo;

            try
            {
                Socket senderSocket = connection.Socket;
                SocketError response;
                int bufferSize = senderSocket.EndReceive(asyncResult, out response);

                if (response == SocketError.Success)
                {
                    byte[] payload = new byte[bufferSize];
                    Array.Copy(connection.Buffer, payload, payload.Length);

                    PayloadHandler.Handle(payload, connection);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Can't receive data# " + e.Message);
            }
            finally
            {
                try
                {
                    connection.Socket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length,
                        SocketFlags.None, ReceiveCallback, connection);
                } 
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    var address = connection.Socket.RemoteEndPoint.ToString();

                    connection.Socket.Close();
                    ConnectionsStorage.Remove(address);
                }
            }

        }
    }
}
