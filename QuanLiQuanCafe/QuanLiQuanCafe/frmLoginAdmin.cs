using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DAO;

namespace QuanLiQuanCafe
{
    public partial class frmLoginAdmin : Form
    {
        public frmLoginAdmin()
        {
            InitializeComponent();
        }
        private void frmLoginAdmin_Load(object sender, EventArgs e)
        {
            //Lay thong tin dang nhap load len form
            String UserName = ConfigurationManager.AppSettings["UserName"];
            
            if(UserName.Equals("") == false)
            {
                String UserPass = ConfigurationManager.AppSettings["UserPass"];
                String CheckSave = ConfigurationManager.AppSettings["CheckSave"];
                txtUserName.Text = UserName;
                txtPassword.Text = UserPass;
                ckeSaveAccount.Checked = true;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Liên hệ: 0978521398 để biết thêm thông tin", "Trợ Giúp");
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            String UserName = txtUserName.Text;
            String Password =Encryption.Instance.md5(txtPassword.Text);
            //Lưu tài khoản đăng nhập
            if (ckeSaveAccount.Checked == true)
            {
                saveAcccount(UserName, Password, "True");
            }
            else
            {
                saveAcccount("", "", "False");
            }

            //Xử lí đăng nhập
            if(AccountDAO.Instance.CheckLogin(UserName, Password))
            {
                frmMain frmmain = new frmMain(UserName);
                this.Hide();
                frmmain.ShowDialog();
                this.Show();
               // MessageBox.Show("Đăng nhập thành công!");
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng." + Environment.NewLine + "Vui lòng kiểm tra lại!", "Đăng nhập không thành công.");
            }
            
        }
        /// <summary>
        /// Lưu tài khoản vào file configApp
        /// </summary>
        private void saveAcccount(String UserName, String Password, String status)
        {
            try
            {
                Encryption.Instance.EditAppSetting("UserName", UserName);
                Encryption.Instance.EditAppSetting("UserPass", Password);
                Encryption.Instance.EditAppSetting("CheckSave", status);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        /// <summary>
        /// Thoát chương trình
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            this.Close();
        }

        private void frmLoginAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Bạn có thoát chương trình?","Thông báo thoát",MessageBoxButtons.OKCancel, MessageBoxIcon.Question)!=DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

    }
}
