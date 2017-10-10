using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;
        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return AccountDAO.instance; }
            private set { AccountDAO.instance = value; }
        }
        private AccountDAO() { }
        public bool CheckLogin(String UserName, String Password)
        {
            String sqlCommand = string.Format(@"EXEC dbo.CheckLogin '{0}', '{1}'", UserName, Password);
            if (DataProvider.Instance.ExcuteScaler(sqlCommand) > 0)
                return true;
            return false;
        }

        public Account getAccount(String UserName)
        {
            String sql =string.Format(@"EXEC dbo.getAccount '{0}'", UserName);
            DataTable dt = new DataTable();
            Account acc = new Account();
           try
           {
               dt = DataProvider.Instance.LoadAllTable(sql);
                acc = new Account(dt.Rows[0]);
           }
            catch(Exception ex)
           {
               ErrorLog.WriteLog(ex.Message);
           }
            return acc;

        }
    }
}
