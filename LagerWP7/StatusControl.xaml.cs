using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Threading;

namespace LagerWP7 {
    public partial class StatusControl : UserControl, INotifyPropertyChanged {
        public StatusControl() {
            InitializeComponent();

            this.DataContext = this;

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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
