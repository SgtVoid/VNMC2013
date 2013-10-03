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
    public partial class Activiteits : PhoneApplicationPage
    {
        public Activiteits()
        {
            InitializeComponent();
            ListActiviteits.ItemsSource = Activity.All;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.ListActiviteits.SelectedItem = null;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListActiviteits.SelectedItem == null)
                return;

            NavigationService.Navigate(new Uri("/ActivityDetails.xaml?Id=" + ((Activity)this.ListActiviteits.SelectedItem).Id, UriKind.Relative));
        }
    }
}