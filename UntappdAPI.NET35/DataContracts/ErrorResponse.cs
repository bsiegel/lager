using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace UntappdAPI.DataContracts
{
    public class ErrorResponse
    {
        public Exception Error { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public WebResponse Reponse { get; set; }
    }
}
