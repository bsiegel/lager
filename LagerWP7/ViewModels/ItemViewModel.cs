using System;
using System.ComponentModel;

namespace LagerWP7 {
    public class ItemViewModel : INotifyPropertyChanged {
        private string _lineOne;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineOne {
            get {
                return _lineOne;
            }
            set {
                if (value != _lineOne) {
                    _lineOne = value;
                    NotifyPropertyChanged("LineOne");
                }
            }
        }

        private string _lineTwo;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineTwo {
            get {
                return _lineTwo;
            }
            set {
                if (value != _lineTwo) {
                    _lineTwo = value;
                    NotifyPropertyChanged("LineTwo");
                }
            }
        }

        private string _lineThree;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineThree {
            get {
                return _lineThree;
            }
            set {
                if (value != _lineThree) {
                    _lineThree = value;
                    NotifyPropertyChanged("LineThree");
                }
            }
        }

        private string _lineFour;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineFour {
            get {
                return _lineFour;
            }
            set {
                if (value != _lineFour) {
                    _lineFour = value;
                    NotifyPropertyChanged("LineFour");
                }
            }
        }

        private string _lineFive;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineFive {
            get {
                return _lineFive;
            }
            set {
                if (value != _lineFive) {
                    _lineFive = value;
                    NotifyPropertyChanged("LineFive");
                }
            }
        }

        private string _image;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Image {
            get {
                return _image;
            }
            set {
                if (value != _image) {
                    _image = value;
                    NotifyPropertyChanged("Image");
                }
            }
        }

        private string _idOne;
        public string IdOne {
            get {
                return _idOne;
            }
            set {
                if (value != _idOne) {
                    _idOne = value;
                    NotifyPropertyChanged("IdOne");
                }
            } 
        }

        private string _idTwo;
        public string IdTwo {
            get {
                return _idTwo;
            }
            set {
                if (value != _idTwo) {
                    _idTwo = value;
                    NotifyPropertyChanged("IdTwo");
                }
            }
        }

        private string _idThree;
        public string IdThree {
            get {
                return _idThree;
            }
            set {
                if (value != _idThree) {
                    _idThree = value;
                    NotifyPropertyChanged("IdThree");
                }
            }
        }

        private string _idFour;
        public string IdFour {
            get {
                return _idFour;
            }
            set {
                if (value != _idFour) {
                    _idFour = value;
                    NotifyPropertyChanged("IdFour");
                }
            }
        }

        private string _idFive;
        public string IdFive {
            get {
                return _idFive;
            }
            set {
                if (value != _idFive) {
                    _idFive = value;
                    NotifyPropertyChanged("IdFive");
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