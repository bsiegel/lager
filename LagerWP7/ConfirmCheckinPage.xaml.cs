﻿using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace LagerWP7 {
    public partial class ConfirmCheckinPage : PhoneApplicationPage {
        private string _id;

        public ConfirmCheckinPage() {
            InitializeComponent();

            ConfirmButton.IsEnabled = true;
            DataContext = new ConfirmCheckinViewModel();

            Loaded += ConfirmCheckinPage_Loaded;
        }

        void ConfirmCheckinPage_Loaded(object sender, RoutedEventArgs e) {
            if (NavigationContext.QueryString["id"].Length > 0) {
                _id = NavigationContext.QueryString["id"];
            }
            if (NavigationContext.QueryString["n"].Length > 0) {
                ((ConfirmCheckinViewModel) DataContext).Name = Uri.UnescapeDataString(NavigationContext.QueryString["n"]);
            }
            if (NavigationContext.QueryString["b"].Length > 0) {
                ((ConfirmCheckinViewModel) DataContext).Brewery = Uri.UnescapeDataString(NavigationContext.QueryString["b"]);
            }
            if (NavigationContext.QueryString["i"].Length > 0) {
                ((ConfirmCheckinViewModel) DataContext).Img = Uri.UnescapeDataString(NavigationContext.QueryString["i"]);
            }
        }

        private void Checkin_Click(object sender, RoutedEventArgs e) {
            var commentFragment = !String.IsNullOrEmpty(Comment.Text) ? "&comment=" + Uri.EscapeDataString(Comment.Text) : "";
            var ratingFragment = (Rating.Value.HasValue && Rating.Value > 0) ? "&rating=" + Convert.ToInt32((Rating.Value.Value * 5)) : "";
            ConfirmButton.IsEnabled = false;
            NavigationService.Navigate(new Uri(string.Format("/CheckinResultPage.xaml?id={0}{1}{2}", _id, commentFragment, ratingFragment), UriKind.Relative));
        }
    }
}