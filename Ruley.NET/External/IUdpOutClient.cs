using System;
using System.Net.Sockets;
using System.Text;

namespace Ruley.NET.External
{
    public interface IUdpOutClient
    {
        void Send(string host, int port, string value);
    }

    public class UdpOutClient : IUdpOutClient, IDisposable
    {
        private UdpClient _udpClient = new UdpClient();

        public void Dispose()
        {
            _udpClient.Dispose();
        }

        public void Send(string host, int port, string value)
        {
            Byte[] sendBytes = Encoding.ASCII.GetBytes(value);
            _udpClient.Send(sendBytes, sendBytes.Length, host, port);
        }
    }

    public class TestUdpOutClient : IUdpOutClient
    {
        public void Send(string host, int port, string value)
        {
            Console.WriteLine("udp: " + value);
        }
    }
}
