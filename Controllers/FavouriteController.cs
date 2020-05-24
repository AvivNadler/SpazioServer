using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using SpazioServer.Models;

namespace SpazioServer.Controllers
{
    public class FavouriteController : ApiController
    {
        // GET api/<controller>
         public List<Favourite> Get()
        {
            Favourite  f = new Favourite();
            return  f.getFavourites();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public Favourite Post([FromBody]Favourite favourite)
        {

            favourite.insert();
            return favourite;

        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }


        // example path: "../api/favourite/?spaceid=8&userid=22"
        // DELETE api/<controller>/5
        public void Delete(int spaceId, int userId)
        {
            Favourite f = new Favourite();
            f.removeFavourite(spaceId, userId);
        }

        [HttpGet]
        [Route("api/favourite/{id}")]
        public List<int> GetFavourites(int id)
        {
            Favourite f = new Favourite();
            return f.getFavourites(id);
        }



    }
}