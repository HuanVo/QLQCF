using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class ReportDAO
    {
        private static ReportDAO instance;

        public static ReportDAO Instance
        {
            get {
                if (instance == null)
                    instance = new ReportDAO();
                return ReportDAO.instance; }
            private set { ReportDAO.instance = value; }
        }

        private ReportDAO() { }

        public DataTable LoadBaoCao()
        {
            String sql = @"EXEC dbo.LoadTKBC";

            DataTable dt = DataProvider.Instance.LoadAllTable(sql);
            return dt;
        }

        public DataTable LoadBaoCaoChiTietBillByIdBill(String idBill)
        {
            String sql = string.Format(@"EXEC LoadTKBCDetailBillById '{0}'", idBill);

            DataTable dt = DataProvider.Instance.LoadAllTable(sql);
            return dt;
        }
    }
}
