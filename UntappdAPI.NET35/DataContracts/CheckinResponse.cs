using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class CheckinResponse : BaseResponse
    {
        [DataMember(Name = "result")]
        public string Result { get; set; }
        [DataMember(Name = "checkin_total")]
        public CheckinTotal CheckinTotals { get; set; }
        [DataMember(Name = "badges")]
        public Badge[] Badges { get; set; }
        [DataMember(Name = "is_too_far_away")]
        public bool IsTooFarAway { get; set; }
        [DataMember(Name = "rapid_fire")]
        public bool IsTooFast { get; set; }
        [DataMember(Name = "rapid_fire_time_left")]
        public int TimeUntilAllowed { get; set; }
        [DataMember(Name = "beer_details")]
        public BeerDetail BeerDetails { get; set; }
        [DataMember(Name = "recommendations")]
        public Recommendation[] Recommendations { get; set; }
    }
}