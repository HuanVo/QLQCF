using DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
namespace QuanLiQuanCafe
{
    public partial class FrmAdminInFo : Form
    {
        private String UserName;
        public FrmAdminInFo(String UserName)
        {
            InitializeComponent();
            this.UserName = UserName;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmAdminInFo_Load(object sender, EventArgs e)
        {
            String sql = string.Format(@"Select * from Account where UserName ='{0}'", UserName);
            DataTable dt = DataProvider.Instance.LoadAllTable(sql);
            if(dt.Rows.Count>0)
            {
                foreach(DataRow row in dt.Rows)
                {
                    txtUserName.Text = row["UserName"].ToString();
                    txtDisplayName.Text = row["DisplayName"].ToString();
                    if(row["Type"].ToString() == "True")
                    {
                        txtTypeAccount.Text = "Quản trị cấp cao";
                    }
                    else
                        txtTypeAccount.Text = "Quản trị cơ bản";
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(txtOldPass.Text !="" && txtNewPass.Text.Trim() !="")
            {
               try
               {
                   String passold = Encryption.Instance.md5(txtOldPass.Text);
                   String passnew = Encryption.Instance.md5(txtNewPass.Text);
                   String sql = string.Format(@"Select count(*) from Account where UserName ='{0}' and PassWord = '{1}'", UserName, passold);
                   if (DataProvider.Instance.ExcuteScaler(sql) > 0)
                   {
                       String sqlUpdate = string.Format(@"UPDATE Account set PassWord = '{0}'", passnew);
                       if (DataProvider.Instance.ExcuteNonQuery(sqlUpdate) > 0)
                       {
                           MessageBox.Show("Cập nhật thành công mật khẩu");
                       }
                       else
                           MessageBox.Show("Không thể cập nhật mật khẩu, vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   }
                   else
                   {
                       MessageBox.Show("Mật khẩu cũ không chính xác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                   }
               }
                catch(Exception ex)
               {
                   MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
               }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập vào mật khẩu cũ và mới", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtNewPass.Text = "";
            txtOldPass.Text = "";
        }
    }
}
