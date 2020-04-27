using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpazioServer.Models
{
    public class Favourite
    {
        int userId;
        int spaceId;

        public int UserId { get => userId; set => userId = value; }
        public int SpaceId { get => spaceId; set => spaceId = value; }
        public Favourite() { }
        public Favourite(int userId, int spaceId)
        {
            this.userId = userId;
            this.spaceId = spaceId;
        }

        public List<Favourite> getFavourites()
        {
            DBServices dbs = new DBServices();
            return dbs.readFavourites();
        }

        public int insert()
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.insert(this);
            return numAffected;
        }
        public List<int> getFavourites(int id)
        {
            DBServices dbs = new DBServices();
            return dbs.readFavouritesSpaces(id);
        }

        public int removeFavourite(int spaceId, int userId)
        {
            DBServices dbs = new DBServices();
            int numEffected = dbs.removeFavourite(spaceId, userId);
            return numEffected;
        }



    }
}