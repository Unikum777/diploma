using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Diloma.DataType;
using Diloma.Managers;

namespace Diloma.Sockets
{
    public class UDPServer
    {
        string mask = "192.168.88.255";
        String StationName;
        String PublicKey;
        BasicClient basic;
        public UDPServer(BasicClient BasicClient, String StationName, String PublicKey)
        {
            basic = BasicClient;
            this.StationName = StationName;
            this.PublicKey = PublicKey;
        }
        public void Run()
        {
            Thread _acceptThread;
            _acceptThread = new Thread(ThreadRun);
            _acceptThread.IsBackground = true;
            _acceptThread.Start();
        }
        private void ThreadRun()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPAddress broadcast = IPAddress.Parse(mask);

            //Get IP address
            byte[] sendbuf = Encoding.ASCII.GetBytes(StationName + "|" + PublicKey);
            IPEndPoint ep = new IPEndPoint(broadcast, basic.UDPport);

            while (true)
            {
                s.SendTo(sendbuf, ep);
                Thread.Sleep(500);
            }
            
        }
    }
}
