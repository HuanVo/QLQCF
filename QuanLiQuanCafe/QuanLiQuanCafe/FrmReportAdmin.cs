using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DAO;

namespace QuanLiQuanCafe
{
    public partial class FrmReportAdmin : DevExpress.XtraEditors.XtraForm
    {
        public FrmReportAdmin()
        {
            InitializeComponent();
        }

        private void backstageViewTabItem1_SelectedChanged(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            try
            {
                //String sql = string.Format(@"SELECT dbo.Food.name, dbo.BillInfo.count, dbo.Food.unit, dbo.Food.price, dbo.Food.price*dbo.BillInfo.count AS[sum] FROM dbo.BillInfo INNER JOIN dbo.Food ON Food.idFood = BillInfo.idFood WHERE idBill = '{0}'", IDBill);
                DataTable dt = ReportDAO.Instance.LoadBaoCao();
                lstReport.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        i = i + 1;
                        ListViewItem im = new ListViewItem(i.ToString());
                        if (i % 2 == 0)
                        {
                            im.BackColor = Color.FromArgb(255, 244, 202);
                        }
                        im.SubItems.Add(row["idBill"].ToString());
                        im.SubItems.Add(String.Format("{0:M/d/yyyy}", row["DateCheckOut"]));
                        im.SubItems.Add(row["saleoff"].ToString());
                        im.SubItems.Add(row["vat"].ToString());
                        im.SubItems.Add(String.Format("{0:0,0  đ}", row["sum"]));
                        lstReport.Items.Add(im);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}