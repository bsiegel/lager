using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using UntappdAPI;
using UntappdAPI.DataContracts;


namespace LagerWP7 {
    public class UserViewModel : INotifyPropertyChanged {
        private StatusControl _status;

        public UserViewModel() {
            User = new User();
            Recent = new ObservableCollection<ItemViewModel>();

            if (!String.IsNullOrEmpty(App.ViewModel.UntappdUsername) && !String.IsNullOrEmpty(App.ViewModel.UntappdPassword)) {
                _client = new UntappdClient(App.ViewModel.UntappdUsername, App.ViewModel.UntappdPassword, App.ApiKey);
            } else {
                _client = new UntappdClient(App.ApiKey);
            }
        }

        public void InitClient(StatusControl status) {
            _status = status;

            _client.UserInfoComplete += (sender, e) => {
                var user = e.Result.Results.User;
                user.FirstName = user.FirstName.ToLower();
                user.LastName = user.LastName.ToLower();
                user.Location = user.Location.ToUpper();
                User = user;
                _status.HideProgress();
            };

            _client.UserFeedComplete += (sender, e) => {
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

        private User _user;
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

        public ObservableCollection<ItemViewModel> Recent {
            get;
            private set;
        }

        public void LoadUserInfo(string userName) {
            _status.ShowProgress(2);
            _client.FetchUserInfo(userName);
            _client.FetchUserFeed(userName);
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