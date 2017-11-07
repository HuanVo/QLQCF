using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CatalogProduct
    {
        private int id;
        private String name;

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

        public CatalogProduct()
        { }
        public CatalogProduct(int id, String name)
        {
            this.Id = id;
            this.Name = name;
        }
        public CatalogProduct(DataRow row)
        {
            this.Id = Convert.ToInt32(row[0].ToString());
            this.Name = row[1].ToString();
        }

    }
}
