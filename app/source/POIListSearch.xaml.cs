using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using Microsoft.Phone.UserData;
using System.Collections;
using Windows.Phone.PersonalInformation;
using VNMC2013.Data;

namespace VNMC2013
{
    public partial class POIListSearch : PhoneApplicationPage
    {
        public POIListSearch()
        {
            InitializeComponent();
            lstPOI.ItemsSource = GlobalData.Instance.PointsOfInterest.ToList();
        }

        protected override void OnNavigatedTo(NavigationEventArgs arg)
        {
            lstPOI.SelectedItem = null;
            txtSearch.Focus();
       
        }

        private void lstPOI_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstPOI.SelectedItem == null)
                return;

            int index = GlobalData.Instance.PointsOfInterest.ToList().IndexOf(((POI)lstPOI.SelectedItem));

            NavigationService.Navigate(new Uri("/POIDetail.xaml?id="+index, UriKind.Relative));
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            lstPOI.ItemsSource = (from x in GlobalData.Instance.PointsOfInterest
                                  where x.Name.ToLower().Contains(txtSearch.Text.ToLower())
                                  select x).ToList();
        }

    }
}