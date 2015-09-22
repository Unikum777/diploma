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
using Diloma.Managers;
using Diloma.Sockets;

namespace Diloma
{
    public partial class Condition : Form
    {
        BasicClient Basic;
        TCPServer serv;
        public Condition(BasicClient client, TCPServer server)
        {
            InitializeComponent();
            Basic = client;
            serv = server;
            UDPport.Text = Basic.UDPport.ToString();
            TCPport.Text = Basic.port.ToString();
            UDPmask.Text = Basic.UDPmask;
            RefreshTime.Text = Basic.RefreshTime.ToString();
            TimeOut.Text = Basic.WaitTime.ToString();
            MaxNode.Text = Basic.MaxNodes.ToString();
            Layers.Text = Basic.LayerCount.ToString();
            MaxConnections.Text = Basic.MaxConnections.ToString();
            FakeMessages.Checked = Basic.SendFakePackages;
            KeyRefreshTime.Text = Basic.KeyRefreshTime.ToString("R");

        }
        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Basic.UDPport = Convert.ToInt32(UDPport.Text);
            Basic.port =Convert.ToInt32(TCPport.Text);
            Basic.UDPmask = UDPmask.Text;
            Basic.RefreshTime =Convert.ToInt32(RefreshTime.Text);
            Basic.WaitTime = Convert.ToInt32(TimeOut.Text);
            Basic.MaxNodes = Convert.ToInt32(MaxNode.Text);
            Basic.LayerCount = Convert.ToInt32(Layers.Text);
            Basic.MaxConnections = Convert.ToInt32(MaxConnections.Text);
            Basic.SendFakePackages=FakeMessages.Checked;
            Close();
            //Basic.KeyRefreshTime = KeyRefreshTime.Text
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serv.RefreshRSAKey();
            Basic.KeyRefreshTime = DateTime.Now;
            KeyRefreshTime.Text = Basic.KeyRefreshTime.ToString("R");
        }
    }
}
