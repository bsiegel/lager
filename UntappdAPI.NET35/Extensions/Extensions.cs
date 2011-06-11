using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UntappdAPI.Extensions
{
    public static class FormatExtensions
    {
        public static string ToTimeAgo(this DateTime dateTime)
        {
                var ts = DateTime.Now.Subtract(dateTime);
                string retVal = string.Empty;
                if (ts.Days == 0 && ts.Hours == 0)
                {
                    if (ts.Minutes == 0)
                        retVal = "1 minute ago";
                    else
                        retVal = ts.Minutes + " minutes ago";
                }
                else if (ts.Days == 0 && ts.Hours > 0)
                {
                    if (ts.Hours == 1)
                        retVal = ts.Hours + " hour ago";
                    else
                        retVal = ts.Hours + " hours ago";
                }
                else
                {
                    if (ts.Days == 1)
                        retVal = ts.Days + " day ago";
                    else
                        retVal = ts.Days + " days ago";
                }

                return retVal;
        }
    }
}
