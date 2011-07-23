using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace LagerWP7 {
    public partial class CheckinResultPage : PhoneApplicationPage, IStatusPage {
        private string _id;
        private string _comment;
        private bool _inited;

        public CheckinResultPage() {
            InitializeComponent();

            DataContext = new CheckinResultViewModel();
            ((CheckinResultViewModel) DataContext).InitClient(_status);

            Loaded += CheckinResultPage_Loaded;
        }

        void CheckinResultPage_Loaded(object sender, RoutedEventArgs e) {
            if (NavigationContext.QueryString["id"].Length > 0) {
                _id = NavigationContext.QueryString["id"];

                if (NavigationContext.QueryString.ContainsKey("comment") && NavigationContext.QueryString["comment"].Length > 0) {
                    _comment = Uri.UnescapeDataString(NavigationContext.QueryString["comment"]);
                }

                if (!_inited) {
                    ((CheckinResultViewModel) DataContext).CheckInToBeer(_id, _comment);
                    _inited = true;
                }
            }
        }

        private void Brewery_Click(object sender, RoutedEventArgs e) {
            var s = sender as FrameworkElement;
            if (s != null && s.Tag != null) {
                NavigationService.Navigate(new Uri(string.Format("/BreweryPage.xaml?id={0}", s.Tag), UriKind.Relative));
            }
        }

        private void Beer_Click(object sender, RoutedEventArgs e) {
            var s = sender as FrameworkElement;
            if (s != null && s.Tag != null) {
                NavigationService.Navigate(new Uri(string.Format("/BeerPage.xaml?id={0}", s.Tag), UriKind.Relative));
            }
        }

        #region IStatusPage Members

        public StatusControl Status {
            get {
                return _status;
            }
        }

        #endregion

    }
}