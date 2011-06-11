using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UntappdAPI;
using UntappdAPI.DataContracts;
using System.Windows.Threading;
using System.Windows.Controls;


namespace LagerWP7 {
    public class MainViewModel : INotifyPropertyChanged {
        public MainViewModel() {
            this.Trending = new ObservableCollection<ItemViewModel>();
            this.Friends = new ObservableCollection<ItemViewModel>();
            this.Recent = new ObservableCollection<ItemViewModel>();
            this.Results = new ObservableCollection<ItemViewModel>();

            this._timer = new DispatcherTimer {
                Interval = TimeSpan.FromSeconds(5)
            };

            _timer.Tick += (o, e) => {
                _timer.Stop();

                ErrorText = null;

                NotifyPropertyChanged("Loading");
                NotifyPropertyChanged("ErrorText");
                NotifyPropertyChanged("ErrorStatus");
            };

            InitClient();
        }

        public void InitClient() {
            if (!String.IsNullOrEmpty(UntappdUsername) && !String.IsNullOrEmpty(UntappdPassword)) {
                _client = new UntappdClient(UntappdUsername, UntappdPassword, ApiKey);
            } else {
                _client = new UntappdClient(ApiKey);
            }
            
            _client.TrendingComplete += (sender, e) => {
                this.Trending.Clear();
                foreach (var result in e.Result.Results) {
                    this.Trending.Add(new ItemViewModel() {
                        LineOne = result.BeerName,
                        LineTwo = result.BreweryName
                    });
                }
                HideProgress();
            };
            _client.UserFeedComplete += (sender, e) => {
                if (e.Result.FeedType == "Friend") {
                    //var results = (from r in e.Result.Results
                    //              group r by r.User.UserName into g
                    //              select g.OrderByDescending(i => DateTime.Parse(i.CreatedAt)).First()).ToList();
                    //if (results.Count < 5) {
                    //    e.Result.
                    //}
                    this.Friends.Clear();
                    foreach (var result in e.Result.Results.OrderByDescending(i => DateTime.Parse(i.CreatedAt))) {
                        if (result != null) {
                            this.Friends.Add(new ItemViewModel() {
                                LineOne = result.User.DisplayName,
                                LineTwo = String.Format("is enjoying a{0} {1} by {2}", _vowels.Contains(result.BeerName[0]) ? "n" : "", result.BeerName, result.BreweryName),
                                LineThree = result.DisplayCreatedTimeAgo,
                                Image = result.User.AvatarUrl
                            });
                        }
                    }
                    HideProgress();
                } else if (e.Result.FeedType == "User") {
                    var first = e.Result.Results.First();
                    if (first.User.UserName == UntappdUsername) {
                        // profile
                        this.Recent.Clear();
                        foreach (var result in e.Result.Results.OrderByDescending(i => DateTime.Parse(i.CreatedAt))) {
                            if (result != null) {
                                this.Recent.Add(new ItemViewModel() {
                                    LineOne = result.BeerName,
                                    LineTwo = result.BreweryName,
                                    LineThree = result.DisplayCreatedTimeAgo
                                });
                            }
                        }
                    }
                    HideProgress();
                }
            };
            _client.UserInfoComplete += (sender, e) => {
                var user = e.Result.Results.User;
                if (user.UserName == UntappdUsername) {
                    // profile
                    ProfileName = user.DisplayName;
                    ProfileCity = user.Location;
                    ProfileImage = user.AvatarUrl;
                }
                HideProgress();
            };

            _client.BeerSearchResultsComplete += (sender, e) => {
                this.Results.Clear();
                foreach (var result in e.Result.Results) {
                    this.Results.Add(new ItemViewModel() {
                        LineOne = result.Name,
                        LineTwo = result.BreweryName
                    });
                }
                HideProgress();
            };

            _client.RemoteError += (sender, e) => {
                HideProgress();
                ShowError(e.Result.Error != null ? e.Result.Error.Message : e.Result.ErrorMessage);
            };
        }

        private UntappdClient _client;
        private List<char> _vowels = new List<char> { 'a', 'e', 'i', 'o', 'u' };

        public ImageBrush PanoramaBackgroundImage {
            get {
                var url = App.LightThemeEnabled ? "PanoramaBackgroundLight.png" : "PanoramaBackgroundDark.png";
                var brush = new ImageBrush {
                    ImageSource = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute))
                };
                return brush;
            }
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Trending {
            get;
            private set;
        }

        public ObservableCollection<ItemViewModel> Friends {
            get;
            private set;
        }

        public ObservableCollection<ItemViewModel> Recent {
            get;
            private set;
        }

        public ObservableCollection<ItemViewModel> Results {
            get;
            private set;
        }

        private string _profileName = "Untappd User";
        private string _profileCity = "Beertopia, USA";
        private string _profileImage = "SampleData/avatar.png";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string ProfileName {
            get {
                return _profileName;
            }
            set {
                if (value != _profileName) {
                    _profileName = value;
                    NotifyPropertyChanged("ProfileName");
                }
            }
        }

        public string ProfileCity {
            get {
                return _profileCity;
            }
            set {
                if (value != _profileCity) {
                    _profileCity = value;
                    NotifyPropertyChanged("ProfileCity");
                }
            }
        }

        public string ProfileImage {
            get {
                return _profileImage;
            }
            set {
                if (value != _profileImage) {
                    _profileImage = value;
                    NotifyPropertyChanged("ProfileImage");
                }
            }
        }

        private int _loadCount = 0;
        private DispatcherTimer _timer;

        public void ShowProgress(int count = 1) {
            if (_loadCount <= 0) {
                _loadCount = 0;
            }

            _loadCount += count;

            NotifyPropertyChanged("Loading");
        }

        public void HideProgress(int count = 1) {
            _loadCount -= count;

            if (_loadCount <= 0) {
                _loadCount = 0;
            }

            NotifyPropertyChanged("Loading");
        }

        public bool Loading {
            get {
                return ErrorStatus ? false : _loadCount > 0;
            }
        }

        public void ShowError(string errorText) {
            if (ErrorText == null) {

                ErrorText = errorText;

                //schedule timer
                _timer.Start();

                NotifyPropertyChanged("Loading");
                NotifyPropertyChanged("ErrorText");
                NotifyPropertyChanged("ErrorStatus");
            }
        }

        public bool ErrorStatus {
            get {
                return ErrorText != null;
            }
        }

        public string ErrorText {
            get;
            set;
        }

        internal bool IsDataLoaded {
            get;
            set;
        }

        internal string UntappdUsername {
            get;
            set;
        }

        internal string UntappdPassword {
            get;
            set;
        }

        internal readonly string ApiKey = "26fd7397ef4a8a0390b465c0b74ea073";

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Recent collection.
        /// </summary>
        public void LoadPublicFeeds() {
            ShowProgress();
            _client.FetchTrending();
        }

        public void LoadUserInfo() {
            ShowProgress();
            _client.FetchUserInfo();
        }

        public void LoadPersonalFeeds() {
            ShowProgress(2);
            _client.FetchUserFeed();
            _client.FetchFriendFeed();
        }

        public void LoadResults(string query) {
            ShowProgress();
            _client.FetchBeerSearchResults(query);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    class DistinctUserFeedComparer : IEqualityComparer<UserFeedResults> {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(UserFeedResults x, UserFeedResults y) {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y))
                return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x.User, null) || Object.ReferenceEquals(y.User, null))
                return false;

            return x.User.UserName == y.User.UserName;
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(UserFeedResults result) {
            //Check whether the object is null
            if (Object.ReferenceEquals(result, null))
                return 0;

            if (Object.ReferenceEquals(result.User, null))
                return 0;

            //Get hash code for the Name field if it is not null.
            return result.User.UserName.GetHashCode();
        }
    }
}