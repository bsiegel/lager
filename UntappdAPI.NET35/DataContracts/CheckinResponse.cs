using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class CheckinResponse : BaseResponse
    {
        [DataMember(Name = "result")]
        public string CheckinResult { get; set; }
    }
}