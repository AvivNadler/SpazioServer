using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpazioServer.Models
{
    public class Equipment
    {
        int id;
        string name;
        int spaceId;

        public Equipment() { }
        public Equipment(int id, string name, int spaceId)
        {
            this.id = id;
            this.name = name;
            this.spaceId = spaceId;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int SpaceId { get => spaceId; set => spaceId = value; }

        public List<Equipment> getEquipments()
        {
            DBServices dbs = new DBServices();
            return dbs.readEquipments();
        }
        public int insert()
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.insert(this);
            return numAffected;
        }

    }

}