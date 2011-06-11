
namespace UntappdAPI
{
    public class Security
    {
        public static string GetMD5Hash(string str)
        {
            return MD5Core.GetHashString(str);
        }
    }
}
