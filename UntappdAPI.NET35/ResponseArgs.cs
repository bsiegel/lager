using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UntappdAPI
{
    public class ResponseArgs<T> : EventArgs
    {
        public ResponseArgs(T result, string rawResult)
        {
            Result = result;
            RawResult = rawResult;
        }

        public T Result { get; set; }
        public string RawResult { get; set; }
    }
}
