using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class Badge {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "descrip")]
        public string Description { get; set; }
        [DataMember(Name = "img_sm")]
        public string ImgSmall { get; set; }
        [DataMember(Name = "img_md")]
        public string ImgMedium { get; set; }
        [DataMember(Name = "img_lg")]
        public string ImgLarge { get; set; }
    }
}
