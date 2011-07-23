using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Text.RegularExpressions;

namespace LagerWP7 {
    public partial class BeerPage : PhoneApplicationPage, IStatusPage {
        private string _id;

        public BeerPage() {
            InitializeComponent();

            DataContext = new BeerViewModel();
            ((BeerViewModel) DataContext).InitClient(_status);

            Loaded += BeerPage_Loaded;
        }

        void BeerPage_Loaded(object sender, RoutedEventArgs e) {
            if (NavigationContext.QueryString["id"].Length > 0) {
                _id = NavigationContext.QueryString["id"];
                ((BeerViewModel) DataContext).LoadBeerInfo(_id);
            }
        }

        private void CheckIn_Click(object sender, RoutedEventArgs e) {
            var r = ((BeerViewModel) DataContext).Result;
            NavigationService.Navigate(new Uri(string.Format("/ConfirmCheckinPage.xaml?id={0}&n={1}&b={2}&i={3}", _id, Uri.EscapeDataString(r.Name), Uri.EscapeDataString(r.Brewery), Uri.EscapeDataString(r.Img)), UriKind.Relative));
        }

        private void Brewery_Click(object sender, RoutedEventArgs e) {
            var s = sender as FrameworkElement;
            if (s != null && s.Tag != null) {
                NavigationService.Navigate(new Uri(string.Format("/BreweryPage.xaml?id={0}", s.Tag), UriKind.Relative));
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