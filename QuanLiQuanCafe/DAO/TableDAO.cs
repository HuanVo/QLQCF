using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance
        {
            get {
                if (instance == null)
                    instance = new TableDAO();
                return instance; }
            private set { instance = value; }
        }

        private TableDAO()
        { }

        /// <summary>
        ///  Load bàn đưa vao datagridview
        /// </summary>
        /// <returns>get a Datatable table food</returns>
        public DataTable LoadTable()
        {
            DataTable dt = null;
            try
            {
                String sqlquery = @"EXEC dbo.getTableTableFood";
                dt = DataProvider.Instance.LoadAllTable(sqlquery);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            return dt;
        }

        /// <summary>
        /// Them moi TableFood
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool AddTableFood(String Name,bool status)
        {
            bool result = false;
            try
            {
                String sqlquery = String.Format(@"EXEC dbo.AddTableFood N'{0}', '{1}'", Name, status);
                if (DataProvider.Instance.ExcuteNonQuery(sqlquery) > 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                ErrorLog.WriteLog(e.Message);
            }
            return result;
        }
        /// <summary>
        /// Lấy bàn bởi id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Table LoadTableFoodID(int id)
        {
            Table pro = null;
            try
            {
                String sqlquery = String.Format(@"getTableTableFoodByID '{0}'", id);
                DataTable dt = DataProvider.Instance.LoadAllTable(sqlquery);
                pro = new Table(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            return pro;
        }
        
        public bool EditTableFood(int id, String Name, bool sts)
        {
            bool result = false;
            try
            {
                String sqlquery = String.Format(@"EXEC dbo.editTableFood '{0}', N'{1}', '{2}'", id, Name, sts);
                if (DataProvider.Instance.ExcuteNonQuery(sqlquery) > 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                ErrorLog.WriteLog(e.Message);
            }
            return result;
        }
        /// <summary>
        /// Xóa Bàn
        /// </summary>
        /// <param name="id"></param>
        /// <returns>get a value bool type</returns>
        public bool DeleteTableFood(int id)
        {
            bool result = false;
            try
            {
                String sqlquery = String.Format(@"EXEC dbo.deleteTableFood '{0}'", id);
                if (DataProvider.Instance.ExcuteNonQuery(sqlquery) > 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                ErrorLog.WriteLog(e.Message);
            }
            return result;
        }

        public DataTable SearchTableFoodExactly(String sql)
        {
            DataTable dt = null;
            try
            {
                dt = DataProvider.Instance.LoadAllTable(sql);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            return dt;
        }

        public List<Table> GetListTable()
        {
            List<Table> Result = new List<Table>();
            DataTable dt = new DataTable();
            String sqlString = @"EXEC dbo.loadAllTableFood";
            dt = DataProvider.Instance.LoadAllTable(sqlString);
            if(dt.Rows.Count>0)
            {
                foreach(DataRow item in dt.Rows)
                {

                    Result.Add(new Table(item));
                }
            }
            return Result;
        }

    }
}
