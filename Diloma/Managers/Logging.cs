using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Diloma.Managers
{
    public class Logging
    {
        String LogFile = "ClientLog.acl";

        public Logging (String FileName){
            LogFile = FileName;
            printlog("startlog");
        }

        public void printlog(String str)
        {
            try
            {
                using (System.IO.StreamWriter logging = new System.IO.StreamWriter(LogFile, true))
                {
                    logging.WriteAsync(DateTime.Now.ToString("R") + " " + str + "\r\n");
                }
                
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Ошибка журналирования", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
