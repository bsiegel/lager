using System;
using System.ComponentModel;
using System.Windows;
using LagerWP7.Utility;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using ShakeGestures;
using UntappdAPI;
using UntappdAPI.DataContracts;

namespace LagerWP7 {
    public partial class MainPage : PhoneApplicationPage, IStatusPage {
        // Constructor
        public MainPage() {
            InitializeComponent();

            DataContext = App.ViewModel;
            App.ViewModel.InitClient(_status);

            Loaded += MainPage_Loaded;
            BackKeyPress += MainPage_BackKeyPress;
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e) {
            if (!App.ViewModel.IsDataLoaded) {
                ShakeGesturesHelper.Instance.ShakeGesture += (s, ea) => {
                    Dispatcher.BeginInvoke(() => {
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
                    App.ViewModel.InitClient(_status);

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
            _status.ShowProgress();
            var client = new UntappdClient(_loginUsername.Text, _loginPassword.Password, App.ApiKey);
            client.CheckinComplete += (s, ea) => {
                _status.HideProgress();
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

                    App.ViewModel.InitClient(_status);

                    App.ViewModel.LoadUserInfo();
                    App.ViewModel.LoadPersonalFeeds();
                }
            };
            client.RemoteError += (s, ea) => {
                _status.HideProgress();
                _loginFailure.Text = ea.Result.ErrorMessage;
                _loginFailure.Visibility = Visibility.Visible;
            };

            client.CheckInBeer(1, -5, null, null, null, null, false, false, false, true);
        }

        private void MainPage_BackKeyPress(object sender, CancelEventArgs e) {
            if (_contextMenu.IsOpen) {
                _contextMenu.IsOpen = false; // Close menu
                e.Cancel = true; // Cancel Navigation
            }
        }

        public void SignOut_Click(object sender, RoutedEventArgs e) {
            _loginFailure.Visibility = Visibility.Collapsed;
            _loginFailure.Text = "";

            App.ViewModel.UntappdUsername = null;
            App.ViewModel.UntappdPassword = null;

            CredentialUtility.DeleteCredentials();

            _friendsList.Visibility = Visibility.Collapsed;
            _profileGrid.Visibility = Visibility.Collapsed;
            _needLoginBlock.Visibility = Visibility.Visible;
            _loginGrid.Visibility = Visibility.Visible;

            App.ViewModel.User = new User();
            App.ViewModel.Friends.Clear();
            App.ViewModel.Recent.Clear();

            App.ViewModel.InitClient(_status);
        }

        public void About_Click(object sender, RoutedEventArgs e) {
            var task = new WebBrowserTask() {
                URL = "https://bitbucket.org/bsiegel/lager"
            };
            task.Show();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e) {
            App.ViewModel.LoadResults(_searchBox.Text);
        }

        private void User_Click(object sender, RoutedEventArgs e) {
            var s = sender as FrameworkElement;
            if (s != null && s.Tag != null) {
                NavigationService.Navigate(new Uri(string.Format("/UserPage.xaml?id={0}", s.Tag), UriKind.Relative));
            }
        }

        private void Beer_Click(object sender, RoutedEventArgs e) {
            var s = sender as FrameworkElement;
            if (s != null && s.Tag != null) {
                NavigationService.Navigate(new Uri(string.Format("/BeerPage.xaml?id={0}", s.Tag), UriKind.Relative));
            }
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