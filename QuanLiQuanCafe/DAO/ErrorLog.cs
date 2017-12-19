using System;
using System.IO;

namespace DAO
{
    public class ErrorLog
    {
        
        public static void WriteLog(String messError)
        {
            StreamWriter sw;
            sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine("{0:g}: {1}", DateTime.Now, messError);
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
            try
            {
                var sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine("{0:g}: {1}", DateTime.Now, ex.Message);
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
