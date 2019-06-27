using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Dtos
{
    public class GeolocationDto
    {
        public string IP { get; set; }

        public Location Location { get; set; }
    }

    public class Location
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }
    }
}
