using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;

namespace DAO
{
    public class DataProvider
    {
        private static String Data_Source = @"Data Source = " + ConfigurationManager.AppSettings["Data_Source"];
        private static String Initial_Catalog = @"Initial Catalog = " + ConfigurationManager.AppSettings["Initial_Catalog"];
        private static String Persist_Security_Info = @"Persist Security Info = " + ConfigurationManager.AppSettings["Persist_Security_Info"];
        private static String User_ID = @"User ID = " + ConfigurationManager.AppSettings["User_ID"];
        private static String Password = @"Password = " + ConfigurationManager.AppSettings["Password"];
        private String ConnectionString = Data_Source + "; "
            + Initial_Catalog + "; "
            + Persist_Security_Info + "; " 
            + User_ID +";"
            + Password;
        //private String ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;
        private static DataProvider instance;
        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider();
                return DataProvider.instance; 
            }
          private set { DataProvider.instance = value; }
        }

        
        private DataProvider() { }
        
        private SqlConnection Connection;
        public bool OpenConnect()
        {
            try
            {
                Connection = new SqlConnection(ConnectionString);
                
                    if (Connection.State == ConnectionState.Broken || Connection.State == ConnectionState.Closed)
                    {
                        Connection.Open();
                        if (Connection.State == ConnectionState.Open)
                        {
                            ErrorLog.WriteLog("Connected");
                            return true;
                        } 
                        return false;
                }
            }
            
            catch(Exception exs)
            {
                ErrorLog.WriteLog(exs.Message);
                //return false;
            }
            return false;
        }

        //Phuong thuc dong ket noi den csdl
        public void CloseConnect()
        {
            try
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                    if(Connection.State == ConnectionState.Closed)
                    {
                        ErrorLog.WriteLog("Disconnected");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
        }
        private SqlDataAdapter Da;
        /// <summary>
        /// lay du lieu tren mot bang
        /// </summary>
        /// <param name="CommandString"></param>
        /// <returns></returns>
 
        public DataTable LoadAllTable(String CommandString)
        {
            DataTable dt = new DataTable();
            try
            {
                if (OpenConnect())
                {
                    Da = new SqlDataAdapter(CommandString, Connection);
                    Da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            CloseConnect();
            return dt;
        }
        /// <summary>
        /// phuong thuc update, insert, del du lieu
        /// </summary>
        /// <param name="str_Proc"></param>
        /// <returns></returns>
        public int ExcuteNonQuery(String str_Proc)
        {
            int result = 0;
            try
            {
                if (OpenConnect())
                {
                    SqlCommand cmd = new SqlCommand(str_Proc, Connection);
                    result = cmd.ExecuteNonQuery();
                }
            } catch(Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            CloseConnect();
            return result;
        }
        /// <summary>
        /// dem, thong ke so dong du lieu
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int ExcuteScaler(String str)
        {
            int result = 0;
            try
            {
               if( OpenConnect())
               {
                   SqlCommand cmd = new SqlCommand(str, Connection);
                   result = (int)cmd.ExecuteScalar();
               }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            CloseConnect();
            return result;
        }
    }
}
