using System;
using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts {
    [DataContract]
    public class TrendingResponse : BaseResponse {
        [DataMember(Name = "results")]
        public TrendingResults[] Results {
            get;
            set;
        }

        public string FeedType {
            get;
            set;
        }
    }
}
