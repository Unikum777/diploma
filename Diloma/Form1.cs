using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Diloma.DataType;
using Diloma.Sockets;
using Diloma.Managers;
using System.Net;
using System.IO;
using System.Threading;
using System.Security.Principal;


namespace Diloma
{
    public partial class Form1 : Form
    {

        TCPClient client;
        TCPServer serv;
        //SelectServer serv;
        UDPServer udp_serv;
        List<Chat> AllChats = new List<Chat>();
        BasicClient Basic;
        int tickcount = 0;
        int NextFake = 30;
        public Form1()
        {
            InitializeComponent();

            String HostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostByName(HostName);
            IPAddress[] addresses = ipEntry.AddressList;
            IPAddress addr = IPAddress.Parse("127.0.0.1");
            foreach (IPAddress a in addresses){
                if (a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    addr = a;
                    break;
                }
            }
            Basic = new BasicClient(19999, 9587, addr);

            client = new TCPClient(Basic);
            serv = new TCPServer(Basic);
           //serv = new SelectServer(Managers.ROUTING_TYPE.onion, DateTime.Parse("00:00:05"), 19999, 1456, addr);
            udp_serv = new UDPServer(Basic, HostName, serv.getpublickey());
      
            serv.Run();         //Запускаем TCP сервер обработки
            udp_serv.Run();     //Запускаем UDP сервер рассылки IP
            //MessageBox.Show("UDP Run");
            
            //MessageBox.Show("Server Run");
            serv.NewMessageEvent += new NewMessageDelegate(Handler);
            client.ScanHosts(1); //Поиск доступных хостов
            //MessageBox.Show("Scanhosts Run");
            HostsToPictures();

            //client.SendString("Приветик", new Workstation(serv.CurrentIP, serv.getpublickey()));
            //client.SendString("Ты тут?", new Workstation(serv.CurrentIP, serv.getpublickey()));

        }
        private void HostsToPictures()
        {
            String[] PictureFiles = Directory.GetFiles("avatars");
            Random rnd = new Random();
            dataGridView1.Rows.Clear();
            foreach (Workstation Station in client.workstations)
            {
                
                if (Station.PictureFile == null)
                {
                    Station.PictureFile = PictureFiles[rnd.Next(PictureFiles.Length-1)];
                }
                var dgvr = new DataGridViewRow();
                dgvr.Cells.Add(new DataGridViewImageCell() { Value = Image.FromFile(Station.PictureFile) });
                dgvr.Cells.Add(new DataGridViewTextBoxCell() { Value = Station.name +" ("+ Station.IPAddr.ToString() + ")" + "\r\n Новых сообщений: " + Station.couner.ToString()});
                //dgvr.Cells.Add(new DataGridViewTextBoxCell() { Value = Station.name + " (" + Station.IPAddr.ToString() + ")" + "\r\n Новых сообщений: " + Station.publickey});
                
                dgvr.DefaultCellStyle.BackColor = Color.White;
                dgvr.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvr.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvr.Height = 70;
                dgvr.MinimumHeight = 70;
                dataGridView1.Rows.Add(dgvr);
                //MessageBox.Show(Station.publickey);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Condition F = new Condition(Basic, serv);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void Handler(NetworkMessage s, IPAddress ip)
        {
            //MessageBox.Show("Мы знаем, что пришло сообщение : " + s);
            switch (s.label)
            {
                case (PackageLabel.final):
                {
                    foreach (Chat C in AllChats)
                    {
                        if (ip.ToString() == C.CurrentWorkstation.IPAddr.ToString())
                        {
                            C.ShowString(Encoding.UTF8.GetString(s.Data));
                            return;
                        }
                    }
                   // Workstation SetCurrentWorkstation;
                    for (int i = 0; i < client.workstations.Count; i++)
                    {
                        if (client.workstations[i].IPAddr.ToString() == ip.ToString())
                        {
                            client.workstations[i].buffer += Encoding.UTF8.GetString(s.Data) + "\r\n";
                            client.workstations[i].couner++;
                        }
                    }
                    break;
                }
                case (PackageLabel.openkey):
                {
                    for (int i = 0; i < client.workstations.Count; i++)
                    {
                        if (client.workstations[i].IPAddr.ToString() == ip.ToString())
                        {
                            client.workstations[i].publickey = Encoding.UTF8.GetString(s.Data);
                        }
                    }
                    break;
                }

            }//switch
            
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Chat CHatForm = new Chat(client.workstations[e.RowIndex], client, serv);
            CHatForm.FormClosing += CHatForm_FormClosing;
            AllChats.Add(CHatForm);
            CHatForm.Show();
        }

        void CHatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show("Мы знаем, что форма закрылась");
            AllChats.Remove((Chat)sender);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            client.ScanHosts(1); //Поиск доступных хостов
            HostsToPictures();
        }


        private void ClosingForm (Chat ip)
        {
            MessageBox.Show("Мы знаем, что форма закрылась");
            AllChats.Remove(ip);
        }
        private void OnLoaded(object sender, EventArgs e)
        {
            Application.Idle -= OnLoaded;
            //_waitForm.Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 0)
            {
                dataGridView1.SelectedCells[0].Selected = false;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Form Conditions = new Condition(Basic, serv);
            Conditions.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tickcount++;
            if (tickcount % Basic.RefreshTime == 0)
            {
                client.ScanHosts(Basic.WaitTime); //Поиск доступных хостов
                HostsToPictures();
            }
            if (Basic.SendFakePackages) 
            {
                if (tickcount % NextFake == 0)
                {
                    client.SendFake();
                    Random rnd = new Random();
                    NextFake = rnd.Next(30);
                }
            }

        }

    }
}
