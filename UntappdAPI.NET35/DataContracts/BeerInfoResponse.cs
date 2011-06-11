using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class BeerInfoResponse : BaseResponse
    {
        [DataMember(Name = "results")]
        public BeerInfoResult Results { get; set; }
    }
}
