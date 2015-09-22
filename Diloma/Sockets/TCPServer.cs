using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

using Diloma.DataType;
using Diloma.Managers;

namespace Diloma.Sockets
{
    public delegate void NewMessageDelegate(NetworkMessage data, IPAddress ip);  
    public class TCPServer
    {
        //Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<ConnectionInfo> connections = new List<ConnectionInfo>();
        int MaxConnections = 10;
        Logging log;
        BasicClient basic;
        RSACryptoServiceProvider RSA;
        public string getpublickey()
        {
            return RSA.ToXmlString(false);
        }
        public TCPServer(BasicClient basic_client)
        {
            basic = basic_client;
            log = new Logging("Serverlog.acl");
            RefreshRSAKey();
        }
        
        public void RefreshRSAKey(){
            
            RSA = new RSACryptoServiceProvider();
            log.printlog("[RSA] generate new private/open key. Open key: " + RSA.ToXmlString(false));
            basic.KeyRefreshTime = DateTime.Now;
        }
        public void SetupServerSocket (Socket sock) { 
            // Получаем информацию о локальном компьютере
            try
            {
                IPHostEntry localMachineInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPEndPoint myEndpoint = new IPEndPoint(basic.CurrentIP, basic.port);

                // Создаем сокет, привязываем его к адресу
                // и начинаем прослушивание
                
                sock.Bind(myEndpoint);
                sock.Listen((int)SocketOptionName.MaxConnections);
                log.printlog("[listen] start listening");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Ошибка запуска сервера", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.printlog("[ERROR][SetupServerSocket] ex.Message");
            }
        }
        public void Run() {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SetupServerSocket(sock);
            for (int i = 0; i < MaxConnections; i++)
                sock.BeginAccept(new AsyncCallback(AcceptCallback), sock);

        }
        private void AcceptCallback(IAsyncResult result)
        {
            ConnectionInfo connection = new ConnectionInfo();
            log.printlog("[listen] start accept callback");
            try
            {
                // Завершение операции Accept
                Socket s = (Socket)result.AsyncState;

                connection.Socket = s.EndAccept(result);
                connection.Msg = new Diloma.DataType.NetworkMessage();
                connection.Buffer = new byte[100000];
                log.printlog("[getdata] New data is available. buffer size = " + connection.Buffer.Length.ToString()+
                    " Call receive procedure");

                lock (connections) connections.Add(connection);

                // Начало операции Receive и новой операции Accept
                connection.Socket.BeginReceive(connection.Buffer,
                    0, connection.Buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReceiveCallback),
                    connection);
   
                s.BeginAccept(new AsyncCallback(
                    AcceptCallback), result.AsyncState);
            }
            catch (SocketException exc)
            {
                CloseConnection(connection);
                MessageBox.Show("Socket exception: " +
                    exc.SocketErrorCode, "Ошибка асинхронного вызова");
                log.printlog("[ERROR][AcceptCallback]" + exc.Message);
            }
            catch (Exception exc)
            {
                CloseConnection(connection);
                MessageBox.Show("Exception: " + exc);
                log.printlog("[ERROR][AcceptCallback]" + exc.Message);
            }
        }
        private void ReceiveCallback(IAsyncResult result)
        {
            ConnectionInfo connection =
                (ConnectionInfo)result.AsyncState;
            log.printlog("[getdata] ReceiveCallback");
            //MessageBox.Show(connection.Buffer.ToString());
            try
            {
                int bytesRead =
                    connection.Socket.EndReceive(result);
                if (0 != bytesRead)
                {
                    lock (connections)
                    {
                        //Здесь происходит обработка 
                        //MessageBox.Show("Сообщение отправлено на проверку");
                        CheckData(connection.Buffer);                        
                    }
                    connection.Socket.BeginReceive(
                        connection.Buffer, 0,
                        connection.Buffer.Length, SocketFlags.None,
                        new AsyncCallback(ReceiveCallback),
                        connection);
                }
                else CloseConnection(connection);
            }
            catch (SocketException exc)
            {
                CloseConnection(connection);
                MessageBox.Show ("Socket exception: " +
                    exc.SocketErrorCode);
                log.printlog("[ERROR][ReceiveCallback]" + exc.SocketErrorCode);
            }
            catch (Exception exc)
            {
                CloseConnection(connection);
                MessageBox.Show("Exception: " + exc.Message);
                log.printlog("[ERROR][ReceiveCallback]" + exc.Message);
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
                basic.SendNext(wst.NextIP, wst.Data, log);
                return DataType.PackageLabel.transit;
            }
            else if (wst.label == DataType.PackageLabel.fake)
            {
                log.printlog("[getdata] Package detected as FAKE");
                return DataType.PackageLabel.fake;
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
                 basic.SendNext(wst.NextIP, Serialization(msg), log);
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
            for (z = 100000-1; z > 0; z--)
            {
                if (content[z] != 0) {
                    break; }
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

        private void CloseConnection(ConnectionInfo ci)
        {
            ci.Socket.Close();
            lock (connections) connections.Remove(ci);
        }

    }
}
