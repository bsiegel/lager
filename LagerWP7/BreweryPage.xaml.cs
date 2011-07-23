using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace LagerWP7 {
    public partial class BreweryPage : PhoneApplicationPage, IStatusPage {
        private string _id;

        public BreweryPage() {
            InitializeComponent();

            DataContext = new BreweryViewModel();
            ((BreweryViewModel) DataContext).InitClient(_status);

            Loaded += BreweryPage_Loaded;
        }

        void BreweryPage_Loaded(object sender, RoutedEventArgs e) {
            if (NavigationContext.QueryString["id"].Length > 0) {
                _id = NavigationContext.QueryString["id"];
                ((BreweryViewModel) DataContext).LoadBreweryInfo(_id);
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