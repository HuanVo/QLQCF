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
                        btn.BackColor = Color.DarkBlue;
                        text = string.Format("{0}{1}Có Khách", text, System.Environment.NewLine);
                    }
                    else
                    {
                        btn.BackColor = Color.White;
                        text = string.Format("{0}{1}Bàn Trống", text, System.Environment.NewLine);
                    }
                    btn.Text = text;
                    btn.Click += btn_Click;
                    btn.Tag = item;
                    flowpnlListTable.Controls.Add(btn);
                }
            }
        }

        void btn_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Table tb = ((sender as Button).Tag as Table);
            MessageBox.Show(tb.Id.ToString());
        }

        private void ShowInfoTableFood(String IDTableFood)
        {

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
    }
}
