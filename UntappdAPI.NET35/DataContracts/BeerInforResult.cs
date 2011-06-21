using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class BeerInfoResult
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "beer_id")]
        public int BeerId { get; set; }
        [DataMember(Name = "brewery")]
        public string Brewery { get; set; }
        [DataMember(Name = "brewery_id")]
        public int BreweryId { get; set; }
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
        [DataMember(Name = "Beer_created")]
        public string BeerCreated { get; set; }
        [DataMember(Name = "beer_owner")]
        public string BeerOwner { get; set; }
        [DataMember(Name = "beer_creator")]
        public string BeerCreator { get; set; }
        [DataMember(Name = "beer_creator_id")]
        public string BeerCreatorId { get; set; }
        [DataMember(Name = "is_had")]
        public string IsHad { get; set; }
        [DataMember(Name = "beer_abv")]
        public decimal Abv { get; set; }
        [DataMember(Name = "type")]
        public string Style { get; set; }
        [DataMember(Name = "your_count")]
        public int YourCount { get; set; }
        [DataMember(Name = "avg_rating")]
        public int AvgRating { get; set; }
        [DataMember(Name = "your_rating")]
        public int YourRating { get; set; }
    }
}
