using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UntappdAPI.DataContracts
{
    public class Venue
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public VenueLocation Location { get; set; }
        public VenueCategory PrimaryCategory { get; set; }
    }
}
