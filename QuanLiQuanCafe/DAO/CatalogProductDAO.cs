using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAO
{
    public class CatalogProductDAO
    {
        private static CatalogProductDAO instance;

        public static CatalogProductDAO Instance
        {
            get { if(instance == null)
                instance = new CatalogProductDAO();
                return CatalogProductDAO.instance;
            }
           private set { CatalogProductDAO.instance = value; }
        }
        private CatalogProductDAO()
        { }
        /// <summary>
        /// Thêm mới catalog
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
       public bool AddCatalog(String Name)
        {
            bool result = false;
            try
            {
                String sqlquery = String.Format(@"EXEC dbo.AddCatalog N'{0}'", Name);
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
        /// Load dữ liệu catalog
        /// </summary>
        /// <returns></returns>
       public DataTable LoadCatalog()
       {
           DataTable dt = null;
           try
           {
               String sqlquery = @"EXEC dbo.getTableCatalog";
               dt = DataProvider.Instance.LoadAllTable(sqlquery);
           }
           catch (Exception ex)
           {
               ErrorLog.WriteLog(ex.Message);
           }
           return dt;
       }
        /// <summary>
        /// Load catalog by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>get a record Catalog food</returns>
       public CatalogProduct LoadCatalogByID(int id)
       {
           CatalogProduct pro = null;
           try
           {
               String sqlquery = String.Format(@"getTableCatalogByID '{0}'", id);
               DataTable dt = DataProvider.Instance.LoadAllTable(sqlquery);
               pro = new CatalogProduct(dt.Rows[0]);
           }
           catch (Exception ex)
           {
               ErrorLog.WriteLog(ex.Message);
           }
           return pro;
       }
        /// <summary>
        /// Sửa Catalog
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
       public bool EditCatalog(int id, String Name)
       {
           bool result = false;
           try
           {
               String sqlquery = String.Format(@"EXEC dbo.editCatalog '{0}', N'{1}'", id, Name);
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
        /// Xóa catalog
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       public bool DeleteCatalog(int id)
       {
           bool result = false;
           try
           {
               String sqlquery = String.Format(@"EXEC dbo.DeleteCatalog '{0}'", id);
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

       public DataTable SearchCatalogExactly(String sql)
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
    }
}
