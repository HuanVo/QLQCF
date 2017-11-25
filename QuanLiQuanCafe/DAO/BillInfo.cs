using System;
using System.Collections.Generic;
using System.Linq;
using DTO;
namespace DAO
{
    public class BillInfo
    {
        private static BillInfo instance;

        public static BillInfo Instance
        {
            get { if(instance == null)
                instance = new BillInfo();
                return instance;
            }
           private set { instance = value; }
        }
        private BillInfo() { }

        public bool CreateBillInfo(String idBill, String idFood, String Count)
        {
            String sqlOpenBill = string.Format(@"INSERT INTO dbo.BillInfo( idBill, idFood, count ) VALUES ( {0},{1},{2})", idBill, idFood, Count);
            try
            {
                if (DataProvider.Instance.ExcuteNonQuery(sqlOpenBill) > 0)
                {
                    return true;
                }
            }
            catch (Exception exx)
            {
                ErrorLog.WriteLog(exx.Message);
            }
            return false;
        }

        public bool UpdateBillInfo(int idBill, int idFood, int Count)
        {

            String sqlOpenBill = string.Format(@"EXEC UpdateBillInfo {0}, {1}, {2}", Count, idBill, idFood);
            try
            {
                if (DataProvider.Instance.ExcuteNonQuery(sqlOpenBill) > 0)
                {
                    return true;
                }
            }
            catch (Exception exx)
            {
                ErrorLog.WriteLog(exx.Message);
            }
            return false;
        }
    }
}
