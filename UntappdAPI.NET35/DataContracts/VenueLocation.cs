using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UntappdAPI.DataContracts
{
    public class VenueLocation
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int Distance { get; set; }
    }
}
