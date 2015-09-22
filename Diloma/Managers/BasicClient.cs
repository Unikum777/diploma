using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Threading;


namespace Diloma.Managers
{
    public enum ROUTING_TYPE{
        onion,
        garlic
    }
    public class BasicClient
    {
        public int UDPport { get; set; }
        public int port { get; set; }

        public String UDPmask { get; set; }
        public int RefreshTime {get; set;}
        public int WaitTime { get; set; }

        public int MaxNodes { get; set; }

        public int MaxConnections { get; set; }
        public int LayerCount { get; set; }

        public bool SendFakePackages { get; set; }

        public DateTime KeyRefreshTime {get; set;}

        public IPAddress CurrentIP { get; set; }

        List<Socket> SockList = new List<Socket>();
        
        public BasicClient(int UDP_port, int TCP_port, IPAddress Current)
        {
            UDPport = UDP_port;
            port = TCP_port;
            CurrentIP = Current;
            UDPmask = "192.168.88.255";
            RefreshTime = 300;
            WaitTime = 1;
            MaxNodes = 10;
            LayerCount = 3;
            MaxConnections = 10;
            SendFakePackages = true;
        }


        public void SendNext(IPAddress IP, byte[] Data, Logging log)
        {
            
            try
            {
                log.printlog("[senddata] Start transit from serversending. Calling IP: " + IP.ToString() +
                    " port: " + port.ToString());
                //nextsock.

                foreach (Socket s in SockList)
                {
                    if (s.RemoteEndPoint.Equals(new IPEndPoint(IP, port))) 
                    {
                        s.Send(Data, Data.Length, SocketFlags.None);
                        log.printlog("[senddata] Transit sending done.");
                        return;
                    }
                }

                Socket nextsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                nextsock.Connect(IP, port);
                nextsock.Send(Data, Data.Length, SocketFlags.None); ;
                SockList.Add(nextsock);
                // nextsock.Close();

                
               
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при отправке пакета", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.printlog("[ERROR][SendNext] " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при отправке пакета", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.printlog("[ERROR][SendNext] " + ex.Message);
            }

        }

    }
}
