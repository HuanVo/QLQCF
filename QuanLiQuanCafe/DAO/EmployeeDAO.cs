using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAO
{
    public class EmployeeDAO
    {
        private static EmployeeDAO instance;

        public static EmployeeDAO Instance
        {
            get { if(instance == null)
                instance = new EmployeeDAO();
                return instance;
            }
           private set { instance = value; }
        }
       private EmployeeDAO()
       {}
        /// <summary>
        /// Them moi nha vien
        /// </summary>
        /// <param name="Empl"></param>
        /// <returns></returns>
       public bool AddEmployee(Employee Empl)
       {
           bool result = false;
            try
            {
                String sqlquery = String.Format(@"EXEC dbo.addEmployee N'{0}', {1}, N'{2}', '{3}', '{4}', '{5}', '{6}', {7}", Empl.Name, Empl.Sex, Empl.Address, Empl.Phone, Empl.StartWork, Empl.Salary, Empl.EndDaySalary, Empl.SalaryAdvance);
                if(DataProvider.Instance.ExcuteNonQuery(sqlquery)>0)
                {
                    result = true;
                }
            }
            catch(Exception e)
            {
                ErrorLog.WriteLog(e.Message);
            }
            return result;
       }
        /// <summary>
        /// Sua mot nhan vien
        /// </summary>
        /// <param name="Empl"></param>
        /// <returns></returns>
       public bool EditEmployee(Employee Empl)
       {
           bool result = false;
           try
           {
               String sqlquery = String.Format(@"EXEC dbo.editEmployee N'{0}', {1}, N'{2}', '{3}', '{4}', '{5}', '{6}', {7}, '{8}'", Empl.Name, Empl.Sex, Empl.Address, Empl.Phone, Empl.StartWork, Empl.Salary, Empl.EndDaySalary, Empl.SalaryAdvance,Empl.Id);
               if (DataProvider.Instance.ExcuteNonQuery(sqlquery) > 0)
               {
                   result = true;
               }
           }
           catch (Exception e)
           {
               ErrorLog.WriteLog(e.Message);
           }
           return result;
       }
        /// <summary>
        /// Xóa một nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteEmployee(String id)
       {
           bool result = false;
           try
           {
               String sqlquery = String.Format(@"EXEC dbo.DeleteEmployee '{0}'", id);
               if (DataProvider.Instance.ExcuteNonQuery(sqlquery) > 0)
               {
                   result = true;
               }
           }
           catch (Exception e)
           {
               ErrorLog.WriteLog(e.Message);
           }
           return result;
       }
        /// <summary>
        /// Load du lieu nhan vien
        /// </summary>
        /// <returns></returns>
        public DataTable LoadEmployee()
       {
           DataTable dt = null;
           try
           {
               String sqlquery = @"EXEC dbo.getEmployees";
               dt = DataProvider.Instance.LoadAllTable(sqlquery);
           }
            catch(Exception ex)
           {
               ErrorLog.WriteLog(ex.Message);
           }
           return dt;
       }
        public Employee LoadEmployeeByID(int id)
        {
            Employee emp = null;
            try
            {
                String sqlquery = String.Format(@"getEmployeesByID '{0}'",id);
                DataTable dt = DataProvider.Instance.LoadAllTable(sqlquery);
                emp = new Employee(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            return emp;
        }

        public DataTable SearchEmployeeExactly(String sql)
        {
            DataTable dt = null;
            try
            {
                dt = DataProvider.Instance.LoadAllTable(sql);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            return dt;
        }
        /// <summary>
        /// Kiểm tra tồn tại 1 nhân viên
        /// trả về kiểu bool
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool CheckIsEmployee(String ID)
        {
            String sql = @"getEmployeesByID "+ ID;
            if (DataProvider.Instance.ExcuteScaler(sql) > 0)
                return true;
            return false;
        }
    }

}
