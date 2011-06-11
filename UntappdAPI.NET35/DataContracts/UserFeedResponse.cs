using System;
using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class UserFeedResponse : BaseResponse
    {
        [DataMember(Name = "next_query")]
        public string NextQuery { get; set; }
        [DataMember(Name = "next_page")]
        public string NextPageUrl { get; set; }
        [DataMember(Name = "results")]
        public UserFeedResults[] Results { get; set; }

        public string FeedType { get; set; }

        public int NextOffset
        {
            get
            {
                var offsetString = GetNextPageQueryParam(NextPageUrl, "offset");
                int offset;
                Int32.TryParse(offsetString, out offset);
                return offset;
            }
        }
    }
}
