using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Diloma.DataType;
using Diloma.Managers;
using System.Security.Cryptography;

namespace Diloma.Sockets
{
    public class TCPClient
    {
        
        public List<Workstation> workstations;

        int layernum = 3;
        int ReadingLimit = 2048;    //Лимит размера сериализованного экземпляра
        int MaxClients = 5;
        int TimeLimit = 5000;          //в миллисекундах
        Logging log;
        BasicClient basic;
        public TCPClient(BasicClient basic_client)
        {
            basic = basic_client;
            workstations = new List<Workstation>();
            log = new Logging("Clientlog.acl");
        }
        public void SendFake()
        {
            Random rnd = new Random();
            Workstation IP = workstations[rnd.Next(workstations.Count)];
            log.printlog("[send] IP: " + IP.IPAddr.ToString() + " fake string" );

            /*Разметка приемочного пакета*/
            NetworkMessage Sendmsg = new NetworkMessage();
            Sendmsg.label = PackageLabel.fake;     //Финальный пакет
            Sendmsg.TimeStamp = DateTime.Now;       //Будет иметь временной штамп,
            Sendmsg.NextIP = basic.CurrentIP;             //IP отправителя (то есть текущий)
            Encoding Utf8 = Encoding.UTF8;

            /*Этот пакет должен иметь "транзитника последней мили" 
             чтобы адес отправителя всегда был равен нужному приемнику*/
            String TunnelPath = "";

            NetworkMessage Buffer = new NetworkMessage();
            Buffer.Data = RSAEncrypt(Serialization(Sendmsg), IP.publickey);
            Buffer.TimeStamp = DateTime.Now;
            Buffer.label = PackageLabel.transit;
            Buffer.NextIP = IP.IPAddr;
            TunnelPath += IP.IPAddr.ToString() + " ";
            Sendmsg = Buffer;
            for (int i = 0; i < layernum - 1; i++)
            {
                int rand = rnd.Next(workstations.Count);
                Buffer.Data = RSAEncrypt(Serialization(Sendmsg), workstations[rand].publickey);
                Buffer.TimeStamp = DateTime.Now;
                Buffer.label = PackageLabel.transit;
                Buffer.NextIP = workstations[rand].IPAddr;
                TunnelPath += workstations[rand].IPAddr.ToString() + " ";
                Sendmsg = Buffer;
            }

            int rnd_next = rnd.Next(workstations.Count);
            //MessageBox.Show("Сообщение готовится к отправке по туннелю");
            basic.SendNext(workstations[rnd_next].IPAddr, RSAEncrypt(Serialization(Sendmsg), workstations[rnd_next].publickey), log);
            log.printlog("[tunnel] size: " + layernum.ToString() + " path: " + TunnelPath + " " + workstations[rnd_next].IPAddr.ToString());

        }

        public void SendString(String str, Workstation IP)
        {
            Random rnd = new Random();
            log.printlog("[send] IP: " + IP.IPAddr.ToString() + " msg: " + str);
            
            /*Разметка приемочного пакета*/
            NetworkMessage Sendmsg = new NetworkMessage();
            Sendmsg.label = PackageLabel.final;     //Финальный пакет
            Sendmsg.TimeStamp = DateTime.Now;       //Будет иметь временной штамп,
            Sendmsg.NextIP = basic.CurrentIP;             //IP отправителя (то есть текущий)
            Encoding Utf8 = Encoding.UTF8;
            Sendmsg.Data = Utf8.GetBytes(str);      //и данные.
            
            /*Этот пакет должен иметь "транзитника последней мили" 
             чтобы адес отправителя всегда был равен нужному приемнику*/
            String TunnelPath = "";

            NetworkMessage Buffer = new NetworkMessage();
            Buffer.Data = RSAEncrypt(Serialization(Sendmsg), IP.publickey);
            Buffer.TimeStamp = DateTime.Now;
            Buffer.label = PackageLabel.transit;
            Buffer.NextIP = IP.IPAddr;
            TunnelPath += IP.IPAddr.ToString() + " ";
            Sendmsg = Buffer;
            for (int i = 0; i < layernum-1; i++)
            {
                int rand = rnd.Next(workstations.Count);
                Buffer.Data = RSAEncrypt(Serialization(Sendmsg), workstations[rand].publickey);
                Buffer.TimeStamp = DateTime.Now;
                Buffer.label = PackageLabel.transit;
                Buffer.NextIP = workstations[rand].IPAddr;
                TunnelPath += workstations[rand].IPAddr.ToString() + " ";
                Sendmsg = Buffer;
            }

            int rnd_next = rnd.Next(workstations.Count);
            //MessageBox.Show("Сообщение готовится к отправке по туннелю");
            basic.SendNext(workstations[rnd_next].IPAddr, RSAEncrypt(Serialization(Sendmsg), workstations[rnd_next].publickey), log);
            log.printlog("[tunnel] size: " + layernum.ToString() + " path: " + TunnelPath + " " + workstations[rnd_next].IPAddr.ToString());
        
        }
        private byte[] RSAEncrypt(byte[] content, String PublicKey)
        {
            byte[] buffer = new byte[100];

            List<byte[]> Encrypted = new List<byte[]>();
            int i = 0;
            int SummarySize = 0;
            RSACryptoServiceProvider RSAProvider = new RSACryptoServiceProvider();
            RSAProvider.FromXmlString(PublicKey);
            while (i < content.Length)
            {
                for (int j = 0; j < 100 && i < content.Length; j++)
                {
                    buffer[j] = content[i];
                    i++;
                }
                //i=i+128;
                byte[] enc = RSAProvider.Encrypt(buffer, false);
                SummarySize += enc.Length;
                Encrypted.Add(enc);
            }

            byte[] Result = new byte[SummarySize];
            int i_res = 0;
            for (int k = 0; k < Encrypted.Count; ++k)
            {
                foreach (byte b in Encrypted.ElementAt(k))
                {
                    Result[i_res] = b;
                    i_res++;
                }
            }
            return Result;
        }
        public void SendNonEncryptMessage(NetworkMessage msg, IPAddress IP)
        {
            basic.SendNext(IP, Serialization(msg), log);
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

        public void GetKey(ref Workstation station)
        {
            NetworkMessage Sendmsg = new NetworkMessage();
            Sendmsg.label = PackageLabel.getkey;
            Sendmsg.TimeStamp = DateTime.Now;
            Sendmsg.NextIP = basic.CurrentIP;
            Sendmsg.Data = null;
            byte[] Data = Serialization(Sendmsg);
            byte[] key = new byte[100000];
            try
            {
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.Connect(station.IPAddr, basic.port);
                sock.Send(Data, Data.Length, SocketFlags.None);
                sock.Receive(key, SocketFlags.None);
                sock.Close();
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при отправке пакета с запросом на ключ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void ScanHosts(int ScanTimeOut) {
            //workstations = new List<Workstation>();

            ScanTimeOut = ScanTimeOut * 10000000;
            UdpClient listener = new UdpClient(basic.UDPport);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse("192.168.88.255"), basic.UDPport);
            DateTime startTime = DateTime.Now;
            DateTime TimeStep = DateTime.Now;
            //workstations = new List<Workstation>();
            try
            {
                while (TimeStep.Ticks - startTime.Ticks <= ScanTimeOut)
                {
                    //Console.WriteLine("Waiting for broadcast");
                    byte[] bytes = listener.Receive(ref groupEP);
                    //MessageBox.Show(groupEP.ToString() + Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                    String[] IP = groupEP.ToString().Split(new Char[] {':'});

                    String[] HostInfo = Encoding.UTF8.GetString(bytes).Split(new Char[] { '|' });

                    Workstation NeoFit = new Workstation(IPAddress.Parse(IP[0]), null);
                    NeoFit.name = HostInfo[0];
                    NeoFit.publickey = HostInfo[1];
                    NeoFit.isChange = true;

                    if (!workstations.Contains(NeoFit, new WorkstationEqualityComparer()))
                    {
                        workstations.Add(NeoFit);
                    }
                    else
                    {
                        foreach (Workstation t in workstations)
                        {
                            if (t.IPAddr.ToString() == NeoFit.IPAddr.ToString())
                            {
                                t.name = NeoFit.name;
                                t.publickey = NeoFit.publickey;
                                t.isChange = true;
                            }
                        }
                    }
                    TimeStep = DateTime.Now;
                }
                foreach (Workstation t in workstations)
                {
                    if (t.isChange)
                    {
                        t.isChange = false;
                    } 
                    else
                    {
                        workstations.Remove(t);
                    }
                }
                log.printlog("[scanhosts] array size: " + workstations.Count.ToString());
                MessageBox.Show("Сканирование хостов завершено");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Close();
            }
        }
    }
}
