using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DAO;

namespace QuanLiQuanCafe.Report
{
    public partial class ReportCheckOut : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportCheckOut()
        {
            InitializeComponent();
        }
        public void BindData()
        {
            lblStt.DataBindings.Add("Text", DataSource, "stt");
            lblDoUong.DataBindings.Add("Text", DataSource, "product");
            lblDVT.DataBindings.Add("Text", DataSource, "dvt");
            lblSoLuong.DataBindings.Add("Text", DataSource, "soluong");
            lblDG.DataBindings.Add("Text", DataSource, "dongia");
            lblThanhTien.DataBindings.Add("Text", DataSource, "thanhtien");
            
        }
        public void huanit(String idbill, int SaleOff, int Thue)
        {
            {
                String sql = string.Format(@"SELECT dbo.Food.price*dbo.BillInfo.count AS[sum] FROM dbo.BillInfo INNER JOIN dbo.Food ON Food.idFood = BillInfo.idFood WHERE idBill = '{0}'", idbill);
                DataTable dt = DataProvider.Instance.LoadAllTable(sql);
                Double MoneyProduct = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        MoneyProduct = MoneyProduct + Convert.ToDouble(row["sum"].ToString());
                    }
                }
                lblSaleOff.Text = SaleOff.ToString();
                lblVAT.Text = Thue.ToString();
                txtMasothue.Text = idbill;
                lblMasothue.Text = idbill;
                lblSumproduct.Text = string.Format("{0:0,0  }",MoneyProduct);

                Double TienThue = 0;
                Double TienSaleOff = 0;
                if (SaleOff > 0)
                {
                    TienSaleOff = (MoneyProduct * SaleOff) / 100;
                }
                if (Thue > 0)
                {
                    TienThue = MoneyProduct * Thue / 100;
                    lblSumCheckOut.Text = string.Format("{0:0,0  }",(MoneyProduct - TienSaleOff + TienThue));
                }
                else
                {
                    lblSumCheckOut.Text = string.Format("{0:0,0  }",(MoneyProduct - TienSaleOff));
                }

            }
        }
        
    }
    
}
