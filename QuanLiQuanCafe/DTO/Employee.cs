using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DTO
{
    public class Employee
    {
        public Employee(int id, String name, Boolean sex, String address, String phone, DateTime startwork, Double salary, DateTime enddaysalary,Double salaryadvence)
        {
            this.Id = id;
            this.name = name;
            this.sex = sex;
            this.address = address;
            this.phone = phone;
            this.startWork = startwork;
            this.salary = salary;
            this.endDaySalary = enddaysalary;
            this.salaryAdvance = salaryadvence;
        }

        public Employee(DataRow dt)
        {
            this.Id = Convert.ToInt32(dt["idEmployee"].ToString());
            this.name = dt["fullName"].ToString();
            this.sex = Convert.ToBoolean(dt["sex"].ToString());
            this.address = dt["addres"].ToString();
            this.phone = dt["phone"].ToString();
            this.startWork = Convert.ToDateTime(dt["dayStart"].ToString());
            this.salary = Convert.ToDouble(dt["salaryLevel"].ToString());
            this.endDaySalary = Convert.ToDateTime(dt["indicator"].ToString());
            this.salaryAdvance = Convert.ToDouble(dt["advance"].ToString());
        }

        private int id;
        private String name;
        private Boolean sex;
        private String address;
        private String phone;
        private DateTime startWork;
        private Double salary;
        private DateTime endDaySalary;
        private Double salaryAdvance;

        public Double SalaryAdvance
        {
            get { return salaryAdvance; }
            set { salaryAdvance = value; }
        }

        public DateTime EndDaySalary
        {
            get { return endDaySalary; }
            set { endDaySalary = value; }
        }

        public Double Salary
        {
            get { return salary; }
            set { salary = value; }
        }

        public DateTime StartWork
        {
            get { return startWork; }
            set { startWork = value; }
        }

        public String Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public String Address
        {
            get { return address; }
            set { address = value; }
        }


        public Boolean Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public Employee()
        { }
    }
   
}
