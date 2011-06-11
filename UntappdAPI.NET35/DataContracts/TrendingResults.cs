using System;
using System.Runtime.Serialization;
using UntappdAPI.Extensions;


namespace UntappdAPI.DataContracts {
    [DataContract]
    public class TrendingResults {
        [DataMember(Name = "beer_name")]
        public string BeerName {
            get;
            set;
        }
        [DataMember(Name = "beer_id")]
        public int BeerId {
            get;
            set;
        }
        [DataMember(Name = "brewery_name")]
        public string BreweryName {
            get;
            set;
        }
        [DataMember(Name = "brewery_id")]
        public string BreweryId {
            get;
            set;
        }
        [DataMember(Name = "count")]
        public int Count {
            get;
            set;
        }
        [DataMember(Name = "img")]
        public string Image {
            get;
            set;
        }
    }
}
