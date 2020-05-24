using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace SpazioServer.Models
{
    public class RealAvailability
    {
        int id;
        int year;
        int weekNumber;
        string day;
        string date;
        DateTime date2;
        string startTime;
        string endTime;
        int fkSpaceId;
        //maybe dates and times with strings  with string and cast


        public RealAvailability() { }

        public RealAvailability(int id, int year, int weekNumber, string day, string date, DateTime date2, string startTime, string endTime, int fkSpaceId)
        {
            this.Id = id;
            this.Year = year;
            this.WeekNumber = weekNumber;
            this.Day = day;
            this.Date = date;
            this.Date2 = date2;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.FkSpaceId = fkSpaceId;
        }

        public int Id { get => id; set => id = value; }
        public int Year { get => year; set => year = value; }
        public int WeekNumber { get => weekNumber; set => weekNumber = value; }
        public string Day { get => day; set => day = value; }
        public string Date { get => date; set => date = value; }
        public DateTime Date2 { get => date2; set => date2 = value; }
        public string StartTime { get => startTime; set => startTime = value; }
        public string EndTime { get => endTime; set => endTime = value; }
        public int FkSpaceId { get => fkSpaceId; set => fkSpaceId = value; }

        
        public List<RealAvailability> getRealAvailbility(int spaceId, string dayname)
        {
            DBServices dbs = new DBServices();
            return dbs.readRealAvailbility(spaceId, dayname);
        }
        public int insert()
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.insert(this);
            return numAffected;
        }
        public List<string> getAllAvailbilities(int spaceid, string date)
        {
            DBServices dbs = new DBServices();
            return dbs.readAllAvailbilities(spaceid,date);
        }

        public int update(RealAvailability realAvailability)
        {

            DBServices dbs = new DBServices();
            dbs = dbs.readRealAvailabilities();
            dbs.dt = RealAvailabilityTable(this, dbs.dt);
            dbs.update();
            return 0;
        }

        public DataTable RealAvailabilityTable(RealAvailability realAvailability, DataTable dt)
        {

            foreach (DataRow dr in dt.Rows)
            {

                int id = Convert.ToInt32(dr["Id"]);
                if (id == realAvailability.Id)
                {
                    //dr["lang"] = country.Lang;
                    // dr["continent"] = country.Continent;
                    dr["Year"] = realAvailability.Year;
                    dr["WeekNumber"] = realAvailability.WeekNumber;
                    dr["Week_Day"] = realAvailability.Day;
                    dr["StartTime"] = realAvailability.StartTime;
                    dr["EndTime"] = realAvailability.endTime;
                    dr["FkSpaceId"] = realAvailability.fkSpaceId;
                    dr["AvailabilityDate"] = realAvailability.Date2;



                }

            }
            return dt;
        }
        //public List<RealAvailability> getRealAvailbilities()
        //{
        //    DBServices dbs = new DBServices();
        //    return dbs.readRealAvailbilities();
        //}
    }
}