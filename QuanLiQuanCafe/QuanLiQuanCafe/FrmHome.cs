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
        }

        private void ShowInfoTableFood(String IDTableFood)
        {
            String sql = string.Format(@"SELECT checkout= CASE when dbo.Bill.stats = 'false' THEN N'Chưa Thanh Toán' WHEN dbo.Bill.stats = 'True' THEN N'Đã Thanh Toán' END, dbo.Employee.fullName FROM dbo.Bill INNER JOIN dbo.Employee ON Employee.idEmployee = Bill.idEmployee INNER JOIN dbo.TableFood ON TableFood.idTableFood = Bill.idTableFood WHERE dbo.Bill.stats = 'false' and dbo.TableFood.idTableFood = '{0}'", IDTableFood);
            DataTable dt = DataProvider.Instance.LoadAllTable(sql);
            if(dt.Rows.Count>0)
            {
                foreach(DataRow row in dt.Rows)
                {
                   
                    //lblNameTableFood.Text = row["name"].ToString();
                    
                    //lblDetailStatusTableFood.Text = row["stats"].ToString();
                    
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

            LoadComboboxs(cbbCatalogFood, @"Select idFoodCategory, name from FoodCategory", "name", "idFoodCategory");


            for(int j =1; j<=20; j++)
            {
                 ListViewItem im = new ListViewItem(j.ToString());
                for (int i = 0; i <3; i++)
                {
                    im.SubItems.Add("Đây là "+j.ToString());
                }
                if (j % 2 == 0)
                {
                    im.BackColor = Color.Gray;
                }
                listView1.Items.Add(im);
                
            }         
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

        private void btnAddBillFood_Click(object sender, EventArgs e)
        {
            if(lblIDFoodTable.Text.Trim() !="")
            {

            }
        }

        
    }
}
