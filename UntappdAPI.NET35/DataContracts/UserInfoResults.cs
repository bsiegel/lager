using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class UserInfoResults
    {
        [DataMember(Name = "user")]
        public User User { get; set; }
    }
}
