using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class BeerSearchResult
    {
        [DataMember(Name = "beer_name")]
        public string Name { get; set; }
        [DataMember(Name = "beer_id")]
        public int BeerId { get; set; }
        [DataMember(Name = "brewery_name")]
        public string BreweryName { get; set; }
        [DataMember(Name = "brewery_id")]
        public string BreweryId { get; set; }
        [DataMember(Name = "beer_stamp")]
        public string BeerStamp { get; set; }
        [DataMember(Name = "brewery_stamp")]
        public string BreweryStamp { get; set; }
        [DataMember(Name = "country_name")]
        public string CountryName { get; set; }
    }
}
