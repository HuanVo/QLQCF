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
    }
}
