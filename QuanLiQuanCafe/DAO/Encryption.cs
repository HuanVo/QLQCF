using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.IO;

namespace DAO
{
    public class Encryption
    {
         private static Encryption instance;

        public static Encryption Instance
        {
            get {
                if(instance ==null)
                    instance = new Encryption();
                return Encryption.instance;
            }
            private set { Encryption.instance = value; }
        }
        private Encryption()
        { }

        private byte[] encryptData(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }

        public string md5(string data)
        {
            return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();//
        }

        public void EditAppSetting(string key, string value)
        {
            try
            {
                string configPath = Path.Combine(System.Environment.CurrentDirectory, "QuanLiQuanCafe.exe");

                Configuration config = ConfigurationManager.OpenExeConfiguration(configPath);
                config.AppSettings.Settings[key].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Không đủ quyền truy cập file, vui lòng chạy chương trình với quyền cao nhất" + System.Environment.NewLine + ex.Message);
            }

        }
    }
}
