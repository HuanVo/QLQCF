using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DTO;
using DAO;
using System.Data;

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
                String NameEmployee = txtNameEmployee.Text.Trim() != "" ? txtNameEmployee.Text.Trim() : "Chua Co Ten";
                bool SexEmployee = Convert.ToBoolean(cbbSexEmployee.SelectedIndex);
                String AddressEmployee = txtAddressEmployee.Text.Trim();
                String PhoneEmployee = txtPhoneEmployee.Text.Trim();
                DateTime StartWorkEmployee = Convert.ToDateTime(txtStartWorkEmployee.Text.Trim());
                Double SalaryEmployee = txtSalaryEmployee.Text.Trim() != "" ? Convert.ToDouble(txtSalaryEmployee.Text.Trim()) : 0;
                DateTime EndDaySalaryEmployee = Convert.ToDateTime(txtEndDaySalaryEmployee.Text.Trim());
                Employee emp = new Employee(1, NameEmployee, SexEmployee, AddressEmployee, PhoneEmployee, StartWorkEmployee, SalaryEmployee, EndDaySalaryEmployee, 0);
                if (MessageBox.Show("Bạn có muốn thêm mới " + NameEmployee + "?", "Xác nhận thêm mới nhân viên", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (EmployeeDAO.Instance.AddEmployee(emp) == true)
                    {
                        loadData(EmployeeDAO.Instance.LoadEmployee(), dtgvEmployee);
                        MessageBox.Show("Thêm nhân viên thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        MessageBox.Show("Không thể thêm nhân viên, Vui lòng kiểm tra lại!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
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

            //Employee
            cbbSexEmployee.SelectedIndex = 0;
            loadData(EmployeeDAO.Instance.LoadEmployee(), dtgvEmployee);

            //Product
            String sql = @"select * from FoodCategory";
            getAllTabletoCombox(cbbSearchProduct, sql, "name", "idFoodCategory");
            getAllTabletoCombox(cbbIDCatalogProduct, sql, "name", "idFoodCategory");
            loadData(ProductDAO.Instance.LoadProduct(), dtgvProduct);
            //Catalog
            loadData(CatalogProductDAO.Instance.LoadCatalog(), dtgrvCatalogProduct);
        }

        /// <summary>
        /// Load danh sach thu gon nhan vien vao datagridview
        /// </summary>
        private void loadData(DataTable dt, DataGridView dtgrv)
        {
            dtgrv.Controls.Clear();
            dtgrv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgrv.DataSource = dt;
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

        /// <summary>
        /// Bingding dữ liệu từ datagridview sang vùng nội dung thông tin
        /// </summary>
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
            if (chkSearchEmployee.CheckState == CheckState.Checked)
                SearchEmployeeExactly();
            else
                SearchEmployee();
        }

        private void SearchEmployeeExactly()
        {
            try
            {
                List<String> data = getSQLquery();
                if (data.Count < 1)
                {
                    loadData(EmployeeDAO.Instance.LoadEmployee(), dtgvEmployee);
                }
                else
                {
                    String sQLQuery = @"SELECT idEmployee AS [Mã Nhân Viên], fullName AS [Họ Và Tên], [Giới Tính]= CASE sex when 'true' then N'Nam' when 'false' then N'Nữ' END, addres AS [Địa Chỉ], phone AS [Số Điện Thoại] FROM Employee WHERE";
                    String tempwhere = "";
                    foreach (String item in data)
                    {
                        tempwhere = string.Format("{0} and {1}", tempwhere, item);
                    }
                    String Str1 = tempwhere.Substring(4);
                    sQLQuery = sQLQuery + Str1;
                    DataTable dt = EmployeeDAO.Instance.SearchEmployeeExactly(sQLQuery);
                    if (dt.Rows.Count > 0)
                    {
                        loadData(dt, dtgvEmployee);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhân viên.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
        }
        
        private void SearchEmployee()
        {
            try
            {
                List<String> data = getSQLquery1();
                if (data.Count < 1)
                {
                    loadData(EmployeeDAO.Instance.LoadEmployee(), dtgvEmployee);
                }
                else
                {
                    String sQLQuery = @"SELECT idEmployee AS [Mã Nhân Viên], fullName AS [Họ Và Tên], [Giới Tính]= CASE sex when 'true' then N'Nam' when 'false' then N'Nữ' END, addres AS [Địa Chỉ], phone AS [Số Điện Thoại] FROM Employee WHERE";
                    String tempwhere = "";
                    foreach (String item in data)
                    {
                        tempwhere = string.Format("{0} or {1}", tempwhere, item);
                    }
                    String Str1 = tempwhere.Substring(3);
                    sQLQuery = sQLQuery + Str1;
                    DataTable dt = EmployeeDAO.Instance.SearchEmployeeExactly(sQLQuery);
                    if (dt.Rows.Count > 0)
                    {
                        loadData(dt, dtgvEmployee);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhân viên.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
        }

        /// <summary>
        /// Lấy dữ liệu tìm kiếm nhân viên chính xác
        /// </summary>
        /// <returns></returns>
        private List<String> getSQLquery()
        {
            List<String> SQLquery = new List<string>();
            //String SQLquery = "";
            String ID;
            if (txtIDSearchEmployee.Text.Trim() != "")
            {
                ID = string.Format(@"idEmployee = '{0}'", txtIDSearchEmployee.Text);
                SQLquery.Add(ID);
            }
            String Name;
            if (txtNameSearchEmployee.Text.Trim() != "")
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
            if (txtPhoneSearchEmployee.Text.Trim() != "")
            {
                Phone = string.Format(@"phone = '{0}'", txtPhoneSearchEmployee.Text);
                SQLquery.Add(Phone);
            }
            String salary;
            if (txtSalarySearchEmployee.Text.Trim() != "")
            {
                salary = string.Format(@"salaryLevel = '{0}'", txtSalarySearchEmployee.Text);
                SQLquery.Add(salary);
            }
            String Address;
            if (txtAddressSearchEmployee.Text.Trim() != "")
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

        /// <summary>
        /// Lấy dữ liệu tìm kiếm nhân viên tương đối
        /// </summary>
        /// <returns></returns>
        private List<String> getSQLquery1()
        {
            List<String> SQLquery = new List<string>();
            //String SQLquery = "";
            String ID;
            if (txtIDSearchEmployee.Text.Trim() != "")
            {
                ID = string.Format(@"idEmployee like '%{0}%'", txtIDSearchEmployee.Text);
                SQLquery.Add(ID);
            }
            String Name;
            if (txtNameSearchEmployee.Text.Trim() != "")
            {
                Name = string.Format(@"fullName like '%{0}%'", txtNameSearchEmployee.Text);
                SQLquery.Add(Name);
            }
            String Sex;
            if (cbbSexSearchEmployee.SelectedIndex > 0)
            {
                if (cbbSexSearchEmployee.SelectedIndex == 1)
                    Sex = @"sex = '" + false + "'";
                else
                    Sex = @"sex = '" + true + "'";
                SQLquery.Add(Sex);
            }
            String Phone;
            if (txtPhoneSearchEmployee.Text.Trim() != "")
            {
                Phone = string.Format(@"phone like '%{0}%'", txtPhoneSearchEmployee.Text);
                SQLquery.Add(Phone);
            }
            String salary;
            if (txtSalarySearchEmployee.Text.Trim() != "")
            {
                salary = string.Format(@"salaryLevel = '{0}'", txtSalarySearchEmployee.Text);
                SQLquery.Add(salary);
            }
            String Address;
            if (txtAddressSearchEmployee.Text.Trim() != "")
            {
                Address = string.Format(@"addres like '%{0}%'", txtAddressSearchEmployee.Text);
                SQLquery.Add(Address);
            }
            String StartWork;
            if (chkselect1.CheckState == CheckState.Checked)
            {
                StartWork = string.Format(@"dayStart = '{0}'", txtStartWorkSearchEmployee.Value);
                SQLquery.Add(StartWork);
            }
            String EnddaySalary;
            if (chkselect2.CheckState == CheckState.Checked)
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

        private void btnResetTextSearchEmployee_Click(object sender, EventArgs e)
        {
            txtIDSearchEmployee.Text = "";
            txtNameSearchEmployee.Text = "";
            txtPhoneSearchEmployee.Text = "";
            txtAddressSearchEmployee.Text = "";
            cbbSexSearchEmployee.SelectedIndex = 0;
            txtSalarySearchEmployee.Text = "";
            chkselect1.Checked = false;
            chkselect2.Checked = false;
        }

        private void chkselect2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkselect2.CheckState == CheckState.Checked)
            {
                txtEnddaySearchEmployee.Enabled = true;
            }
            else
            {
                txtEnddaySearchEmployee.Enabled = false;
            }
        }

        /// <summary>
        /// Sửa một nhân viên
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditEmployee_Click(object sender, EventArgs e)
        {
            String IDEmployee = txtIDEmployee.Text.Trim();
            if(EmployeeDAO.Instance.CheckIsEmployee(IDEmployee))
            {
                try
                {
                    String NameEmployee = txtNameEmployee.Text.Trim() != "" ? txtNameEmployee.Text.Trim() : "Chua Co Ten";
                    bool SexEmployee = Convert.ToBoolean(cbbSexEmployee.SelectedIndex);
                    String AddressEmployee = txtAddressEmployee.Text.Trim();
                    String PhoneEmployee = txtPhoneEmployee.Text.Trim();
                    DateTime StartWorkEmployee = Convert.ToDateTime(txtStartWorkEmployee.Text.Trim());
                    Double SalaryEmployee = txtSalaryEmployee.Text.Trim() != "" ? Convert.ToDouble(txtSalaryEmployee.Text.Trim()) : 0;
                    DateTime EndDaySalaryEmployee = Convert.ToDateTime(txtEndDaySalaryEmployee.Text.Trim());
                    Double SalaryAdvanceEmployee = txtSalaryAdvanceEmployee.Text.Trim() != "" ? Convert.ToDouble(txtSalaryAdvanceEmployee.Text.Trim()) : 0;
                    Employee emp = new Employee(Convert.ToInt32(IDEmployee), NameEmployee, SexEmployee, AddressEmployee, PhoneEmployee, StartWorkEmployee, SalaryEmployee, EndDaySalaryEmployee, SalaryAdvanceEmployee);
                    if (MessageBox.Show("Bạn có muốn sửa " + IDEmployee, "Cảnh báo sửa", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (EmployeeDAO.Instance.EditEmployee(emp) == true)
                        {
                            loadData(EmployeeDAO.Instance.LoadEmployee(), dtgvEmployee);
                            MessageBox.Show(string.Format("Sửa nhân viên {0} thành công!", IDEmployee), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show(string.Format("Không thể sửa nhân viên {0} ,Vui lòng kiểm tra lại!", IDEmployee), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteLog(ex.Message);
                }
            }
            else
            {
                MessageBox.Show(string.Format("Nhân Viên có mã {0} Không tồn tại", IDEmployee), "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        /// <summary>
        /// Xóa một nhân viên
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelEmployee_Click(object sender, EventArgs e)
        {
            String IDEmployee = txtIDEmployee.Text.Trim();
            if (EmployeeDAO.Instance.CheckIsEmployee(IDEmployee))
            {
                DialogResult mes =  MessageBox.Show("Bạn có muốn xóa " + IDEmployee, "Cảnh báo xóa", MessageBoxButtons.YesNo);
                if (mes == System.Windows.Forms.DialogResult.Yes)
                {
                    if (EmployeeDAO.Instance.DeleteEmployee(IDEmployee))
                    {
                        loadData(EmployeeDAO.Instance.LoadEmployee(), dtgvEmployee);
                        MessageBox.Show("Xóa thành công nhân viên " + IDEmployee, "Thông Báo");
                    } 
                    else
                        MessageBox.Show("Không thể xóa nhâ viên " + IDEmployee, "Thông Báo");
                }
            }
            else
            {
                MessageBox.Show(string.Format("Nhân Viên có mã {0} Không tồn tại", IDEmployee), "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Them moi san pham
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            //int ID = Convert.ToInt32(txtIDProduct.Text.Trim());
            String Name = txtNameProduct.Text.Trim() != "" ? txtNameProduct.Text.Trim() : "Chưa có tên";
            String Unit = txtUnitProduct.Text.Trim() != "" ? txtUnitProduct.Text.Trim() : "Chưa có đơn vị";
            Double Price = txtPriceProduct.Text.Trim() != "" ? Convert.ToDouble(txtPriceProduct.Text.Trim()) : 0;
            int CatalogID = Convert.ToInt32(cbbIDCatalogProduct.SelectedValue);
            
            Product pro = new Product(1, Name, Unit, CatalogID, Price);
            if (MessageBox.Show(string.Format("Bạn có muốn thêm {0} ?", Name), "Thông Báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (ProductDAO.Instance.AddProduct(pro))
                {
                    loadData(ProductDAO.Instance.LoadProduct(), dtgvProduct);
                    MessageBox.Show("Thêm thành công sản phẩm", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Không thể thêm sản phẩm, vui lòng kiểm tra lại.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void txtPriceProduct_KeyPress(object sender, KeyPressEventArgs e)
        {
            inputnumber(sender, e);
        }

        private void textEdit28_KeyPress(object sender, KeyPressEventArgs e)
        {
            inputnumber(sender, e);
        }

        public void getAllTabletoCombox(ComboBox cbb, String sql, String DisplayMember, String ValueMember)
        {
            try
            {
                DataTable dt = DataProvider.Instance.LoadAllTable(sql);
                cbb.DataSource = dt;
                cbb.DisplayMember = DisplayMember;
                cbb.ValueMember = ValueMember;
            }
            catch(Exception mes)
            {
                ErrorLog.WriteLog(mes.Message);
            }
        }

        private void dtgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BingdingProduct();
        }

        public void BingdingProduct()
        {
            Product pro;
            int id = Convert.ToInt32(dtgvProduct.Rows[this.dtgvProduct.CurrentCell.RowIndex].Cells[0].Value.ToString());
            pro = ProductDAO.Instance.LoadProductByID(id);
            txtIDProduct.Text = pro.Id.ToString();
            txtNameProduct.Text = pro.Name;
            txtPriceProduct.Text = pro.Price.ToString();
            txtUnitProduct.Text = pro.Unit;
            cbbIDCatalogProduct.SelectedValue = pro.Idcatalog;
            cbbSearchProduct.SelectedValue = pro.Idcatalog;
        }

        private void btnEditProduct_Click(object sender, EventArgs e)
        {
            if(txtIDProduct.Text !="")
            {
                int ID = Convert.ToInt32(txtIDProduct.Text);
                String Name = txtNameProduct.Text.Trim() != "" ? txtNameProduct.Text.Trim() : "Chưa có tên";
                Double Price = Convert.ToDouble(txtPriceProduct.Text.Trim() != "" ? txtPriceProduct.Text.Trim() : "0");
                String Unit = txtUnitProduct.Text;
                int IDCatalog = Convert.ToInt32(cbbIDCatalogProduct.SelectedValue);
                Product pro = new Product(ID, Name, Unit, IDCatalog, Price);
                if(MessageBox.Show("Bạn muốn sửa sản phẩm có mã "+ ID,"Cảnh Báo Sửa", MessageBoxButtons.YesNo)== DialogResult.Yes)
                {
                    if (ProductDAO.Instance.EditProduct(pro))
                    {
                        loadData(ProductDAO.Instance.LoadProduct(), dtgvProduct);
                        MessageBox.Show("Thay đổi thảnh công sản phẩm có mã " + ID, "Thông báo");
                    }
                    else
                        MessageBox.Show("Không thể thay đổi sản phẩm có mã " + ID, "Lỗi Sửa sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Không có sản phẩm nào được chọn.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelProduct_Click(object sender, EventArgs e)
        {
            if (txtIDProduct.Text != "")
            {
                int ID = Convert.ToInt32(txtIDProduct.Text);
                if (MessageBox.Show("Bạn có muốn xóa sản phẩm có mã " + ID, "Cảnh Báo Xóa", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (ProductDAO.Instance.DeleteProduct(ID))
                    {
                        loadData(ProductDAO.Instance.LoadProduct(), dtgvProduct);
                        MessageBox.Show("Đã Xóa " + ID, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        MessageBox.Show("Không thể xóa " + ID, "Lỗi Xóa Sản Phẩm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Không có sản phẩm nào được chọn.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnResetProduct_Click(object sender, EventArgs e)
        {
            txtIDProduct.Text = "";
            txtNameProduct.Text = "";
            txtPriceProduct.Text = "";
            txtUnitProduct.Text = "";
        }

        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            if (chkSearchExactlyProduct.CheckState == CheckState.Checked)
                SearchProductEx();
            else
                SearchProduct();
        }

        private void SearchProductEx()
        {
            try
            {
                List<String> data = DataSearchProductEx();
                if (data.Count < 1)
                {
                    loadData(ProductDAO.Instance.LoadProduct(), dtgvProduct);
                }
                else
                {
                    String sQLQuery = @"SELECT idFood AS [Mã Sản Phẩm], name AS [Tên Phẩn Phẩm], unit AS [Đơn Vị], price AS [Giá] FROM Food WHERE";
                    String tempwhere = "";
                    foreach (String item in data)
                    {
                        tempwhere = string.Format("{0} and {1}", tempwhere, item);
                    }
                    String Str1 = tempwhere.Substring(4);
                    sQLQuery = sQLQuery + Str1;
                    DataTable dt = ProductDAO.Instance.SearchProductExactly(sQLQuery);
                    if (dt.Rows.Count > 0)
                    {
                        loadData(dt, dtgvProduct);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
        }
        private void SearchProduct()
        {
            try
            {
                List<String> data = DataSearchProduct();
                if (data.Count < 1)
                {
                    loadData(ProductDAO.Instance.LoadProduct(), dtgvProduct);
                }
                else
                {
                    String sQLQuery = @"SELECT idFood AS [Mã Sản Phẩm], name AS [Tên Phẩn Phẩm], unit AS [Đơn Vị], price AS [Giá] FROM Food WHERE";
                    String tempwhere = "";
                    foreach (String item in data)
                    {
                        tempwhere = string.Format("{0} or {1}", tempwhere, item);
                    }
                    String Str1 = tempwhere.Substring(3);
                    sQLQuery = sQLQuery + Str1;
                    DataTable dt = ProductDAO.Instance.SearchProductExactly(sQLQuery);
                    if (dt.Rows.Count > 0)
                    {
                        loadData(dt, dtgvProduct);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
        }

        private List<String> DataSearchProductEx()
        {
            List<String> result = new List<string>();
            String ID = "";
            if(txtSearchIDProduct.Text.Trim() !="")
            {
                ID = string.Format(@"idFood = '{0}'", txtSearchIDProduct.Text.Trim());
                result.Add(ID);
            }
            String Name = "";
            if (txtSearchNameProduct.Text.Trim() !="")
            {
                Name = string.Format(@"name = '{0}'", txtSearchNameProduct.Text.Trim());
                result.Add(Name);
            }
            String Unit = "";
            if(txtSearchUnitProduct.Text.Trim() !="")
            {
                Unit = string.Format(@"unit = '{0}'", txtSearchUnitProduct.Text.Trim());
                result.Add(Unit);
            }
            String Price = "";
            if(txtSearchPriceProduct.Text.Trim()!="")
            {
                Price = string.Format(@"price = '{0}'", txtSearchPriceProduct.Text.Trim());
                result.Add(Price);
            }
            String IdCatalog = "";
            if(chkidcatalog.Checked == true)
            {
                IdCatalog = @"idFoodCategory = '" + cbbSearchProduct.SelectedValue + "'";
                result.Add(IdCatalog);
            }

            return result;
        }
        private List<String> DataSearchProduct()
        {
            List<String> result = new List<string>();
            String ID = "";
            if (txtSearchIDProduct.Text.Trim() != "")
            {
                ID = string.Format(@"idFood = '{0}'", txtSearchIDProduct.Text.Trim());
                result.Add(ID);
            }
            String Name = "";
            if (txtSearchNameProduct.Text.Trim() != "")
            {
                Name = string.Format(@"name like '%{0}%'", txtSearchNameProduct.Text.Trim());
                result.Add(Name);
            }
            String Unit = "";
            if (txtSearchUnitProduct.Text.Trim() != "")
            {
                Unit = string.Format(@"unit like '%{0}%'", txtSearchUnitProduct.Text.Trim());
                result.Add(Unit);
            }
            String Price = "";
            if (txtSearchPriceProduct.Text.Trim() != "")
            {
                Price = string.Format(@"price like '%{0}%'", txtSearchPriceProduct.Text.Trim());
                result.Add(Price);
            }
            String IdCatalog = "";
            if (chkidcatalog.Checked == true)
            {
                IdCatalog = string.Format(@"idFoodCategory = '{0}'", cbbSearchProduct.SelectedValue);
                result.Add(IdCatalog);
            }

            return result;
        }

        private void chkidcatalog_CheckedChanged(object sender, EventArgs e)
        {
            if (chkidcatalog.CheckState == CheckState.Checked)
            {
                cbbSearchProduct.Enabled = true;
            }
            else
            {
                cbbSearchProduct.Enabled = false;
            }
        }

        private void btnSearchResetProduct_Click(object sender, EventArgs e)
        {
            txtSearchIDProduct.Text = "";
            txtSearchNameProduct.Text = "";
            txtSearchUnitProduct.Text = "";
            txtSearchPriceProduct.Text = "";
        }

        private void textEdit7_KeyPress(object sender, KeyPressEventArgs e)
        {
            inputnumber(sender, e);
        }
        /// <summary>
        /// Thêm catalog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCatalog_Click(object sender, EventArgs e)
        {
            String Name = txtNameCatalog.Text.Trim() != "" ? txtNameCatalog.Text.Trim() : "Chưa có tên";
            if(MessageBox.Show("Bạn có muốn thêm "+ Name, "Thông Báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (CatalogProductDAO.Instance.AddCatalog(Name))
                {
                    loadData(CatalogProductDAO.Instance.LoadCatalog(), dtgrvCatalogProduct);
                    MessageBox.Show("Thêm thành công danh mục "+ Name,"Thêm Danh Mục Thành Công");
                }
                else
                    MessageBox.Show("Lỗi khi thêm Danh Mục sản phẩm mới.", "Lỗi Thêm Danh mục sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtgrvCatalogProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BingdingCatalog();
        }
        private void BingdingCatalog()
        {
            CatalogProduct Catalog;
            int id = Convert.ToInt32(dtgrvCatalogProduct.Rows[this.dtgrvCatalogProduct.CurrentCell.RowIndex].Cells[0].Value.ToString());
            Catalog = CatalogProductDAO.Instance.LoadCatalogByID(id);
            txtIDCatalog.Text = Catalog.Id.ToString();
            txtNameCatalog.Text = Catalog.Name;

        }
        /// <summary>
        /// Sửa catalog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditCatalog_Click(object sender, EventArgs e)
        {
            if (txtIDCatalog.Text.Trim() != "")
            {
                int id = Convert.ToInt32(txtIDCatalog.Text.Trim());
                String Name = txtNameCatalog.Text != "" ? txtNameCatalog.Text.Trim() : "Chưa có tên";
                if (MessageBox.Show("Bạn có muốn sửa " + id + " ?", "Cảnh Báo Sửa", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (CatalogProductDAO.Instance.EditCatalog(id, Name))
                    {
                        loadData(CatalogProductDAO.Instance.LoadCatalog(), dtgrvCatalogProduct);
                        MessageBox.Show("Sửa thành công " + id, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                        MessageBox.Show("Lỗi không thể sửa " + id, "Lỗi Sửa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Không có danh mục nào được chọn", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnResetCatalog_Click(object sender, EventArgs e)
        {
            txtNameCatalog.Text = "";
            txtIDCatalog.Text = "";
        }
        /// <summary>
        /// Xóa Catalog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteCatalog_Click_1(object sender, EventArgs e)
        {
            if (txtIDCatalog.Text.Trim() != "")
            {
                int id = Convert.ToInt32(txtIDCatalog.Text.Trim());

                if (MessageBox.Show(string.Format("Bạn có muốn xóa {0} ?", id), "Cảnh Báo Xóa", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (CatalogProductDAO.Instance.DeleteCatalog(id))
                    {
                        loadData(CatalogProductDAO.Instance.LoadCatalog(), dtgrvCatalogProduct);
                        MessageBox.Show("Xóa thành công " + id, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                        MessageBox.Show("Lỗi không thể xóa " + id, "Lỗi Xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Không có danh mục nào được chọn", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        
        }
        /// <summary>
        /// Sự kiện tìm kiếm catalog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchCatalog_Click(object sender, EventArgs e)
        {
            if (chkExactlySearchCatalog.Checked == true)
            {
                SearchCatalogExac();
            }
            else
                SearchCatalog();
        }
        /// <summary>
        /// Tìm kiếm chính xác danh mục
        /// </summary>
        private void SearchCatalogExac()
        {
            List<String> sql = DataSearchCatalogExac();
            if(sql.Count <1)
            {
                loadData(CatalogProductDAO.Instance.LoadCatalog(), dtgrvCatalogProduct);
            }
            else
            {
                String sQLQuery = @"SELECT dbo.FoodCategory.idFoodCategory AS [Mã Danh Mục], dbo.FoodCategory.name AS [Tên Danh Mục] FROM dbo.FoodCategory WHERE";
                String tempwhere = "";
                foreach (String item in sql)
                {
                    tempwhere = string.Format("{0} and {1}", tempwhere, item);
                }
                String Str1 = tempwhere.Substring(4);
                sQLQuery = sQLQuery + Str1;
                DataTable dt = CatalogProductDAO.Instance.SearchCatalogExactly(sQLQuery);
                if (dt.Rows.Count > 0)
                {
                    loadData(dt, dtgrvCatalogProduct);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy danh mục.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        /// <summary>
        /// Lấy dứ liệu nhập tìm kiếm chính xác catalog
        /// </summary>
        /// <returns></returns>
        private List<String> DataSearchCatalogExac()
        {
            List<String> result = new List<string>();
            String id = "";
            if(txtSearchIDCatalog.Text.Trim() !="")
            {
                id = string.Format(@"idFoodCategory = '{0}'", txtSearchIDCatalog.Text.Trim());
                result.Add(id);
            }
            String name;
            if(txtSearchNameCatalog.Text.Trim()!= "")
            {
                name = string.Format(@"name = '{0}'", txtSearchNameCatalog.Text.Trim());
                result.Add(name);
            }
            return result;
        }

        /// <summary>
        /// Tìm kiếm tương đối danh mục
        /// </summary>
        private void SearchCatalog()
        {
            List<String> sql = DataSearchCatalog();
            if (sql.Count < 1)
            {
                loadData(CatalogProductDAO.Instance.LoadCatalog(), dtgrvCatalogProduct);
            }
            else
            {
                String sQLQuery = @"SELECT dbo.FoodCategory.idFoodCategory AS [Mã Danh Mục], dbo.FoodCategory.name AS [Tên Danh Mục] FROM dbo.FoodCategory WHERE";
                String tempwhere = "";
                foreach (String item in sql)
                {
                    tempwhere = string.Format("{0} or {1}", tempwhere, item);
                }
                String Str1 = tempwhere.Substring(3);
                sQLQuery = sQLQuery + Str1;
                DataTable dt = CatalogProductDAO.Instance.SearchCatalogExactly(sQLQuery);
                if (dt.Rows.Count > 0)
                {
                    loadData(dt, dtgrvCatalogProduct);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy danh mục.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        /// <summary>
        /// Lấy dứ liệu nhập tìm kiếm catalog
        /// </summary>
        /// <returns></returns>
        private List<String> DataSearchCatalog()
        {
            List<String> result = new List<string>();
            String id = "";
            if (txtSearchIDCatalog.Text.Trim() != "")
            {
                id = string.Format(@"idFoodCategory like '%{0}%'", txtSearchIDCatalog.Text.Trim());
                result.Add(id);
            }
            String name;
            if (txtSearchNameCatalog.Text.Trim() != "")
            {
                name = string.Format(@"name like N'%{0}%'", txtSearchNameCatalog.Text.Trim());
                result.Add(name);
            }
            return result;
        }

        private void btnSearchResetCatalog_Click(object sender, EventArgs e)
        {
            txtSearchIDCatalog.Text = "";
            txtSearchNameCatalog.Text = "";
            chkExactlySearchCatalog.Checked = false;
        }



        
        #region method
        #endregion

    }
}
