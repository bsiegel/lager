using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class BreweryInfoResponse : BaseResponse
    {
        [DataMember(Name = "results")]
        public BreweryInfoResult Results { get; set; }
    }
}
