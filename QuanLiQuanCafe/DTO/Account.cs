using System;
using System.Data;

namespace DTO
{
    public class Account
    {
        private String userName;

        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        private String displayName;
    

        public String DisplayName
        {
          get { return displayName; }
          set { displayName = value; }
        }
        private String password;

        public String Password
        {
            get { return password; }
            set { password = value; }
        }
        private bool type;

        public bool Type
        {
            get { return type; }
            set { type = value; }
        }

        public Account() { }
        public Account(String UserName, String DisplayName, String Password, bool Type)
        {
            this.UserName = UserName;
            this.DisplayName = DisplayName;
            this.Password = Password;
            this.Type = Type;
        }
        public Account(DataRow row)
        {
            this.UserName = row["UserName"].ToString();
            this.DisplayName = row["DisplayName"].ToString();
            //this.password = row["PassWord"].ToString();
            this.Type = bool.Parse(row["Type"].ToString());
        }
    }
}
