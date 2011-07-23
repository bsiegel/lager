using System;
using System.ComponentModel;
using UntappdAPI;
using UntappdAPI.DataContracts;

namespace LagerWP7 {
    public class BeerViewModel : INotifyPropertyChanged {
        private StatusControl _status;

        public BeerViewModel() {
            Result = new BeerInfoResult();

            if (!String.IsNullOrEmpty(App.ViewModel.UntappdUsername) && !String.IsNullOrEmpty(App.ViewModel.UntappdPassword)) {
                _client = new UntappdClient(App.ViewModel.UntappdUsername, App.ViewModel.UntappdPassword, App.ApiKey);
                SignedIn = true;
            } else {
                _client = new UntappdClient(App.ApiKey);
            }
        }

        public void InitClient(StatusControl status) {
            _status = status;

            _client.BeerInfoComplete += (sender, e) => {
                var res = e.Result.Results;
                res.Brewery = res.Brewery.ToLower();
                res.Name = res.Name.ToLower();
                res.Style = res.Style.ToLower();
                Result = res;

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

        private BeerInfoResult _result;
        public BeerInfoResult Result {
            get {
                return _result;
            }
            set {
                if (value != _result) {
                    _result = value;
                    NotifyPropertyChanged("Result");
                }
            }
        }

        private bool _signedIn;
        public bool SignedIn {
            get {
                return _signedIn;
            }
            set {
                if (value != _signedIn) {
                    _signedIn = value;
                    NotifyPropertyChanged("SignedIn");
                    NotifyPropertyChanged("SignedOut");
                }
            }
        }

        public bool SignedOut {
            get {
                return !this.SignedIn;
            }
        }

        public void LoadBeerInfo(string beerID) {
            _status.ShowProgress();
            _client.FetchBeerInfo(Convert.ToInt32(beerID));
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