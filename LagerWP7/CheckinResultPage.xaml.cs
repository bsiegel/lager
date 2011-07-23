using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace LagerWP7 {
    public partial class CheckinResultPage : PhoneApplicationPage {
        public string BeerName;
        public string BreweryName;

        public CheckinResultPage() {
            InitializeComponent();

            DataContext = this;

            Loaded += CheckinResultPage_Loaded;
        }

        void CheckinResultPage_Loaded(object sender, RoutedEventArgs e) {
            if (NavigationContext.QueryString["beer"].Length > 0) {
                BeerName = NavigationContext.QueryString["beer"];
            }
            if (NavigationContext.QueryString["brewery"].Length > 0) {
                BreweryName = NavigationContext.QueryString["brewery"];
            }
        }
    }
}