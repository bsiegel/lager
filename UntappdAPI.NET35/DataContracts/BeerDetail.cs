using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class BeerDetail {
        [DataMember(Name = "beer_name")]
        public string Name { get; set; }
        [DataMember(Name = "beer_id")]
        public int BeerId { get; set; }
        [DataMember(Name = "brewery_name")]
        public string Brewery { get; set; }
        [DataMember(Name = "brewery_id")]
        public int BreweryId { get; set; }
        [DataMember(Name = "img")]
        public string Img { get; set; }
        [DataMember(Name = "beer_creator")]
        public string BeerCreator { get; set; }
        [DataMember(Name = "type_name")]
        public string Style { get; set; }
        [DataMember(Name = "type_id")]
        public int StyleId { get; set; }
    }
}
