using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace UntappdAPI.DataContracts
{
    [DataContract]
    public class BaseResponse
    {
        [DataMember(Name = "http_code")]
        public int HttpCode { get; set; }
        [DataMember(Name = "error")]
        public string ErrorMessage { get; set; }

        protected static string GetNextPageQueryParam(string nextPageUrl, string paramName)
        {
            Uri nextPage;
            if (!Uri.TryCreate(nextPageUrl, UriKind.Absolute, out nextPage))
                return null;

            var qparams = nextPage.Query.Remove(0, 1).Split('&');
            return Uri.UnescapeDataString(qparams.Where(p => p.StartsWith(paramName + "=", StringComparison.OrdinalIgnoreCase)).Single().Replace(paramName + "=", String.Empty)).Replace('%', ' ');
        }
    }
}
