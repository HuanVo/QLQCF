using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using DTO;
using DAO;
using System.Windows.Forms;
using QuanLiQuanCafe.Report;

namespace QuanLiQuanCafe
{
    public partial class FrmHome : Form
    {
        private String UserName;
        public FrmHome(String UserName)
        {
            InitializeComponent();
            this.UserName = UserName;
        }
        
        private void Assets()
        {
            lblDate.Text = DateTime.Now.ToShortDateString();
            lblTime.Text = DateTime.Now.ToShortTimeString();
        }
        /// <summary>
        /// Hiển thị danh sách bàn.
        /// </summary>
        private void LoadListTable()
        {
            List<Table> listTableFood = TableDAO.Instance.GetListTable();
            flowpnlListTable.Controls.Clear();
            if(listTableFood.Count > 0)
            {
                foreach(Table item in listTableFood)
                {
                    Button btn = new Button();
                    btn.Width = 97;
                    btn.Height = 70;
                    btn.Name = item.Id.ToString();
                    //btn.Text = item.Name;
                    String text = item.Name;
                    if (item.Status == true)
                    {
                        btn.BackColor = System.Drawing.Color.Green;
                        text = string.Format("{0}{1}Có Khách", text, System.Environment.NewLine);
                    }
                    else
                    {
                        btn.BackColor = Color.White;
                        text = string.Format("{0}{1}Bàn Trống", text, System.Environment.NewLine);
                    }
                    btn.Text = text;
                    btn.Click += btn_Click;
                    btn.MouseHover += btn_MouseHover;
                    btn.MouseLeave += btn_MouseLeave;
                    btn.Tag = item;
                    flowpnlListTable.Controls.Add(btn);
                }
            }
        }

        void btn_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Table tb = ((sender as Button).Tag as Table);
            if(tb.Status ==true)
            {
                btn.BackColor = System.Drawing.Color.Green;
            }
            else
                btn.BackColor = Color.White;
        }

        void btn_MouseHover(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.SlateGray;
        }

        void btn_Click(object sender, EventArgs e)
        {
            lblIDFoodTable.Text = "";
            lblNameFoodTable.Text = "";
            lblDetailStatusTableFood.Text = "";
            txtMoneyProduct.Text = "";
            txtMoneyCheckout.Text = "";
            //lblNameTableFood.Text = "";
            lblDetailEmployeeTableFood.Text = "";
            lblStatusBillTableFood.Text = "";
            Table tb = ((sender as Button).Tag as Table);
            //lblNameTableFood.Text = tb.Name; //cbbNameTableFood
            lblIDFoodTable.Text = tb.Id.ToString();
            lblNameFoodTable.Text = tb.Name;
            
            if (tb.Status)
                lblDetailStatusTableFood.Text = "Có Khách";
            else
                lblDetailStatusTableFood.Text = "Bàn Trống";
            ShowInfoTableFood(tb.Id.ToString());

            // Show hoa don neu co
            int idBills = BillDAO.Instance.getIDBills(tb.Id);
            ShowBillInFor(idBills.ToString());
            //idbillstatic = idBills.ToString();
        }

        private void ShowInfoTableFood(String IDTableFood)
        {

            String sql = string.Format(@"SELECT bans = CASE when dbo.TableFood.stats = 'false' THEN N'Bàn Trống' WHEN dbo.TableFood.stats = 'True' THEN N'Có Khách' END, checkout= CASE when dbo.Bill.stats = 'false' THEN N'Chưa Thanh Toán' WHEN dbo.Bill.stats = 'True' THEN N'Đã Thanh Toán' END, dbo.Employee.fullName FROM dbo.Bill INNER JOIN dbo.Employee ON Employee.idEmployee = Bill.idEmployee INNER JOIN dbo.TableFood ON TableFood.idTableFood = Bill.idTableFood WHERE dbo.Bill.stats = 'false' and dbo.TableFood.idTableFood = '{0}'", IDTableFood);
            DataTable dt = DataProvider.Instance.LoadAllTable(sql);
            if(dt.Rows.Count>0)
            {
                foreach(DataRow row in dt.Rows)
                {
                   
                    //lblNameTableFood.Text = row["name"].ToString();
                    
                    //lblDetailStatusTableFood.Text = row["stats"].ToString();
                    lblDetailStatusTableFood.Text = "";
                    lblDetailStatusTableFood.Text = row["bans"].ToString();
                    lblDetailEmployeeTableFood.Text = row["fullName"].ToString();
                    
                    lblStatusBillTableFood.Text = row["checkout"].ToString();
                }
            }
        }

        private void LoadComboboxs(ComboBox cbb, String sql, String ValueDisplay, String ValueMember)
        {
            try
            {
                DataTable dt = DataProvider.Instance.LoadAllTable(sql);
                cbb.DataSource = dt;
                cbb.ValueMember = ValueMember;
                cbb.DisplayMember = ValueDisplay;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmHome_Load(object sender, EventArgs e)
        {
            LoadListTable();
            Assets();
            //load bàn vào cbb
            LoadComboboxs(cbbTableSwitch, @"select idTableFood, name from TableFood", "name", "idTableFood");

            //
            LoadComboboxs(cbbCatalogFood, @"Select idFoodCategory, name from FoodCategory", "name", "idFoodCategory");
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void quảnTrịToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }

        private void cbbCatalogFood_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbbCatalogFood.SelectedIndex != -1)
            {
                String Id = cbbCatalogFood.SelectedValue.ToString();
                String sql = string.Format(@"Select idFood, name from Food where idFoodCategory = '{0}'", Id);
                LoadComboboxs(cbbFood, sql, "name", "idFood");
            }
        }

        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            ComboBox cbb = new ComboBox() { Left = 50, Top = 50, Width = 300 };
            String sql = @"select idEmployee, fullName from Employee";
            DataTable dt = DataProvider.Instance.LoadAllTable(sql);


            cbb.DataSource = dt;
            cbb.DisplayMember = "fullName";
            cbb.ValueMember = "idEmployee";
            cbb.DropDownStyle = ComboBoxStyle.DropDownList;
            //TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(cbb);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? cbb.SelectedValue.ToString() : "";
        }

        private void btnAddBillFood_Click(object sender, EventArgs e)
        {
            String TableFood = lblIDFoodTable.Text.Trim();
            if (TableFood != "")
            {
                String idFood = cbbFood.SelectedValue.ToString();
                int Count = Convert.ToInt32(numCountFood.Value) > 0 ? Convert.ToInt32(numCountFood.Value) : 1;
                int IDBill = BillDAO.Instance.getIDBills(Convert.ToInt32(TableFood));
                if (lblDetailStatusTableFood.Text.Trim() != @"Có Khách") // Bàn đang trống
                {
                   if(MessageBox.Show("Bàn đang trống, mở hóa đơn mới và thêm đồ uống?","Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                   {
                       // buoc 1
                       // 1 mo trang thai ban hien tai
                       String Employee = ShowDialog("Chọn nhân viên order bàn", "Hộp thoại");
                       if(Employee !="")
                       {
                           String sqlOpenTableFood = string.Format(@"update TableFood set stats = 'true' where idTableFood = '{0}'", TableFood);
                           if (TableDAO.Instance.UpdateTableFood(sqlOpenTableFood))
                           {
                               // 2 mo hoa don moi
                               if (BillDAO.Instance.CreateBill(TableFood, Employee))
                               {
                                   // 3 Mo Chi tiet cho hoa don va them mon

                                   IDBill = BillDAO.Instance.getIDBills(Convert.ToInt32(TableFood));
                                   //idbillstatic = IDBill.ToString();
                                   if (BillInfo.Instance.CreateBillInfo(IDBill.ToString(), idFood, Count.ToString()))
                                   {

                                       MessageBox.Show("Đã mở hóa đơn và thêm đồ uống cho bàn " + lblNameFoodTable.Text, "Xác nhận thành công");
                                       // Load lại thông tin
                                       ShowInfoTableFood(TableFood);
                                       //reset Ds bàn
                                       LoadListTable();
                                       //Load Chi tiet hoa dơn
                                       // SELECT dbo.Food.name, dbo.BillInfo.count, dbo.Food.price, dbo.Food.price*dbo.BillInfo.count AS[sum] FROM dbo.BillInfo INNER JOIN dbo.Food ON Food.idFood = BillInfo.idFood WHERE idBill = 
                                       ShowBillInFor(IDBill.ToString());

                                   }
                                   else
                                       MessageBox.Show("Đã mở bàn nhưng không thể thêm đồ uống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                               }
                               else
                                   MessageBox.Show("Đã mở bàn nhưng không thể tạo mới hóa đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                           }
                           else
                               MessageBox.Show("Không thể mở bàn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                       }
                       else
                           MessageBox.Show("Vui lòng chọn nhân viên order", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                   }
                }
                else // ban  da co khach ngoi
                {
                    //String sqlgetIDBill = string.Format(@"select idBill from Bill where idTableFood = '{0}' and stats ='False'", TableFood);
                    
                    // neu thuc phan da co thi cap nhat so uong moi con chua co thi them moi
                    String sqlgetIDBills = string.Format(@"select count(*) from BillInfo where idBill = '{0}' and idFood ='{1}'", IDBill, idFood);
                    if(DataProvider.Instance.ExcuteScaler(sqlgetIDBills)>0)
                    {
                        int dount = Convert.ToInt32(numCountFood.Value);
                        if(BillInfo.Instance.UpdateBillInfo( IDBill, Convert.ToInt32(idFood), dount))
                        {
                            MessageBox.Show("Thêm thành công vào bàn " + lblNameFoodTable.Text, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Load chi tiết hóa đơn
                            ShowBillInFor(IDBill.ToString());
                        }
                    }
                    else
                    {
                        if (BillInfo.Instance.CreateBillInfo(IDBill.ToString(), idFood, Count.ToString()))
                        {
                            ShowBillInFor(IDBill.ToString());
                            MessageBox.Show("Đã thêm đồ uống cho bàn " + lblNameFoodTable.Text, "Xác nhận thành công");
                        }
                        else
                            MessageBox.Show("Không thể thêm đồ uống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    
                }
            }
            else
                MessageBox.Show("Không có bàn nào được chọn");
        }

        private String IDBillCheckout = "";

        public void ShowBillInFor(String IDBill)
        {
            try
            {
                String sql = string.Format(@"SELECT dbo.Food.name, dbo.BillInfo.count, dbo.Food.unit, dbo.Food.price, dbo.Food.price*dbo.BillInfo.count AS[sum] FROM dbo.BillInfo INNER JOIN dbo.Food ON Food.idFood = BillInfo.idFood WHERE idBill = '{0}'", IDBill);
                DataTable dt = DataProvider.Instance.LoadAllTable(sql);
                lsvCTHD.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    Double MoneyProduct = 0;
                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        i = i + 1;
                        
                        ListViewItem im = new ListViewItem(i.ToString());
                        if(i%2 == 0)
                        {
                            im.BackColor = Color.FromArgb(255, 244, 202);
                        }
                        im.SubItems.Add(row["name"].ToString());
                        im.SubItems.Add(row["unit"].ToString());
                        im.SubItems.Add(row["count"].ToString());
                        im.SubItems.Add(String.Format("{0:0,0  }", row["price"]));
                        im.SubItems.Add(String.Format("{0:0,0  }", row["sum"]));
                        MoneyProduct = MoneyProduct + Convert.ToDouble(row["sum"].ToString());
                        lsvCTHD.Items.Add(im);
                    }
                    txtMoneyProduct.Text = string.Format("{0:0,0  }", MoneyProduct);
                    IDBillCheckout = IDBill;
                }
                //ShowCheckOut(IDBill);
            } 
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
       
        public void TextChange()
        {
            if (txtMoneyProduct.Text != "")
            {
                Double MoneyProduct = Convert.ToDouble(txtMoneyProduct.Text);

                int SaleOff = Convert.ToInt32(numSaleOff.Value);
                int Thue = Convert.ToInt32(numThue.Value);
                // Double MoneyCheckOut = 0;
                Double TienThue = 0;
                Double TienSaleOff = 0;
                if (SaleOff > 0)
                {
                    TienSaleOff = (MoneyProduct * SaleOff) / 100;
                }
                if (Thue > 0)
                {
                    TienThue = MoneyProduct * Thue / 100;
                    txtMoneyCheckout.Text = string.Format("{0:0,0  }", MoneyProduct - TienSaleOff + TienThue);
                }
                else
                {
                    txtMoneyCheckout.Text = string.Format("{0:0,0  }", MoneyProduct - TienSaleOff);
                }
            }
        }

        private void txtMoneyProduct_TextChanged(object sender, EventArgs e)
        {
            TextChange();
                //MessageBox.Show("f");

        }

        private void numSaleOff_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                TextChange();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void numThue_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                TextChange();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
           try
           {
                // Kiểm tra Hóa đơn đã được thanh toán hay chưa.
            if (lblDetailStatusTableFood.Text.Trim() == @"Có Khách")
            {
                if (MessageBox.Show("Xác nhận thanh toán", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (BillDAO.Instance.CheckThanhToan(IDBillCheckout))
                    {
                        // nếu chưa thì thực hiện thanh toán, đóng hóa đơn, đóng bàn
                        // 1 Thanh Toán.
                        CheckOutBill(IDBillCheckout);
                        // 2. Đóng Hóa Đơn. // 3. Đóng bàn.
                        String sqlCloseTable = string.Format(@"update TableFood set stats ='False' where idTableFood ='{0}'", lblIDFoodTable.Text);
                        if (BillDAO.Instance.CloseBill(IDBillCheckout) == true && TableDAO.Instance.UpdateTableFood(sqlCloseTable) == true)
                        {
                            LoadListTable();
                            MessageBox.Show("Thanh toán thành công!");
                        }
                        else
                            MessageBox.Show("Thanh toán không thành công", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Bàn trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           }
            catch(Exception ex)
           {
               MessageBox.Show(ex.Message);
           }
        }

        private void CheckOutBill(String idbill)
        {
            String SaleOff = numSaleOff.Value.ToString();
            String VAT = numThue.Value.ToString();
            FrmReport frmreport = new FrmReport(idbill, SaleOff, VAT);
            frmreport.Show();
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            // 1 Kiểm tra bàn chuyển có đang mở hay không.
            try
            {
                String TableFoodSwitch = cbbTableSwitch.SelectedValue.ToString();
                String TableFood = lblIDFoodTable.Text.Trim();
                if (TableFood != "" && TableFood != TableFoodSwitch)
                {
                    int IDBill = BillDAO.Instance.getIDBills(Convert.ToInt32(TableFood));
                    int IDBillMoi = BillDAO.Instance.getIDBills(Convert.ToInt32(TableFoodSwitch));
                    if (lblDetailStatusTableFood.Text.Trim() != @"Có Khách") // Bàn đang trống
                    {
                        MessageBox.Show("Bàn không có gì để chuyển");
                    }
                    else
                    {
                        if (MessageBox.Show(string.Format("Xác nhận chuyển từ bàn {0} đến bàn {1}", lblNameFoodTable.Text, cbbTableSwitch.GetItemText(this.cbbTableSwitch.SelectedItem)) + "?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            // 2 Kiểm tra bàn được chuyển có khách hay không
                            if (TableDAO.Instance.CheckTableIsEmpty(TableFoodSwitch)) // bàn đang trống
                            {
                                // 3 nếu không có, thực hiện mở bàn và gộp bàn - đóng bàn cũ.
                                // chuyển ID table của bill sang id table bàn muốn chuyển;
                                TableDAO.Instance.CloseTable(TableFood, false);
                                TableDAO.Instance.CloseTable(TableFoodSwitch, true);
                                String sqlChuyenban = string.Format(@"update Bill set idTableFood ='{0}' where idBill = '{1}'", TableFoodSwitch, IDBill);
                                if (BillDAO.Instance.UpdateBill(sqlChuyenban))
                                {
                                    LoadListTable();
                                    MessageBox.Show(string.Format("Chuyển thành công từ bàn {0} đến bàn {1}", lblNameFoodTable.Text, cbbTableSwitch.GetItemText(this.cbbTableSwitch.SelectedItem)));
                                }
                                else
                                {
                                    MessageBox.Show("Không thể chuyển bàn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else // bàn có người
                            {
                                // 4 nếu có. thực hiện gộp bàn - đóng bàn cũ
                                // 1 chuyển toàn bộ đồ uống qua idbill mới
                                // 2 Delete bill cũ.
                                // 3 Đưa bàn về trạng thái trống
                                String sqlChuyenBan = string.Format(@"Update BillInfo set idBill = '{0}' where idBill ='{1}'", IDBillMoi, IDBill);
                                String sqlDelBillOld = string.Format(@"delete from Bill where idBill ='{0}' and stats = 'False'", IDBill);
                                if (DataProvider.Instance.ExcuteNonQuery(sqlChuyenBan) > 0 && DataProvider.Instance.ExcuteNonQuery(sqlDelBillOld) > 0)
                                {
                                    TableDAO.Instance.CloseTable(TableFood, false);
                                    LoadListTable();
                                    MessageBox.Show(string.Format("Chuyển thành công từ bàn {0} đến bàn {1}", lblNameFoodTable.Text, cbbTableSwitch.GetItemText(this.cbbTableSwitch.SelectedItem)));
                                }
                                else
                                {
                                    MessageBox.Show("Không thể chuyển bàn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnResetCheckout_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra Hóa đơn đã được thanh toán hay chưa.
                if (lblDetailStatusTableFood.Text.Trim() == @"Có Khách")
                {
                    
                        if (BillDAO.Instance.CheckThanhToan(IDBillCheckout))
                        {
                            CheckOutBill(IDBillCheckout);
                        }
                }
                else
                {
                    MessageBox.Show("Bàn trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelCheckout_Click(object sender, EventArgs e)
        {
            try
            {
                String TableFood = lblIDFoodTable.Text.Trim();
                int IDBill = BillDAO.Instance.getIDBills(Convert.ToInt32(TableFood));
                if (IDBill > 0)
                {
                   if(MessageBox.Show("Xóa đơn hàng?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                   {
                       if (BillInfo.Instance.DeleteBillInfo(IDBill.ToString()))
                       {
                           // delete bill from Bill table
                           String sqlDelBill = string.Format(@"delete from Bill where idBill = '{0}'", IDBill);
                           if (BillDAO.Instance.UpdateBill(sqlDelBill))
                           {
                               // Clear table with IDbill
                               if (TableDAO.Instance.CloseTable(TableFood, false))
                               {
                                   LoadListTable();
                                   ShowBillInFor(IDBill.ToString());
                                   txtMoneyProduct.Text = "";
                                   txtMoneyCheckout.Text = "";
                                   MessageBox.Show("Xóa thành công!");

                               }
                           }
                       }
                   }
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            String TableName = txtTableNameSearch.Text.Trim();
            int Status = cbbStatusTableSearch.SelectedIndex; // 1 có khách, 2 trống
            String sqlsss = "";
            if(TableName !="")
            {
                sqlsss = string.Format(@"name ='{0}'", TableName);
            }
            String sts = "";
            if(Status>0)
            {
                if(Status == 1)
                {
                    sts = @"stats = '" + true + "' ORDER BY stats";
                    //sqlsss = sqlsss + sts;
                }
                else
                {
                    sts = @"stats = '" + false + "' ORDER BY stats";
                    //sqlsss = sqlsss + sts;
                }
                if(sqlsss !="")
                {
                    sqlsss = string.Format("{0} and {1}", sqlsss, sts);
                }
                else
                {
                    sqlsss = sts;
                }
                
            }
            if(sqlsss !="")
            {
                LoadListTableSearch(sqlsss);
            }
            else
            {
                LoadListTable();
            }

        }

        private void LoadListTableSearch(String sql)
        {
            List<Table> listTableFood = TableDAO.Instance.GetListTableBysql(sql);
            flowpnlListTable.Controls.Clear();
            if (listTableFood.Count > 0)
            {
                foreach (Table item in listTableFood)
                {
                    Button btn = new Button();
                    btn.Width = 97;
                    btn.Height = 70;
                    btn.Name = item.Id.ToString();
                    //btn.Text = item.Name;
                    String text = item.Name;
                    if (item.Status == true)
                    {
                        btn.BackColor = System.Drawing.Color.Green;
                        text = string.Format("{0}{1}Có Khách", text, System.Environment.NewLine);
                    }
                    else
                    {
                        btn.BackColor = Color.White;
                        text = string.Format("{0}{1}Bàn Trống", text, System.Environment.NewLine);
                    }
                    btn.Text = text;
                    btn.Click += btn_Click;
                    btn.MouseHover += btn_MouseHover;
                    btn.MouseLeave += btn_MouseLeave;
                    btn.Tag = item;
                    flowpnlListTable.Controls.Add(btn);
                }
            }
        }

        private void cậpNhậtDanhMụcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAdmin frmAdmin = new FrmAdmin();
            frmAdmin.ShowDialog();
        }

        private void báoCáoThốngKêToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmReportAdmin frm = new FrmReportAdmin();
            frm.ShowDialog();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void thôngTinToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void đăngXuấtChươngTrìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinVàCậpNhậtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAdminInFo frm = new FrmAdminInFo(UserName);
            frm.ShowDialog();
        }
    }
}
