using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class DataProvider
    {
        private String ConnectionString = @"Data Source=DESKTOP-4TTRDJ3;Initial Catalog=CoffeeRes;Persist Security Info=True;User ID=sa;Password=huanit";
        private static DataProvider instance;
        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider();
                return DataProvider.instance; 
            }
          private set { DataProvider.instance = value; }
        }

        private DataProvider() { }
        private SqlDataAdapter da;

        public SqlDataAdapter Da
        {
            get { return da; }
            set { da = value; }
        }
        private SqlConnection Connection;
        private void OpenConnect()
        {
            try
            {
                using( Connection = new SqlConnection(ConnectionString))
                { 
}
                if (Connection.State == ConnectionState.Broken || Connection.State == ConnectionState.Closed)
                    Connection.Open();
            }
            
            catch(Exception exs)
            {
                
            }
        }

        //Phuong thuc dong ket noi den csdl
        private void CloseConnect()
        {
            try
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        // lay du lieu tren mot bang
        public DataTable LoadAllTable(String CommandString)
        {
            DataTable dt = new DataTable();
            try
            {
                OpenConnect();
                Da = new SqlDataAdapter(CommandString, Connection);
                Da.Fill(dt);
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            CloseConnect();
            return dt;
        }

        //phuong thuc update, insert, del du lieu
        public int ExcuteNonQuery(String str_Proc)
        {
            int kq = 0;
            OpenConnect();
            SqlCommand cmd = new SqlCommand(str_Proc, Connection);
            kq = cmd.ExecuteNonQuery();
            return kq;
        }

        public int ExcuteScaler(String str)
        {
            int kq = 0;
            try
            {
                OpenConnect();
                SqlCommand cmd = new SqlCommand(str, Connection);
                kq = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Không thể thực thi yêu cầu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            CloseConnect();
            return kq;
        }
    }
}
