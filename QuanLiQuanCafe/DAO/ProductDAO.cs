using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class ProductDAO
    {
        private static ProductDAO instance;

        public static ProductDAO Instance
        {
            get {
                if (instance == null)
                    instance = new ProductDAO();
                return instance;
            }
            private set { instance = value; }
        }
        private ProductDAO()
        { }
        /// <summary>
        /// Load toàn bộ sản phẩm
        /// </summary>
        /// <returns></returns>
        public DataTable LoadProduct()
        {
            DataTable dt = null;
            try
            {
                String sqlquery = @"EXEC dbo.getTableProduct";
                dt = DataProvider.Instance.LoadAllTable(sqlquery);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            return dt;
        }

        /// <summary>
        /// Thêm mới sản phẩm
        /// </summary>
        /// <param name="Empl"></param>
        /// <returns></returns>
        public bool AddProduct(Product pro)
        {
            bool result = false;
            try
            {
                String sqlquery = String.Format(@"EXEC dbo.addProduct N'{0}', {1}, N'{2}', '{3}'", pro.Name, pro.Idcatalog, pro.Unit, pro.Price);
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
        /// Sửa sản phẩm
        /// </summary>
        /// <param name="pro"></param>
        /// <returns></returns>
        public bool EditProduct(Product pro)
        {
            bool result = false;
            try
            {
                String sqlquery = String.Format(@"EXEC dbo.editProduct '{0}', N'{1}', '{2}', '{3}', '{4}'",pro.Id, pro.Name, pro.Unit, pro.Price, pro.Idcatalog);
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
        /// Xóa sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteProduct(int id)
        {
            bool result = false;
            try
            {
                String sqlquery = String.Format(@"EXEC dbo.DeleteProduct '{0}'", id);
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
        /// Load sản phẩm bởi mã sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product LoadProductByID(int id)
        {
            Product pro = null;
            try
            {
                String sqlquery = String.Format(@"getProductByID '{0}'", id);
                DataTable dt = DataProvider.Instance.LoadAllTable(sqlquery);
                pro = new Product(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            return pro;
        }
        /// <summary>
        ///  Tìm kiếm chính xác sản phẩm
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable SearchProductExactly(String sql)
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
