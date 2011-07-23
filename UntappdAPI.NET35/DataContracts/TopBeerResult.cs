using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class TopBeerResult
    {
        [DataMember(Name = "beer_name")]
        public string Name { get; set; }
        [DataMember(Name = "beer_id")]
        public int BeerId { get; set; }
        [DataMember(Name = "beer_type")]
        public string Style { get; set; }
    }
}
