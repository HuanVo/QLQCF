using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAO;
using DTO;

namespace QuanLiQuanCafe
{
    public partial class frmMain : Form
    {
        private String UserName;
        public frmMain(String User)
        {
            UserName = User;
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Account acc = AccountDAO.Instance.getAccount(UserName);
            label1.Text = acc.DisplayName;

        }

        
    }
}
