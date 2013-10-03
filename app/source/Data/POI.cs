using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNMC2013.Data
{
    public class POI
    {
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Phone { get; set; }

        public string ImagePath { get; set; }
        public string Website { get; set; }

        public double GeoLong { get; set; }
        public double GeoLat { get; set; }


    }
}
