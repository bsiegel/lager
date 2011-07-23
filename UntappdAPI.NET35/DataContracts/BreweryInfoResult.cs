using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class BreweryInfoResult
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "brewery_id")]
        public int BreweryId { get; set; }
        [DataMember(Name = "country")]
        public string Country { get; set; }
        [DataMember(Name = "img")]
        public string Img { get; set; }
        [DataMember(Name = "total_count")]
        public int TotalCount { get; set; }
        [DataMember(Name = "unique_count")]
        public int UniqueCount { get; set; }
        [DataMember(Name = "monthly_count")]
        public int MonthlyCount { get; set; }
        [DataMember(Name = "weekly_count")]
        public int WeeklyCount { get; set; }
        [DataMember(Name = "twitter_handle")]
        public string TwitterHandle { get; set; }
        [DataMember(Name = "top_beers")]
        public TopBeerResult[] TopBeers { get; set; }
    }
}
