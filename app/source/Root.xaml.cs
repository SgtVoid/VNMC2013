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
    public partial class Root : PhoneApplicationPage
    {
        public Root()
        {
            InitializeComponent();
        }

        private void Programma_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Programma.xaml",UriKind.Relative));
        }
        private void Roomies_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show("hoi");
        }
        private void Poi_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show("hoi");
        }
        private void Contact_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Contact.xaml", UriKind.Relative));
        }

    }
}