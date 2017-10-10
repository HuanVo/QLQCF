using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class ErrorLog
    {
        
        public static void WriteLog(String MessError)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString("g") + ": " + MessError);
                sw.Flush();
                sw.Close();
            }
            catch
            {
                // ignored
            }

        }
        public static void WriteLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString("g") + ": " + ex.Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
                // ignored
            }

        }
    }
}
