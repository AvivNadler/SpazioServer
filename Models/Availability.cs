using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SpazioServer.Models
{
    public class Availability
    {
        int id;
        string sunday;
        string monday;
        string tuesday;
        string wednesday;
        string thursday;
        string friday;
        string saturday;
        int spaceId;

        public Availability() { }


        public Availability(int id, string sunday, string monday, string tuesday, string wednesday, string thursday, string friday, string saturday, int spaceId)
        {
            this.id = id;
            this.sunday = sunday;
            this.monday = monday;
            this.tuesday = tuesday;
            this.wednesday = wednesday;
            this.thursday = thursday;
            this.friday = friday;
            this.saturday = saturday;
            this.spaceId = spaceId;
        }

        public int Id { get => id; set => id = value; }
        public string Sunday { get => sunday; set => sunday = value; }
        public string Monday { get => monday; set => monday = value; }
        public string Tuesday { get => tuesday; set => tuesday = value; }
        public string Wednesday { get => wednesday; set => wednesday = value; }
        public string Thursday { get => thursday; set => thursday = value; }
        public string Friday { get => friday; set => friday = value; }
        public string Saturday { get => saturday; set => saturday = value; }
        public int SpaceId { get => spaceId; set => spaceId = value; }

        public List<Availability> getAvailabilities()
        {
            DBServices dbs = new DBServices();
            return dbs.readAvailabilities();
        }
        public int insert()
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.insert(this);
            return numAffected;
        }

        //internal IEnumerable<ParameterInfo> GetParameters()
        //{
        //    throw new NotImplementedException();
        //}
    }
}