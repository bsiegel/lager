using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LagerWP7.Utility;
using Microsoft.Phone.Controls;
using UntappdAPI;
using ShakeGestures;
using System;
using System.Windows.Threading;
using System.Windows.Data;
using System.Globalization;
using Microsoft.Phone.Tasks;

namespace LagerWP7 {
    public partial class MainPage : PhoneApplicationPage {
        // Constructor
        public MainPage() {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }



        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e) {
            if (!App.ViewModel.IsDataLoaded) {
                ShakeGesturesHelper.Instance.ShakeGesture += (s, ea) => {
                    this.Dispatcher.BeginInvoke(() => {
                        //load public feeds
                        App.ViewModel.LoadPublicFeeds();

                        if (!String.IsNullOrEmpty(App.ViewModel.UntappdUsername) && !String.IsNullOrEmpty(App.ViewModel.UntappdPassword)) {
                            //load personal feeds
                            App.ViewModel.LoadPersonalFeeds();
                        }
                    });
                };

                ShakeGesturesHelper.Instance.Active = true;

                //load public feeds
                App.ViewModel.LoadPublicFeeds();

                //rehydrate credentials
                if (CredentialUtility.LoadCredentials()) {
                    //show profile screen
                    _loginGrid.Visibility = Visibility.Collapsed;
                    _needLoginBlock.Visibility = Visibility.Collapsed;
                    _profileGrid.Visibility = Visibility.Visible;
                    _friendsList.Visibility = Visibility.Visible;

                    //re-initialize client with credentials
                    App.ViewModel.InitClient();

                    //load user info
                    App.ViewModel.LoadUserInfo();

                    //load personal feeds
                    App.ViewModel.LoadPersonalFeeds();
                } else {
                    //show login screen
                    _profileGrid.Visibility = Visibility.Collapsed;
                    _friendsList.Visibility = Visibility.Collapsed;
                    _loginGrid.Visibility = Visibility.Visible;
                    _needLoginBlock.Visibility = Visibility.Visible;
                }

                App.ViewModel.IsDataLoaded = true;
            }
        }

        private void SignIn_Click(object sender, RoutedEventArgs e) {
            App.ViewModel.ShowProgress();
            var client = new UntappdClient(_loginUsername.Text, _loginPassword.Password, App.ViewModel.ApiKey);
            client.CheckinComplete += (s, ea) => {
                App.ViewModel.HideProgress();
                if (ea.Result.HttpCode != 200) {
                    _loginFailure.Text = ea.Result.ErrorMessage;
                    _loginFailure.Visibility = Visibility.Visible;
                } else {
                    _loginFailure.Visibility = Visibility.Collapsed;
                    _loginFailure.Text = "";

                    App.ViewModel.UntappdUsername = _loginUsername.Text;
                    App.ViewModel.UntappdPassword = _loginPassword.Password;

                    CredentialUtility.StoreCredentials();

                    _loginGrid.Visibility = Visibility.Collapsed;
                    _needLoginBlock.Visibility = Visibility.Collapsed;
                    _profileGrid.Visibility = Visibility.Visible;
                    _friendsList.Visibility = Visibility.Visible;

                    App.ViewModel.InitClient();

                    App.ViewModel.LoadUserInfo();
                    App.ViewModel.LoadPersonalFeeds();
                }
            };
            client.RemoteError += (s, ea) => {
                App.ViewModel.HideProgress();
                _loginFailure.Text = ea.Result.ErrorMessage;
                _loginFailure.Visibility = Visibility.Visible;
            };

            client.CheckInBeer(1, -5, null, null, null, null, false, false, false, true);
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e) {
            var task = new WebBrowserTask();
            task.URL = "https://bitbucket.org/bsiegel/lager";
            task.Show();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e) {
            App.ViewModel.LoadResults(_searchBox.Text);
        }
    }

    public class VisibilityConverter : IValueConverter {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture) {
            bool visibility = (bool) value;
            return visibility ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture) {
            Visibility visibility = (Visibility) value;
            return (visibility == Visibility.Visible);
        }
    }
}