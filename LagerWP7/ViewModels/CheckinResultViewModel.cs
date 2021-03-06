﻿using System;
using System.ComponentModel;
using UntappdAPI;
using UntappdAPI.DataContracts;

namespace LagerWP7 {
    public class CheckinResultViewModel : INotifyPropertyChanged {
        private StatusControl _status;

        public CheckinResultViewModel() {
            Result = new CheckinResponse();

            if (!String.IsNullOrEmpty(App.ViewModel.UntappdUsername) && !String.IsNullOrEmpty(App.ViewModel.UntappdPassword)) {
                _client = new UntappdClient(App.ViewModel.UntappdUsername, App.ViewModel.UntappdPassword, App.ApiKey);
            } else {
                _client = new UntappdClient(App.ApiKey);
            }
        }

        public void InitClient(StatusControl status) {
            _status = status;

            _client.CheckinComplete += (sender, e) => {
                var res = e.Result;
                res.BeerDetails.Name = res.BeerDetails.Name.ToLowerInvariant();
                res.BeerDetails.Brewery = res.BeerDetails.Brewery.ToLowerInvariant();

                foreach (var badge in res.Badges) {
                    badge.Name = badge.Name.ToLowerInvariant();
                    badge.Description = badge.Description.ToLowerInvariant();
                }

                foreach (var rec in res.Recommendations) {
                    rec.Name = rec.Name.ToLowerInvariant();
                    rec.BreweryName = rec.BreweryName.ToLowerInvariant();
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

        private CheckinResponse _result;
        public CheckinResponse Result {
            get {
                return _result;
            }
            set {
                if (value != _result) {
                    _result = value;
                    NotifyPropertyChanged("Result");
                    NotifyPropertyChanged("PageTitle");
                    NotifyPropertyChanged("HasBadges");
                }
            }
        }

        public string PageTitle {
            get {
                if (Result == null || String.IsNullOrEmpty(Result.Result))
                    return "check-in in progress";
                else if (Result.Result.ToLowerInvariant() != "success")
                    return "check-in unsuccessful";
                else
                    return "check-in successful";
            }
        }

        public bool HasBadges {
            get {
                return Result != null && Result.Badges != null && Result.Badges.Length > 0;
            }
        }

        public void CheckInToBeer(string beerID, string comment, int? rating) {
            _status.ShowProgress();

#if DEBUG
            var testOnly = true;
#else
            var testOnly = false;
#endif

            _client.CheckInBeer(Convert.ToInt32(beerID), TimeZoneInfo.Local.BaseUtcOffset.TotalHours, null, null, null, comment, rating, false, false, false, testOnly);
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