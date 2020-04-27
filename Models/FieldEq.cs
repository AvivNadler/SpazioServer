using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpazioServer.Models
{
    public class FieldEq
    {
        int id;
        string field;
        string name;

        public FieldEq() { }

        public FieldEq(int id, string field, string name)
        {
            this.id = id;
            this.field = field;
            this.name = name;
        }

        public int Id { get => id; set => id = value; }
        public string Field { get => field; set => field = value; }
        public string Name { get => name; set => name = value; }

        public List<FieldEq> getFieldsEq()
        {
            DBServices dbs = new DBServices();
            return dbs.readFieldsEq();
        }
        public int insert()
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.insert(this);
            return numAffected;
        }

    }
}