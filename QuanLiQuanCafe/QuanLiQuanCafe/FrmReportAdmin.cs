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
using QuanLiQuanCafe.Report;

namespace QuanLiQuanCafe
{
    public partial class FrmReportAdmin : DevExpress.XtraEditors.XtraForm
    {
        public FrmReportAdmin()
        {
            InitializeComponent();
        }

        private Double numMoney = 0;
        private void backstageViewTabItem1_SelectedChanged(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            try
            {
                //String sql = string.Format(@"SELECT dbo.Food.name, dbo.BillInfo.count, dbo.Food.unit, dbo.Food.price, dbo.Food.price*dbo.BillInfo.count AS[sum] FROM dbo.BillInfo INNER JOIN dbo.Food ON Food.idFood = BillInfo.idFood WHERE idBill = '{0}'", IDBill);
                DataTable dt = ReportDAO.Instance.LoadBaoCao();
                lstReport.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    numMoney = 0;
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
                        im.SubItems.Add(row["fullName"].ToString());
                        im.SubItems.Add(row["saleoff"].ToString());
                        im.SubItems.Add(row["vat"].ToString());
                        im.SubItems.Add(string.Format("{0:0,0  đ}", row["sum"]));
                        numMoney = numMoney + Convert.ToDouble(row["sum"].ToString());
                        lstReport.Items.Add(im);
                    }
                    txtSumDoanhThu.Text = string.Format("{0:0,0  đ}", numMoney);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lstReport_Click(object sender, EventArgs e)
        {
            if (lstReport.Items.Count > 0)
            {
                ShowInfoBillDetail(lstReport.Items[lstReport.FocusedItem.Index].SubItems[1].Text);
            }

        }

        private void ShowInfoBillDetail(String idBill)
        {
            try
            {
                //String sql = string.Format(@"SELECT dbo.Food.name, dbo.BillInfo.count, dbo.Food.unit, dbo.Food.price, dbo.Food.price*dbo.BillInfo.count AS[sum] FROM dbo.BillInfo INNER JOIN dbo.Food ON Food.idFood = BillInfo.idFood WHERE idBill = '{0}'", IDBill);
                DataTable dt = ReportDAO.Instance.LoadBaoCaoChiTietBillByIdBill(idBill);
                lsvReportDetail.Items.Clear();
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
                        im.SubItems.Add(row["idBillInfo"].ToString());
                        im.SubItems.Add(row["name"].ToString());
                        im.SubItems.Add(row["count"].ToString());
                        im.SubItems.Add(row["idBill"].ToString());

                        lsvReportDetail.Items.Add(im);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void chkStartDay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStartDay.Checked == true)
            {
                dtpStartDay.Enabled = true;
                dtpEndday.Enabled = true;
            }

            else
            {
                dtpStartDay.Enabled = false;
                dtpEndday.Enabled = false;
            }
        }

        

        private void btnFind_Click(object sender, EventArgs e)
        {
            String rule = GetData(true);
            if(rule != "")
            {
                if (chkTimchinhxac.Checked)
                {
                    String sql =
                        @"SELECT dbo.BillInfo.idBill, dbo.Employee.fullName, dbo.Bill.DateCheckOut, dbo.Bill.saleoff, dbo.Bill.vat, SUM(dbo.BillInfo.count*dbo.Food.price) [sum]"
                        + " FROM dbo.BillInfo"
                        + " INNER JOIN dbo.Food ON dbo.BillInfo.idFood = dbo.Food.idFood"
                        + " INNER JOIN dbo.Bill ON Bill.idBill = BillInfo.idBill"
                        + " INNER JOIN dbo.Employee ON Employee.idEmployee = Bill.idEmployee"
                        + " INNER JOIN dbo.TableFood ON TableFood.idTableFood = Bill.idTableFood"
                        + " WHERE " + rule
	                +" GROUP BY dbo.BillInfo.idBill, saleoff, vat, DateCheckOut, dbo.Employee.fullName";
                    try
                    {
                        //String sql = string.Format(@"SELECT dbo.Food.name, dbo.BillInfo.count, dbo.Food.unit, dbo.Food.price, dbo.Food.price*dbo.BillInfo.count AS[sum] FROM dbo.BillInfo INNER JOIN dbo.Food ON Food.idFood = BillInfo.idFood WHERE idBill = '{0}'", IDBill);
                        DataTable dt = DataProvider.Instance.LoadAllTable((sql));
                        lstReport.Items.Clear();
                        if (dt.Rows.Count > 0)
                        {
                            numMoney = 0;
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
                                im.SubItems.Add(row["fullName"].ToString());
                                im.SubItems.Add(row["saleoff"].ToString());
                                im.SubItems.Add(row["vat"].ToString());
                                im.SubItems.Add(String.Format("{0:0,0  đ}", row["sum"]));
                                numMoney = numMoney + Convert.ToDouble(row["sum"].ToString());
                                lstReport.Items.Add(im);
                            }
                            txtSumDoanhThu.Text = string.Format("{0:0,0  đ}", numMoney);
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    rule = GetData(false);
                    String sql =
                        @"SELECT dbo.BillInfo.idBill, dbo.Employee.fullName, dbo.Bill.DateCheckOut, dbo.Bill.saleoff, dbo.Bill.vat, SUM(dbo.BillInfo.count*dbo.Food.price) [sum]"
                        + " FROM dbo.BillInfo"
                        + " INNER JOIN dbo.Food ON dbo.BillInfo.idFood = dbo.Food.idFood"
                        + " INNER JOIN dbo.Bill ON Bill.idBill = BillInfo.idBill"
                        + " INNER JOIN dbo.Employee ON Employee.idEmployee = Bill.idEmployee"
                        + " INNER JOIN dbo.TableFood ON TableFood.idTableFood = Bill.idTableFood"
                        + " WHERE " + rule
                    + " GROUP BY dbo.BillInfo.idBill, saleoff, vat, DateCheckOut, dbo.Employee.fullName";
                    try
                    {
                        //String sql = string.Format(@"SELECT dbo.Food.name, dbo.BillInfo.count, dbo.Food.unit, dbo.Food.price, dbo.Food.price*dbo.BillInfo.count AS[sum] FROM dbo.BillInfo INNER JOIN dbo.Food ON Food.idFood = BillInfo.idFood WHERE idBill = '{0}'", IDBill);
                        DataTable dt = DataProvider.Instance.LoadAllTable((sql));
                        lstReport.Items.Clear();
                        if (dt.Rows.Count > 0)
                        {
                            numMoney = 0;
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
                                im.SubItems.Add(row["fullName"].ToString());
                                im.SubItems.Add(row["saleoff"].ToString());
                                im.SubItems.Add(row["vat"].ToString());
                                im.SubItems.Add(String.Format("{0:0,0  đ}", row["sum"]));
                                numMoney = numMoney + Convert.ToDouble(row["sum"].ToString());
                                lstReport.Items.Add(im);
                            }
                            txtSumDoanhThu.Text = string.Format("{0:0,0  đ}", numMoney);
                    
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            else
            {
                try
                {
                    //String sql = string.Format(@"SELECT dbo.Food.name, dbo.BillInfo.count, dbo.Food.unit, dbo.Food.price, dbo.Food.price*dbo.BillInfo.count AS[sum] FROM dbo.BillInfo INNER JOIN dbo.Food ON Food.idFood = BillInfo.idFood WHERE idBill = '{0}'", IDBill);
                    DataTable dt = ReportDAO.Instance.LoadBaoCao();
                    lstReport.Items.Clear();
                    if (dt.Rows.Count > 0)
                    {
                        numMoney = 0;
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
                            im.SubItems.Add(row["fullName"].ToString());
                            im.SubItems.Add(row["saleoff"].ToString());
                            im.SubItems.Add(row["vat"].ToString());
                            im.SubItems.Add(String.Format("{0:0,0  đ}", row["sum"]));
                            numMoney = numMoney + Convert.ToDouble(row["sum"].ToString());
                            lstReport.Items.Add(im);
                        }
                        txtSumDoanhThu.Text = string.Format("{0:0,0  đ}", numMoney);
                    
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            

        }


        public String GetData(bool chinhXac)
        {
            String relation = "";
            if (chinhXac)
            {
                relation = "and";
            }
            else
            {
                relation = "or";
            }
            String sql = "";
            int i = 0;
            if (dtpStartDay.Enabled)
            {
                DateTime startDay = dtpStartDay.Value;
                DateTime endDay = dtpEndday.Value;
                if ((endDay - startDay).Ticks > 0)
                {
                    sql = sql + string.Format("DateCheckOut BETWEEN '{0}' and '{1}'", startDay, endDay);
                    i++;
                }
            }
            if (txtIdBill.Text.Trim() != "")
            {
                if (i > 0)
                {
                    sql = sql + string.Format(" {1} BillInfo.idBill = '{0}'", txtIdBill.Text.Trim(), relation);
                    i++;
                }
                else
                    sql = sql + string.Format(" BillInfo.idBill = '{0}'", txtIdBill.Text.Trim());


            }

            if (txtEmploeeName.Text.Trim() != "")
            {
                if (i > 0)
                {
                    sql = sql + string.Format(" {1} fullName like N'{0}'", txtEmploeeName.Text.Trim(), relation);
                    i++;
                }
                else
                {
                    sql = sql + string.Format(" fullName like N'{0}'", txtEmploeeName.Text.Trim());

                }
            }

            if (txtTableFood.Text.Trim() != "")
            {
                if (i > 0)
                {
                    sql = sql + string.Format(" {1} TableFood.name like N'{0}'", txtTableFood.Text.Trim(), relation);
                    i++;
                }
                else
                {
                    sql = sql + string.Format(" TableFood.name like N'{0}'", txtTableFood.Text.Trim());
                }
            }
            return sql;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (lstReport.Items.Count > 0)
            {
                try
                {
                    //var listView1 = new ListView();
                    DataTable table = new DataTable();
                    foreach (ListViewItem item in lstReport.Items)
                    {
                        table.Columns.Add(item.ToString());
                        foreach (var it in item.SubItems)
                            table.Rows.Add(it.ToString());
                    }


                    FrmReportBaoCaoTK frm = new FrmReportBaoCaoTK(table);
                    frm.Show();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }
    }
}