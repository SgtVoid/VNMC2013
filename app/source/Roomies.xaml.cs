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
    public partial class Roomies : PhoneApplicationPage
    {
        public Roomies()
        {
            InitializeComponent();

            ListRooms.ItemsSource = Room.All;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/RoomiesSearch.xaml", UriKind.Relative));
        }
    }
}