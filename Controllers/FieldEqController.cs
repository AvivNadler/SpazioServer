using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpazioServer.Models;

namespace SpazioServer.Controllers
{
    public class FieldEqController : ApiController
    {
        // GET api/<controller>
        public List<FieldEq> Get()
        {
            FieldEq fe = new FieldEq();
            return fe.getFieldsEq();
        }

        // GET api/<controller>/5

        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>



        public FieldEq Post([FromBody]FieldEq fieldEq)
        {

            fieldEq.insert();
            return fieldEq;

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