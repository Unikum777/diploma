using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
namespace Diloma
{
    class Udp_connection
    {
        private int UDP_port;
        
        //private int MAX_stations;
        public Udp_connection(int Port)
        {
            UDP_port = Port;
        }
        public List<string> get_available_IPs()
        {
            List<string> IPs_array = new List<string>();
            // Создаем UdpClient для чтения входящих данных
            UdpClient receivingUdpClient = new UdpClient(UDP_port);
            
            IPEndPoint RemoteIpEndPoint = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {

                while (stopWatch.Elapsed.Seconds < 10)
                {
                    // Ожидание дейтаграммы
                    byte[] receiveBytes = receivingUdpClient.Receive(
                       ref RemoteIpEndPoint);

                    // Преобразуем и отображаем данные
                    string returnData = Encoding.UTF8.GetString(receiveBytes);
                    if (!IPs_array.Contains(returnData))
                    {
                        IPs_array.Add(returnData);
                    }
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show("Ошибка: " + ex.Message, ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error );
               return null;
            }
            return IPs_array;
            
        }
    }
}
