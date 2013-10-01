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

namespace VNMC2013
{
    public partial class Root : PhoneApplicationPage
    {
        public Root()
        {
            InitializeComponent();
            IsolatedStorageSettings localSettings = IsolatedStorageSettings.ApplicationSettings;

            if (!localSettings.Contains("DisplayName"))
            {
                GetDisplayName.IsOpen = true;

                if (GlobalData.Instance.Contacts == null)
                    GlobalData.Instance.OnContactsLoaded += Instance_OnContactsLoaded;
                else
                    Instance_OnContactsLoaded();
            }
        }

        void Instance_OnContactsLoaded()
        {
            DisplayName.ItemsSource = GlobalData.Instance.Contacts.Select(x => x.DisplayName);
        }

        private void Programma_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Programma.xaml",UriKind.Relative));
        }
        private void Roomies_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Roomies.xaml", UriKind.Relative));
        }

        private void Poi_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show("hoi");
        }
        private void Contact_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Contact.xaml", UriKind.Relative));
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings localSettings = IsolatedStorageSettings.ApplicationSettings;
            localSettings["DisplayName"] = DisplayName.Text;
            GetDisplayName.IsOpen = false;
        }
    }
}