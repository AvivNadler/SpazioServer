using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpazioServer.Models;

namespace SpazioServer.Controllers
{
    public class EquipmentController : ApiController
    {
        // GET api/<controller>
        public List<Equipment> Get()
        {
            Equipment e = new Equipment();
            return e.getEquipments();
        }

        // GET api/<controller>/5

        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>



        public Equipment Post([FromBody]Equipment equipment)
        {

            equipment.insert();
            return equipment;

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