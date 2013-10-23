using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace VNMC2013
{
    public partial class RoomiesSearch : PhoneApplicationPage
    {
        public RoomiesSearch()
        {
            InitializeComponent();
            this.Loaded += ((object s, RoutedEventArgs e) => txtSearch.Focus());

            lstRoomies.ItemsSource = Room.All;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            lstRoomies.ItemsSource = (from x in Room.All
                                      where (x.Person1 != null && x.Person1.DisplayName.ToLower().Contains(txtSearch.Text.ToLower())) || 
                                      (x.Person2 != null && x.Person2.DisplayName.ToLower().Contains(txtSearch.Text.ToLower()))
                                      select x).ToList();
        }
    }
}