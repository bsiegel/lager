using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace LagerWP7 {
    public partial class BeerPage : PhoneApplicationPage, IStatusPage {
        private string _id;

        public BeerPage() {
            InitializeComponent();

            DataContext = new BeerViewModel();
            ((BeerViewModel)DataContext).InitClient(_status);

            Loaded += BeerPage_Loaded;
        }

        void BeerPage_Loaded(object sender, RoutedEventArgs e) {
            if (NavigationContext.QueryString["id"].Length > 0) {
                _id = NavigationContext.QueryString["id"];
                ((BeerViewModel) DataContext).LoadBeerInfo(_id);
            }
        }

        private void CheckIn_Click(object sender, RoutedEventArgs e) {
            ((BeerViewModel) DataContext).CheckInToBeer(_id);
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