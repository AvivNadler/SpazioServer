using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using SpazioServer.Models;

namespace SpazioServer.Models
{
    public class SpaceData
    {
        Space space;
        Facility facility;
        Equipment[] equipment;
        Availability availability;

        public SpaceData() { }

        public SpaceData(Space space, Facility facility, Equipment[] equipment, Availability availability)
        {
            this.space = space;
            this.facility = facility;
            this.equipment = equipment;
            this.availability = availability;
        }

        public Space Space { get => space; set => space = value; }
        public Facility Facility { get => facility; set => facility = value; }
        public Equipment[] Equipment { get => equipment; set => equipment = value; }
        public Availability Availability { get => availability; set => availability = value; }
    }
}