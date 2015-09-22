using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace Diloma.DataType
{
    class ConnectionInfo
    {
        public Socket Socket;
        public NetworkMessage Msg;
        public byte[] Buffer;
    }
}
