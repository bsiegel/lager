using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using UntappdAPI.DataContracts;

namespace LagerWP7 {
    public class CheckinConverter : IValueConverter {
        private List<char> _vowels = new List<char> { 'a', 'e', 'i', 'o', 'u' };

        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture) {
            var model = (CheckinResponse) value;
            var sb = new StringBuilder();

            if (model.IsTooFast) {
                sb.Append("woah there, slow down buddy! you need to wait ");
                sb.Append(model.TimeUntilAllowed);
                sb.Append(" more minute");
                sb.Append(model.TimeUntilAllowed == 1 ? "" : "s");
                sb.Append(" before you can check-in to another.");
            } else if (model.Result != null && model.Result.ToLowerInvariant() == "success") {
                sb.Append("you're having ");
                if (_vowels.Contains(model.BeerDetails.Name[0])) {
                    sb.Append("an ");
                } else {
                    sb.Append("a ");
                }
                sb.Append(model.BeerDetails.Name);
                sb.Append(" by ");
                sb.Append(model.BeerDetails.Brewery);
                sb.Append(". this is your ");
                sb.Append(model.CheckinTotals.BeerMonth);
                sb.Append(" ");
                sb.Append(model.BeerDetails.Name);
                sb.Append(" this month, and your ");
                sb.Append(model.CheckinTotals.Beer);
                sb.Append(" ever.");
            }

            return sb.ToString();
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture) {
            return null;
        }
    }
}
