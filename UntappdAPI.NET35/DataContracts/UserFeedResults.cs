using System;
using System.Runtime.Serialization;
using UntappdAPI.Extensions;


namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class UserFeedResults
    {
        [DataMember(Name = "user")]
        public User User { get; set; }
        [DataMember(Name = "checkin_id")]
        public int CheckinId { get; set; }
        [DataMember(Name = "beer_name")]
        public string BeerName { get; set; }
        [DataMember(Name = "beer_id")]
        public int BeerId { get; set; }
        [DataMember(Name = "brewery_name")]
        public string BreweryName { get; set; }
        [DataMember(Name = "brewery_id")]
        public string BreweryId { get; set; }
        [DataMember(Name = "beer_stamp")]
        public string BeerStamp { get; set; }
        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }
        [DataMember(Name = "checkin_comment")]
        public string CheckinComment { get; set; }
        [DataMember(Name = "created_link")]
        public string CheckinLink { get; set; }
        [DataMember(Name = "venue_name")]
        public string VenueName { get; set; }
        [DataMember(Name = "venue_id")]
        public int? VenueId { get; set; }
        [DataMember(Name = "venue_lat")]
        public float? VenueLat { get; set; }
        [DataMember(Name = "venue_lng")]
        public float? VenueLng { get; set; }

        public string DisplayCreatedTimeAgo
        {
            get
            {
                return DateTime.Parse(CreatedAt).ToTimeAgo();
            }
        }
    }
}
