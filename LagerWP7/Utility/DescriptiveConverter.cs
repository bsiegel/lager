using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace LagerWP7 {
    public class DescriptiveConverter : IValueConverter {
        private List<char> _vowels = new List<char> { 'a', 'e', 'i', 'o', 'u' };
        private List<string> _verbs = new List<string> {
            "drinking",
            "sipping",
            "enjoying",
            "relaxing with",
            "knocking back",
            "having",
            "nursing"
        };

        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture) {
            var model = (ItemViewModel) value;
            var sb = new StringBuilder();

            sb.Append("is ");
            sb.Append(_verbs[new Random().Next(_verbs.Count)]);
            if (_vowels.Contains(model.LineTwo[0])) {
                sb.Append(" an ");
            } else {
                sb.Append(" a ");
            }

            sb.Append("[link=\"BeerPage.xaml?id=");
            sb.Append(model.IdTwo);
            sb.Append("\"]");
            sb.Append(model.LineTwo);
            sb.Append("[/link] by [link=\"BreweryPage.xaml?id=");
            sb.Append(model.IdThree);
            sb.Append("\"]");
            sb.Append(model.LineThree);
            sb.Append("[/link]");

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
