using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diloma.Managers
{
    enum Algorithm { 
        AES,
        DES,
        RSA
    }
    class Protection
    {
        //private byte[] secretkey{set;}
        private byte[] secretkey = new byte[255];
        public static byte[] UniversalEncode(Algorithm alg, byte[] Data, byte[] openkey) {
            return null;
        }

        public byte[] Decode(Algorithm alg, byte[] Data) {
            return null;
        }
    }
}
