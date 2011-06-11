using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UntappdAPI.DataContracts
{
    public class VenueCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PluralName { get; set; }
        public string Icon { get; set; }
        public bool IsPrimary { get; set; }
    }
}
