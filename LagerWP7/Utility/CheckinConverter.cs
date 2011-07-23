using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace LagerWP7 {
    public class CheckinConverter : IValueConverter {
        private List<char> _vowels = new List<char> { 'a', 'e', 'i', 'o', 'u' };

        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture) {
            var model = (CheckinResultPage) value;
            var sb = new StringBuilder();

            sb.Append("you're having ");
            if (_vowels.Contains(model.BeerName[0])) {
                sb.Append(" an ");
            } else {
                sb.Append(" a ");
            }
            sb.Append(model.BeerName);
            sb.Append(" by ");
            sb.Append(model.BreweryName);

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
