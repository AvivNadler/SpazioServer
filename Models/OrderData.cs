using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using SpazioServer.Models;
using System.Collections.Generic;

namespace SpazioServer.Models
{
    public class OrderData
    {
        User user;
        Order order;

        public OrderData(User user, Order order)
        {
            this.User = user;
            this.Order = order;
        }

        public OrderData() { }
        public User User { get => user; set => user = value; }
        public Order Order { get => order; set => order = value; }
    }
}