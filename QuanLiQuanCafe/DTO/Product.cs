using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Product
    {
        private int id;
        private String name;
        private String unit;
        private int idcatalog;
        private Double price;

        public Double Price
        {
            get { return price; }
            set { price = value; }
        }

        public int Idcatalog
        {
            get { return idcatalog; }
            set { idcatalog = value; }
        }

        public String Unit
        {
            get { return unit; }
            set { unit = value; }
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

        public Product()
        {

        }
        public Product(int id, String name, String unit, int idcatalog, Double price)
        {
            this.Id = id;
            this.Name = name;
            this.Unit = unit;
            this.Idcatalog = idcatalog;
            this.Price = price;
        }

        public Product(DataRow dtr)
        {
            this.Id = Convert.ToInt32(dtr["idFood"].ToString());
            this.Name = dtr["name"].ToString();
            this.Unit = dtr["unit"].ToString();
            this.Idcatalog = Convert.ToInt32(dtr["idFoodCategory"].ToString());
            this.Price = Convert.ToDouble(dtr["price"].ToString());
        }
    }
}
