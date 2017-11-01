using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DTO;
using DAO;

namespace QuanLiQuanCafe
{
    public partial class FrmAdmin : Form
    {
        public FrmAdmin()
        {
            InitializeComponent();
        }
        #region Event 
        /// <summary>
        /// Thêm mới nhân viên
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                String NameEmployee = txtNameEmployee.Text != "" ? txtNameEmployee.Text : "Chua Co Ten";
                bool SexEmployee = Convert.ToBoolean(cbbSexEmployee.SelectedIndex);
                String AddressEmployee = txtAddressEmployee.Text;
                String PhoneEmployee = txtPhoneEmployee.Text;
                DateTime StartWorkEmployee = Convert.ToDateTime(txtStartWorkEmployee.Text);
                Double SalaryEmployee = txtSalaryEmployee.Text != "" ? Convert.ToDouble(txtSalaryEmployee.Text) : 0;
                DateTime EndDaySalaryEmployee = Convert.ToDateTime(txtEndDaySalaryEmployee.Text);
                Employee emp = new Employee(1, NameEmployee, SexEmployee, AddressEmployee, PhoneEmployee, StartWorkEmployee, SalaryEmployee, EndDaySalaryEmployee, 0);
                if (EmployeeDAO.Instance.AddEmployee(emp) == true)
                {
                    dtgvEmployee.Refresh();
                    loadData();
                    MessageBox.Show("Thêm nhân viên thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Không thể thêm nhân viên, Vui lòng kiểm tra lại!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }

        }



         #endregion

        private void txtSalaryEmployee_KeyPress(object sender, KeyPressEventArgs e)
        {
            inputnumber(sender, e);
        
        }
        private void inputnumber(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }

                // only allow one decimal point
                if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
        }

        private void txtPhoneEmployee_KeyPress(object sender, KeyPressEventArgs e)
        {
            inputnumber(sender, e);
        }

        private void txtSalaryAdvanceEmployee_KeyPress(object sender, KeyPressEventArgs e)
        {
            inputnumber(sender, e);
        }

        private void FrmAdmin_Load(object sender, EventArgs e)
        {
            loadData();

        }
        /// <summary>
        /// Load danh sach thu gon nhan vien vao datagridview
        /// </summary>
        private void loadData()
        {
            cbbSexEmployee.SelectedIndex = 0;
            dtgvEmployee.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvEmployee.DataSource = EmployeeDAO.Instance.LoadEmployee();
        }

        /// <summary>
        /// Chon tung dong du lieu bang click chuot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtgvEmployee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bingdingData();
        }
        public void bingdingData()
        {
            Employee emp;
            int id = Convert.ToInt32(dtgvEmployee.Rows[this.dtgvEmployee.CurrentCell.RowIndex].Cells[0].Value.ToString());
            emp = EmployeeDAO.Instance.LoadEmployeeByID(id);
            txtIDEmployee.Text = emp.Id.ToString();
            txtNameEmployee.Text = emp.Name;
            if (emp.Sex == true)
            {
                cbbSexEmployee.SelectedIndex = 1;
            }
            else cbbSexEmployee.SelectedIndex = 0;
            txtAddressEmployee.Text = emp.Address;
            txtPhoneEmployee.Text = emp.Phone;
            txtStartWorkEmployee.Value = emp.StartWork;
            txtEndDaySalaryEmployee.Value = emp.EndDaySalary;
            txtSalaryEmployee.Text = emp.Salary.ToString();
            txtSalaryAdvanceEmployee.Text = emp.SalaryAdvance.ToString();
        }
        /// <summary>
        /// Tim kiem nhan vien
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchEmployee_Click(object sender, EventArgs e)
        {
            List<String> data = getSQLquery();
            if(data.Count <1)
            {
                loadData();
            }
            else
            {
                String huanit = "";
                foreach(String item in data)
                {
                    huanit = string.Format("{0}{1} -- ", huanit, item);
                }
                MessageBox.Show(huanit);
            }
        }

        private List<String> getSQLquery()
        {
            List<String> SQLquery = new List<string>();
            //String SQLquery = "";
            String ID;
            if(txtIDSearchEmployee.Text !="")
            {
                ID = string.Format(@"idEmployee = '{0}'", txtIDSearchEmployee.Text);
                SQLquery.Add(ID);
            }
            String Name;
            if (txtNameSearchEmployee.Text != "")
            {
                Name = string.Format(@"fullName = '{0}'", txtNameSearchEmployee.Text);
                SQLquery.Add(Name);
            }
            String Sex;
            if(cbbSexSearchEmployee.SelectedIndex >0)
            {
                if (cbbSexSearchEmployee.SelectedIndex == 1)
                    Sex = @"sex = '"+false+"'";
                else
                    Sex = @"sex = '"+true+"'";
                SQLquery.Add(Sex);
            }
            String Phone;
            if(txtPhoneSearchEmployee.Text !="")
            {
                Phone = string.Format(@"phone = '{0}'", txtPhoneSearchEmployee.Text);
                SQLquery.Add(Phone);
            }
            String salary;
            if(txtSalarySearchEmployee.Text !="")
            {
                salary = string.Format(@"salaryLevel = '{0}'", txtSalarySearchEmployee.Text);
                SQLquery.Add(salary);
            }
            String Address;
            if(txtAddressSearchEmployee.Text!="")
            {
                Address = string.Format(@"addres = '{0}'", txtAddressSearchEmployee.Text);
                SQLquery.Add(Address);
            }
            String StartWork;
            if(chkselect1.CheckState == CheckState.Checked)
            {
                StartWork = string.Format(@"dayStart = '{0}'", txtStartWorkSearchEmployee.Value);
                SQLquery.Add(StartWork);
            }
            String EnddaySalary;
            if(chkselect2.CheckState == CheckState.Checked)
            {
                EnddaySalary = string.Format(@"indicator = '{0}'", txtEnddaySearchEmployee.Value);
                SQLquery.Add(EnddaySalary);
            }
            return SQLquery;
        }

        private void chkselect1_CheckedChanged(object sender, EventArgs e)
        {
            if(chkselect1.CheckState == CheckState.Checked)
            {
                txtStartWorkSearchEmployee.Enabled = true;
            }
            else
            {
                txtStartWorkSearchEmployee.Enabled = false;
            }

        }

        private void txtPhoneSearchEmployee_KeyPress(object sender, KeyPressEventArgs e)
        {
            inputnumber(sender, e);
        }

        private void txtIDSearchEmployee_KeyPress(object sender, KeyPressEventArgs e)
        {
            inputnumber(sender, e);
        }

        private void txtSalarySearchEmployee_KeyPress(object sender, KeyPressEventArgs e)
        {
            inputnumber(sender, e);
        }

       

        

       

      


        #region method
        #endregion

    }
}
