using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Diloma.DataType
{
    public enum PackageLabel {
        getkey,
        transit,
        final,
        openkey,
        fake
    };
    [Serializable()]
    public class NetworkMessage {
        public PackageLabel label { get; set; }
        public DateTime TimeStamp { get; set; }
        public byte[] Data { get; set; }
        public IPAddress NextIP;
    }
}
