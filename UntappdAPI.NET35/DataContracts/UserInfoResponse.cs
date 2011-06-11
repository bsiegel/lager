using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class UserInfoResponse : BaseResponse
    {
        [DataMember(Name = "results")]
        public UserInfoResults Results { get; set; }
    }
}
