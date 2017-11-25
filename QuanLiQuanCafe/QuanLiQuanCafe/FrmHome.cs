using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using DTO;
using DAO;
using System.Windows.Forms;

namespace QuanLiQuanCafe
{
    public partial class FrmHome : Form
    {
        public FrmHome()
        {
            InitializeComponent();
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
            //
            LoadComboboxs(cbbCatalogFood, @"Select idFoodCategory, name from FoodCategory", "name", "idFoodCategory");
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      

        private void quảnTrịToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmAdmin frmAdmin = new FrmAdmin();
            frmAdmin.ShowDialog();
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
                //String sqlgetIDBill = string.Format(@"select idBill from Bill where idTableFood = '{0}' and stats ='False'", TableFood);
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
        public void ShowBillInFor(String IDBill)
        {
            
            try
            {

                String sql = string.Format(@"SELECT dbo.Food.name, dbo.BillInfo.count, dbo.Food.unit, dbo.Food.price, dbo.Food.price*dbo.BillInfo.count AS[sum] FROM dbo.BillInfo INNER JOIN dbo.Food ON Food.idFood = BillInfo.idFood WHERE idBill = '{0}'", IDBill);
                DataTable dt = DataProvider.Instance.LoadAllTable(sql);
                lsvCTHD.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    int i = 0;
                    
                    foreach (DataRow row in dt.Rows)
                    {
                        i = i + 1;
                        ListViewItem im = new ListViewItem(i.ToString());
                        im.SubItems.Add(row["name"].ToString());
                        im.SubItems.Add(row["unit"].ToString());
                        im.SubItems.Add(row["count"].ToString());
                        im.SubItems.Add(String.Format("{0:0,0 vnđ}", row["price"]));
                        im.SubItems.Add(String.Format("{0:0,0 vnđ}", row["sum"]));
                        lsvCTHD.Items.Add(im);
                    }
                    
                }
            } 
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
    }
}
