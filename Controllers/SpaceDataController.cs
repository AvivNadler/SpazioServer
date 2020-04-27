using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpazioServer.Models;

namespace SpazioServer.Controllers
{
    public class SpaceDataController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public SpaceData Post([FromBody]SpaceData spaceData)
        {
            bool userType;
            Space s = spaceData.Space;
            Facility f = spaceData.Facility;
            Equipment[] e = spaceData.Equipment;
            Availability a = spaceData.Availability;

            DBServices dbs = new DBServices();

            int newSpaceId = dbs.insert(s);

            f.SpaceId = newSpaceId;
            foreach (Equipment item in e)
            {
                item.SpaceId = newSpaceId;
            }
            //e.SpaceId = newSpaceId;
            a.SpaceId = newSpaceId;
            userType = dbs.userTypeCheckandUpdate(s.UserEmail);

            //  countertest variabe for test how many rows affected 
            int countertest = 0;
            countertest += dbs.insert(f);

            foreach (Equipment item in e)
            {
                countertest += dbs.insert(item);
            }
            
            countertest += dbs.insert(a);

            return spaceData;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}