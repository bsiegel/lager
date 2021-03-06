﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UntappdAPI;
using UntappdAPI.DataContracts;


namespace LagerWP7 {
    public class MainViewModel : INotifyPropertyChanged {
        private StatusControl _status;

        public MainViewModel() {
            Trending = new ObservableCollection<ItemViewModel>();
            Friends = new ObservableCollection<ItemViewModel>();
            Recent = new ObservableCollection<ItemViewModel>();
            Results = new ObservableCollection<ItemViewModel>();
        }

        public void InitClient(StatusControl status) {
            _status = status;

            if (!String.IsNullOrEmpty(UntappdUsername) && !String.IsNullOrEmpty(UntappdPassword)) {
                _client = new UntappdClient(UntappdUsername, UntappdPassword, App.ApiKey);
            } else {
                _client = new UntappdClient(App.ApiKey);
            }
            
            _client.TrendingComplete += (sender, e) => {
                Trending.Clear();
                foreach (var result in e.Result.Results) {
                    Trending.Add(new ItemViewModel() {
                        LineOne = result.BeerName.ToLower(),
                        IdOne = result.BeerId.ToString(),
                        LineTwo = result.BreweryName.ToLower(),
                        IdTwo = result.BreweryId
                    });
                }
                _status.HideProgress();
            };
            _client.UserFeedComplete += (sender, e) => {
                if (e.Result.FeedType == "Friend") {
                    Friends.Clear();
                    foreach (var result in e.Result.Results.OrderByDescending(i => DateTime.Parse(i.CreatedAt))) {
                        if (result != null) {
                            Friends.Add(new ItemViewModel() {
                                LineOne = result.User.DisplayName.ToLower(),
                                IdOne = result.User.UserName,
                                LineTwo = result.BeerName.ToLower(),
                                IdTwo = result.BeerId.ToString(),
                                LineThree = result.BreweryName.ToLower(),
                                IdThree = result.BreweryId, 
                                LineFour = result.DisplayCreatedTimeAgo,
                                Image = result.User.AvatarUrl,
                                LineFive = String.IsNullOrEmpty(result.CheckinComment) ? null : result.CheckinComment.ToLower()
                            });
                        }
                    }
                    _status.HideProgress();
                } else if (e.Result.FeedType == "User") {
                    if (e.Result.Results.Length > 0) {
                        // profile
                        Recent.Clear();
                        foreach (var result in e.Result.Results.OrderByDescending(i => DateTime.Parse(i.CreatedAt))) {
                            if (result != null) {
                                Recent.Add(new ItemViewModel() {
                                    LineOne = result.BeerName.ToLower(),
                                    IdOne = result.BeerId.ToString(),
                                    LineTwo = result.BreweryName.ToLower(),
                                    IdTwo = result.BreweryId,
                                    LineThree = result.DisplayCreatedTimeAgo,
                                    LineFour = String.IsNullOrEmpty(result.CheckinComment) ? null : result.CheckinComment.ToLower()
                                });
                            }
                        }
                    }
                    _status.HideProgress();
                }
            };
            _client.UserInfoComplete += (sender, e) => {
                var user = e.Result.Results.User;
                if (user.UserName == UntappdUsername) {
                    user.FirstName = user.FirstName.ToLower();
                    user.LastName = user.LastName.ToLower();
                    user.Location = user.Location.ToUpper();
                    User = user;
                }
                _status.HideProgress();
            };

            _client.BeerSearchResultsComplete += (sender, e) => {
                Results.Clear();
                foreach (var result in e.Result.Results) {
                    Results.Add(new ItemViewModel() {
                        LineOne = result.Name.ToLower(),
                        IdOne = result.BeerId.ToString(),
                        LineTwo = result.BreweryName.ToLower(),
                        IdTwo = result.BreweryId
                    });
                }
                _searchComplete = true;
                NotifyPropertyChanged("NoResults");
                _status.HideProgress();
            };

            _client.RemoteError += (sender, e) => {
                _status.HideProgress();
                _status.ShowError(e.Result.Error != null ? e.Result.Error.Message : e.Result.ErrorMessage);
            };
        }

        private UntappdClient _client;
        public UntappdClient Client {
            get {
                return _client;
            }
        }

        public static ImageBrush PanoramaBackgroundImage {
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

        private bool _searchComplete;

        public bool NoResults {
            get {
                return (_searchComplete && Results.Count == 0);
            }
        }

        private User _user = new User();

        public User User {
            get {
                return _user;
            }
            set {
                if (value != _user) {
                    _user = value;
                    NotifyPropertyChanged("User");
                }
            }
        }

        private string _untappdUserName;
        internal string UntappdUsername {
            get {
                return _untappdUserName;
            }
            set {
                if (value != _untappdUserName) {
                    _untappdUserName = value;
                    NotifyPropertyChanged("UntappdUsername");
                }
            }
        }

        internal string UntappdPassword {
            get;
            set;
        }

        internal bool IsDataLoaded {
            get;
            set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Recent collection.
        /// </summary>
        public void LoadPublicFeeds() {
            _status.ShowProgress();
            _client.FetchTrending();
        }

        public void LoadUserInfo() {
            _status.ShowProgress();
            _client.FetchUserInfo();
        }

        public void LoadPersonalFeeds() {
            _status.ShowProgress(2);
            _client.FetchUserFeed();
            _client.FetchFriendFeed();
        }

        public void LoadResults(string query) {
            _searchComplete = false;
            NotifyPropertyChanged("NoResults");

            _status.ShowProgress();
            _client.FetchBeerSearchResults(query, sortType: UntappdClient.SearchSortType.Count);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}