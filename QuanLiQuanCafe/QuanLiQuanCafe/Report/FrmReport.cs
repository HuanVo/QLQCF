using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DAO;

namespace QuanLiQuanCafe.Report
{
    public partial class FrmReport : DevExpress.XtraEditors.XtraForm
    {
        private String IDBill = "";
        private String SaleOff = "";
        private String VAT = "";
        public FrmReport(String idbill, String saleoff, String VAT)
        {
            InitializeComponent();
            SaleOff = saleoff;
            this.VAT = VAT;
            IDBill = idbill;
        }
         
        private void FrmReport_Load(object sender, EventArgs e)
        {
            ReportCheckOut rp = new ReportCheckOut();
            rp.DataSource = ShowBillInFor(IDBill);
            rp.BindData();
            rp.huanit(IDBill, Convert.ToInt32(SaleOff), Convert.ToInt32(VAT));
            documentViewer1.PrintingSystem = rp.PrintingSystem;
            rp.CreateDocument();
        }

        public DataTable ShowBillInFor(String IDBill)
        {
            DataTable dtsource = new DataTable();
            try
            {
                String sql = string.Format(@"SELECT dbo.Food.name, dbo.BillInfo.count, dbo.Food.unit, dbo.Food.price, dbo.Food.price*dbo.BillInfo.count AS[sum] FROM dbo.BillInfo INNER JOIN dbo.Food ON Food.idFood = BillInfo.idFood WHERE idBill = '{0}'", IDBill);
                DataTable dt = DataProvider.Instance.LoadAllTable(sql);
                dtsource.Columns.Add("stt", typeof(int));
                dtsource.Columns.Add("product", typeof(string));
                dtsource.Columns.Add("dvt", typeof(string));
                dtsource.Columns.Add("soluong", typeof(int));
                dtsource.Columns.Add("dongia", typeof(double));
                dtsource.Columns.Add("thanhtien", typeof(double));
                if (dt.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        DataRow rs = dtsource.NewRow();
                        i = i + 1;
                        rs["stt"] = i.ToString();
                        rs["product"] = (row["name"].ToString());
                        rs["dvt"] = (row["unit"].ToString());
                        rs["soluong"] = (row["count"].ToString());
                        rs["dongia"] = row["price"];
                        rs["thanhtien"] = row["sum"];
                        dtsource.Rows.Add(rs);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dtsource;

        }
    }
}