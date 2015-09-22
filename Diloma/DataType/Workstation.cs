using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Collections;

namespace Diloma.DataType
{
    public class Workstation
    {
        public IPAddress IPAddr { get; set;}
        public String publickey { get; set; }
        public String name { get; set; }

        public bool isChange { get; set; }

        public String buffer;
        public int couner = 0;
        public String PictureFile {get; set;}
        //public static bool Contains();
        public Workstation(byte[] ip, String key)
        {
            IPAddr = new IPAddress(ip);
            publickey = key;
        }
        public Workstation(IPAddress ip, String key)
        {            IPAddr = ip;
            publickey = key;
        }
    }

    class WorkstationEqualityComparer : IEqualityComparer<Workstation>
    {

        public bool Equals(Workstation b1, Workstation b2)
        {
            if (b1.IPAddr.GetHashCode() == b2.IPAddr.GetHashCode())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int GetHashCode(Workstation bx)
        {
            return bx.IPAddr.GetHashCode();
        }

    }
}
