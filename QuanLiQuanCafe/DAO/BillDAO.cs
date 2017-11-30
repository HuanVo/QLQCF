using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DTO;
namespace DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get
            { if (instance == null)
                    instance = new BillDAO();
                return instance; }
           private set { instance = value; }
        }
        private BillDAO(){}

        public bool CreateBill(String TableFood, String Employee)
        {
            String sqlOpenBill = string.Format(@"INSERT INTO dbo.Bill(DateCheckIn, DateCheckOut, idTableFood, idEmployee, stats) VALUES ( GETDATE(), GETDATE(), {0}, {1}, 'false')", TableFood, Employee);
            try
            {
                if(DataProvider.Instance.ExcuteNonQuery(sqlOpenBill)>0)
                {
                    return true;
                }
            }
            catch(Exception exx)
            {
                ErrorLog.WriteLog(exx.Message);
            }
            return false;
        }

        public int getIDBills(int IDTableFood)     
        {
            int kq = 0;

            try
            {
                String sqlgetIDBill = string.Format(@"select idBill from Bill where idTableFood = '{0}' and stats ='False'", IDTableFood);
                DataTable dt = DataProvider.Instance.LoadAllTable(sqlgetIDBill);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        kq = Convert.ToInt32(row["idBill"].ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            return kq;
        }

        public bool CheckThanhToan(String IDBill)
        {
            try
            {
                String sqlgetIDBill = string.Format(@"select count(*) from Bill where idBill = '{0}' and stats ='False'", IDBill);
                if (DataProvider.Instance.ExcuteScaler(sqlgetIDBill) > 0)
                    return true;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            return false;
            
        }

        public bool CloseBill(String IDBill)
        {
            try
            {
                String sqlgetIDBill = string.Format(@"update Bill set stats ='true', DateCheckOut = GETDATE() where idBill = '{0}'", IDBill);
                if (DataProvider.Instance.ExcuteNonQuery(sqlgetIDBill) > 0)
                    return true;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            return false;
        }

        public bool UpdateBill(String sqlUpdate)
        {
            try
            {
                if (DataProvider.Instance.ExcuteNonQuery(sqlUpdate) > 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                ErrorLog.WriteLog(e.Message);

            }
            return false;
        }

    }
}
