using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Diloma.DataType;
using Diloma.Managers;


namespace Diloma.Sockets
{
    public class SelectServer
    {
        private Socket _serverSocket;
        private int _port;  
        int MaxConnections = 10;
        Logging log;
        BasicClient basic;
        RSACryptoServiceProvider RSA;
        public string getpublickey()
        {
            return RSA.ToXmlString(false);
        }
        public SelectServer(BasicClient Basic_client)
        {
            basic = Basic_client;
            log = new Logging("Serverlog.acl");
            RefreshRSAKey();
        }
        public void RefreshRSAKey()
        {

            RSA = new RSACryptoServiceProvider();
            log.printlog("[RSA] generate new private/open key. Open key: " + RSA.ToXmlString(false));
        }
        private void SetupServerSocket()
        {
            // Получаем информацию о локальном компьютере
            IPEndPoint myEndpoint = new IPEndPoint(basic.CurrentIP, basic.port);

            // Создаем сокет, привязываем его к адресу
            // и начинаем прослушивание
            _serverSocket = new Socket( myEndpoint.Address.AddressFamily,  SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(myEndpoint);
            _serverSocket.Listen((int)SocketOptionName.MaxConnections);
        }
        public void Run()
        {
            Thread selectThread = new Thread(ProcessSockets);
            selectThread.IsBackground = true;
            selectThread.Start();
        }
        private void ProcessSockets()
        {
            byte[] buffer = new byte[100000];
            List<Socket> readSockets = new List<Socket>();
            List<Socket> connectedSockets = new List<Socket>();
            try
            {
                SetupServerSocket();
                while (true)
                {
                    // Заполняем список сокетов чтения
                    readSockets.Clear();
                    readSockets.Add(_serverSocket);
                    readSockets.AddRange(connectedSockets);

                    // Определяем статус сокетов
                    Socket.Select(readSockets, null, null,
                        int.MaxValue);
                    // Обрабатываем каждый сокет, требующий
                    // каких-либо действий
                    foreach (Socket readSocket in readSockets)
                    {
                        if (readSocket == _serverSocket)
                        {
                            // Создаем новый сокет и сохраняем его
                            Socket newSocket = readSocket.Accept();
                            connectedSockets.Add(newSocket);
                        }
                        else
                        {
                            // Читаем и обрабатываем данные
                            int bytesRead =
                                readSocket.Receive(buffer);
                            if (0 == bytesRead)
                            {
                                connectedSockets.Remove(
                                    readSocket);
                                readSocket.Close();
                            }
                            else
                            {
                                CheckData(buffer); 
                            }
                        }
                    }
                }
            }
            catch (SocketException exc)
            {
                Console.WriteLine("Socket exception: " +
                    exc.SocketErrorCode);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception: " + exc);
            }
            finally
            {
                foreach (Socket s in connectedSockets) s.Close();
                connectedSockets.Clear();
            }
        }

        private byte[] Serialization(Object obj)
        {
            //Stream TestFileStream = File.Create(FileName);
            BinaryFormatter serializer = new BinaryFormatter();
            Stream BytesStream = new MemoryStream();
            serializer.Serialize(BytesStream, obj);


            byte[] buffer = new byte[BytesStream.Length];
            BytesStream.Seek(0, SeekOrigin.Begin);
            BytesStream.Read(buffer, 0, (int)BytesStream.Length);

            BytesStream.Close();
            return buffer;
        }
        public event NewMessageDelegate NewMessageEvent;
        private DataType.PackageLabel CheckData(byte[] Serialized)
        {
            Serialized = RSADecrypt(Serialized);
            Stream BytesStream = new MemoryStream();
            BinaryFormatter deserializer = new BinaryFormatter();
            BytesStream.Write(Serialized, 0, Serialized.Length);
            BytesStream.Seek(0, SeekOrigin.Begin);
            NetworkMessage wst = (NetworkMessage)deserializer.Deserialize(BytesStream);
            BytesStream.Close();
            log.printlog("[getdata] New serialized data package: Data size = " + wst.Data.Length.ToString() +
                " Next (or sender) IP: " + wst.NextIP.ToString() + " Timestamp: " +
                wst.TimeStamp.ToString("o"));
            if (wst.label == DataType.PackageLabel.final)
            {
                log.printlog("[getdata] Package detected as FINALLY");
                //MessageBox.Show(Encoding.UTF8.GetString(wst.Data, 0, wst.Data.Length));
                // wst.Data = RSA.Decrypt(wst.Data, false);
                if (NewMessageEvent != null)
                {
                    NewMessageEvent(wst, wst.NextIP);
                }
                return DataType.PackageLabel.final;
            }
            else if (wst.label == DataType.PackageLabel.transit)
            {
                log.printlog("[getdata] Package detected as TRANSIT");
                SendNext(wst.NextIP, wst.Data);
                return DataType.PackageLabel.transit;
            }
            else if (wst.label == DataType.PackageLabel.getkey)
            {
                log.printlog("[getdata] Package detected as GETKEY");
                NetworkMessage msg = new DataType.NetworkMessage();
                msg.NextIP = basic.CurrentIP;
                msg.label = DataType.PackageLabel.openkey;
                msg.TimeStamp = DateTime.Now;
                msg.Data = Encoding.UTF8.GetBytes(RSA.ToXmlString(false));
                //connection.Socket.Send(, 0, SocketFlags.None);
                SendNext(wst.NextIP, Serialization(msg));
                //Отправка ключа штатными средствами
                return DataType.PackageLabel.getkey;
            }
            else if (wst.label == DataType.PackageLabel.openkey)
            {
                log.printlog("[getdata] Package detected as OPENKEY");
                //MessageBox.Show(Encoding.UTF8.GetString(wst.Data, 0, wst.Data.Length));
                //wst.Data = RSA.Decrypt(wst.Data, false);
                if (NewMessageEvent != null)
                {
                    NewMessageEvent(wst, wst.NextIP);
                }
                return DataType.PackageLabel.final;
            }
            return DataType.PackageLabel.final;
        }

        public byte[] RSADecrypt(byte[] content)
        {
            log.printlog("[RSA] Start RSA decrypt");
            int z;
            for (z = 100000 - 1; z > 0; z--)
            {
                if (content[z] != 0)
                {
                    break;
                }
            }

            log.printlog("[RSA] Changed package size: " + z.ToString());

            List<byte[]> ResList = new List<byte[]>();
            byte[] buffer = new byte[128];
            //RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            // RSA.FromXmlString(KeyString);

            int SummarySize = 0;
            int i = 0;
            while (i <= z)
            {
                int buf_size = 0;
                while (buf_size < 128)
                {
                    buffer[buf_size] = content[i];
                    i++;
                    buf_size++;
                }
                byte[] decryptBuffer = RSA.Decrypt(buffer, false);
                SummarySize += decryptBuffer.Length;
                ResList.Add(decryptBuffer);
            }
            log.printlog("[RSA] New summary size: " + SummarySize.ToString());
            byte[] Result = new byte[SummarySize];
            int i_res = 0;
            for (int k = 0; k < ResList.Count; ++k)
            {
                foreach (byte b in ResList.ElementAt(k))
                {
                    Result[i_res] = b;
                    i_res++;
                }
            }
            log.printlog("[RSA] Stop RSAdecrypt ");
            return Result;

        }
        private void SendNext(IPAddress IP, byte[] Data)
        {
            Socket nextsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                log.printlog("[senddata] Start transit from serversending. Calling IP: " + IP.ToString() +
                    " port: " + basic.port.ToString());
                nextsock.Connect(IP, basic.port);
                nextsock.Send(Data, Data.Length, SocketFlags.None);
                nextsock.Close();
                log.printlog("[senddata] Transit sending done.");
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при отправке пакета из сервера", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.printlog("[ERROR][SendNext] " + ex.Message);
            }

        }
    }
}
