using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpazioServer.Models;

namespace SpazioServer.Controllers
{
    public class AvailabilityController : ApiController
    {
        // GET api/<controller>
        public List<Availability> Get()
        {
            Availability a = new Availability();
            return a.getAvailabilities();
        }

        // GET api/<controller>/5

        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>



        public Availability Post([FromBody]Availability availability)
        {

            availability.insert();
            return availability;

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