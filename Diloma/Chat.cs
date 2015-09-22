using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

using Diloma.DataType;
using Diloma.Managers;
using Diloma.Sockets;

namespace Diloma
{
    public partial class Chat : Form
    {
        public Workstation CurrentWorkstation;
        TCPClient client;
        TCPServer server;
        //SelectServer server;
        public Chat(Workstation Current, TCPClient client, TCPServer server)
        {
            InitializeComponent();

            CurrentWorkstation = Current;
            this.client = client;
            this.server = server;
            pictureBox1.BackgroundImage = Image.FromFile(Current.PictureFile);
            label2.Text = Current.name;
            this.Text = "Чат с "+ Current.name;
            if (Current.couner > 0)
            {
                history.Text = Current.name + ": " + Current.buffer;
                Current.buffer = "";
                Current.couner = 0;
            }

            //server.NewMessageEvent += new NewMessageDelegate(Handler);
        }
        /*public Chat(Workstation Current, TCPClient client, SelectServer server)
        {
            InitializeComponent();

            CurrentWorkstation = Current;
            this.client = client;
            this.server = server;
            pictureBox1.BackgroundImage = Image.FromFile(Current.PictureFile);
            label2.Text = Current.name;
            this.Text = "Чат с " + Current.name;
            if (Current.couner > 0)
            {
                history.Text = Current.name + ": " + Current.buffer;
                Current.buffer = "";
                Current.couner = 0;
            }

            //server.NewMessageEvent += new NewMessageDelegate(Handler);
        }*/
        private void send_Click(object sender, EventArgs e)
        {
            client.SendString(messagelist.Text, CurrentWorkstation);
            history.Text += "Я: " + messagelist.Text + "\r\n";
            messagelist.Text = "";
        }

        public void ShowString(String s)
        {
            s = CurrentWorkstation.name + ": " + s + "\r\n";
            if (history.InvokeRequired)
            {
                history.Invoke(new Action<string>((str) => history.Text += str), s);
                //history.Invoke(new NewMessageDelegate(), s);
            }
            else
            {
                history.Text = s;
            }
        }
        private void Handler(String s, IPAddress ip)
        {
            //MessageBox.Show("Мы знаем, что пришло сообщение : " + s);
          //  if (ip.Equals(CurrentWorkstation.IPAddr))
            //MessageBox.Show(ip.ToString());
            if (ip.ToString() ==  CurrentWorkstation.IPAddr.ToString())
            {
                s = CurrentWorkstation.name + ": " + s + "\r\n";
                if (history.InvokeRequired)
                {
                    history.Invoke(new Action<string>((str) => history.Text += str), s);
                    //history.Invoke(new NewMessageDelegate(), s);
                }
                else
                {
                    history.Text = s;
                }
            }
        }
    }
}
