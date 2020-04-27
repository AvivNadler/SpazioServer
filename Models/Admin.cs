using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpazioServer.Models
{
    public class Admin
    {
        int id;
        string userName;
        string password;
        string firstName;
        string lastName;
        public int Id { get => id; set => id = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }

        public Admin() { }

        public Admin(int id, string userName, string password, string firstName, string lastName)
        {
            this.id = id;
            this.userName = userName;
            this.password = password;
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public bool CheckLoginDetails(string username, string password)
        {
            DBServices dbs = new DBServices();
            return dbs.CheckAdmin(username, password);
        }

        public Admin GetAdmin(string username)
        {
            DBServices dbs = new DBServices();
            Admin a = new Admin();
            a = dbs.GetAdmin(username);
            return a;
        }
        public List<Admin> getAdmins()
        {
            DBServices dbs = new DBServices();
            return dbs.readAdmins();
        }




    }
}