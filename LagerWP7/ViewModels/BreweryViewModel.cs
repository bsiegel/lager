using System;
using System.ComponentModel;
using UntappdAPI;
using UntappdAPI.DataContracts;


namespace LagerWP7 {
    public class BreweryViewModel : INotifyPropertyChanged {
        private StatusControl _status;

        public BreweryViewModel() {
            Result = new BreweryInfoResult();

            if (!String.IsNullOrEmpty(App.ViewModel.UntappdUsername) && !String.IsNullOrEmpty(App.ViewModel.UntappdPassword)) {
                _client = new UntappdClient(App.ViewModel.UntappdUsername, App.ViewModel.UntappdPassword, App.ApiKey);
            } else {
                _client = new UntappdClient(App.ApiKey);
            }
        }

        public void InitClient(StatusControl status) {
            _status = status;

            _client.BreweryInfoComplete += (sender, e) => {
                var res = e.Result.Results;
                res.Country = res.Country.ToUpper();
                res.Name = res.Name.ToLower();
                res.TwitterHandle = String.IsNullOrEmpty(res.TwitterHandle) ? null : "Twitter: @" + res.TwitterHandle.ToLower();
                foreach (var beer in res.TopBeers) {
                    beer.Name = beer.Name.ToLower();
                    beer.Style = beer.Style.ToLower();
                }
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

        private BreweryInfoResult _result;
        public  BreweryInfoResult Result {
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

        public void LoadBreweryInfo(string breweryID) {
            _status.ShowProgress();
            _client.FetchBreweryInfo(Convert.ToInt32(breweryID));
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