using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LagerWP7 {
    public class VisibilityConverter : IValueConverter {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture) {
                if (value is bool) {
                    return (bool) value ? Visibility.Visible : Visibility.Collapsed;
                } else {
                    return value != null ? Visibility.Visible : Visibility.Collapsed;
                }
            
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