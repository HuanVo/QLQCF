using DTO;
using System;
using System.Data;

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
        public bool CheckLogin(String userName, String password)
        {
            //if (userName == null) throw new ArgumentNullException("userName");
            String sqlCommand = string.Format(@"EXEC dbo.CheckLogin '{0}', '{1}'", userName, password);
            if (DataProvider.Instance.ExcuteScaler(sqlCommand) > 0)
                return true;
            return false;
        }

        public Account GetAccount(String userName)
        {
            String sql =string.Format(@"EXEC dbo.getAccount '{0}'", userName);
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
