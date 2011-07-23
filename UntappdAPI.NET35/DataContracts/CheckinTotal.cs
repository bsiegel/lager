using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class CheckinTotal {
        [DataMember(Name = "beer")]
        public string Beer { get; set; }
        [DataMember(Name = "beer_month")]
        public string BeerMonth { get; set; }
        [DataMember(Name = "venue")]
        public string Venue { get; set; }
        [DataMember(Name = "venue_month")]
        public string VenueMonth { get; set; }
    }
}
