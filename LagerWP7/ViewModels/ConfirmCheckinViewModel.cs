using System;
using System.ComponentModel;

namespace LagerWP7 {
    public class ConfirmCheckinViewModel : INotifyPropertyChanged {
        private string _name;
        public string Name {
            get {
                return _name;
            }
            set {
                if (value != _name) {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _brewery;
        public string Brewery {
            get {
                return _brewery;
            }
            set {
                if (value != _brewery) {
                    _brewery = value;
                    NotifyPropertyChanged("Brewery");
                }
            }
        }

        private string _img;
        public string Img {
            get {
                return _img;
            }
            set {
                if (value != _img) {
                    _img = value;
                    NotifyPropertyChanged("Img");
                }
            }
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