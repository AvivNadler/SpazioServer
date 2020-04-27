using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpazioServer.Models
{
    public class Facility
    {
        int id;
        bool parking;
        bool toilet;
        bool kitchen;
        bool intercom;
        bool accessible;
        bool airCondition;
        bool wifi;
        int spaceId;

        public Facility() { }

        public Facility(int id, bool parking, bool toilet, bool kitchen, bool intercom, bool accessible, bool airCondition, bool wifi, int spaceId)
        {
            this.id = id;
            this.parking = parking;
            this.toilet = toilet;
            this.kitchen = kitchen;
            this.intercom = intercom;
            this.accessible = accessible;
            this.airCondition = airCondition;
            this.wifi = wifi;
            this.spaceId = spaceId;
        }

        public int Id { get => id; set => id = value; }
        public bool Parking { get => parking; set => parking = value; }
        public bool Toilet { get => toilet; set => toilet = value; }
        public bool Kitchen { get => kitchen; set => kitchen = value; }
        public bool Intercom { get => intercom; set => intercom = value; }
        public bool Accessible { get => accessible; set => accessible = value; }
        public bool AirCondition { get => airCondition; set => airCondition = value; }
        public bool Wifi { get => wifi; set => wifi = value; }
        public int SpaceId { get => spaceId; set => spaceId = value; }

        public List<Facility> getFacilities()
        {
            DBServices dbs = new DBServices();
            return dbs.readFacilities();
        }
        public int insert()
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.insert(this);
            return numAffected;
        }
    }
}