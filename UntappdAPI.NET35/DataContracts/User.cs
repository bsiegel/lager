using System;
using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "user_name")]
        public string UserName { get; set; }
        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }
        [DataMember(Name = "last_name")]
        public string LastName { get; set; }
        [DataMember(Name = "user_avatar")]
        public string AvatarUrl { get; set; }
        [DataMember(Name = "location")]
        public string Location { get; set; }
        [DataMember(Name = "website")]
        public string Website { get; set; }
        [DataMember(Name = "bio")]
        public string Bio { get; set; }
        [DataMember(Name = "is_friends")]
        public int? IsFriendsValue {get; set;}
        [DataMember(Name = "is_requested")]
        public int? IsRequestedValue {get; set;}
        [DataMember(Name = "date_joined")]
        public string DateJoined { get; set; }
        [DataMember(Name = "total_badges")]
        public int Badges { get; set; }
        [DataMember(Name = "total_friends")]
        public int Friends { get; set; }
        [DataMember(Name = "total_checkins")]
        public int Checkins { get; set; }
        [DataMember(Name = "total_beers")]
        public int UniqueBeers { get; set; }
        [DataMember(Name = "created_beers")]
        public int CreatedBeers { get; set; }

        public bool IsFriends {
            get {
                return IsFriendsValue.GetValueOrDefault(0) == 1;
            }
        }

        public bool IsRequested {
            get {
                return IsRequestedValue.GetValueOrDefault(0) == 1;
            }
        }

        public string DisplayName
        {
            get
            {
                var name = FirstName;
                if (!String.IsNullOrEmpty(LastName) && LastName.Length > 0)
                    name = name + " " + LastName[0] + ".";
                
                return name;
            }
        }

        public string DisplayNameFull
        {
            get { return FirstName + " " + LastName; }
        }
    }
}
