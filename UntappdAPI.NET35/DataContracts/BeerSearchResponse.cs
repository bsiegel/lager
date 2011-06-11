using System;
using System.Linq;
using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class BeerSearchResponse : BaseResponse
    {
        [DataMember(Name = "returned_results")]
        public int ReturnedResults { get; set; }
        [DataMember(Name = "results")]
        public BeerSearchResult[] Results { get; set; }
        [DataMember(Name = "next_page")]
        public string NextPageUrl { get; set; }
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
        public string Query
        {
            get
            {
                return GetNextPageQueryParam(NextPageUrl, "q");
            }
        }
    }
}
