using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpazioServer.Models;

namespace SpazioServer.Controllers
{
    public class FacilitiesController : ApiController
    {
        // GET api/<controller>
        public List<Facility> Get()
        {
            Facility s = new Facility();
            return s.getFacilities();
        }

        // GET api/<controller>/5

        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>



        public Facility Post([FromBody]Facility facility)
        {

            facility.insert();
            return facility;

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