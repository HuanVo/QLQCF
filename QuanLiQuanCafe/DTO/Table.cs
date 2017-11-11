using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Table
    {
        private int id;
        private String name;
        private bool status;

        public bool Status
        {
            get { return status; }
            set { status = value; }
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
        public Table()
        {

        }
        public Table(int id, String name, bool status)
        {
            this.Id = id;
            this.Name = name;
            this.Status = status;
        }
        public Table(DataRow dtr)
        {
            this.Id = Convert.ToInt32(dtr["idTableFood"].ToString());
            this.Name = dtr["name"].ToString();
            this.Status = Convert.ToBoolean(dtr["stats"].ToString());
        }

    }
}
