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

            if (!GlobalData.Instance.IsLoaded)
            {
                if (!GlobalData.Instance.Load())
                {
                    if (MessageBox.Show("It seems you haven't synced up yet :O, let go and do that") == MessageBoxResult.OK)
                    {
                        Sync();
                    }
                }
                else
                {
                    if (GlobalData.Instance.Contacts == null)
                        GlobalData.Instance.OnContactsLoaded += LoadPictures;
                    else
                        LoadPictures();
                }
            }

            if (!localSettings.Contains("DisplayName"))
            {
                GetDisplayName.IsOpen = true;

                if (GlobalData.Instance.Contacts == null)
                    GlobalData.Instance.OnContactsLoaded += Instance_OnContactsLoaded;
                else
                    Instance_OnContactsLoaded();
            }
        }

        void LoadPictures()
        {
            foreach (var p in GlobalData.Instance.People) { p.LoadPhoto(); }
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
            NavigationService.Navigate(new Uri("/Activities.xaml",UriKind.Relative));
        }
        private void POI_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/POIList.xaml", UriKind.Relative));
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings localSettings = IsolatedStorageSettings.ApplicationSettings;
            localSettings["DisplayName"] = DisplayName.Text;
            GetDisplayName.IsOpen = false;
        }

        private bool Sync()
        {
            return GlobalData.Instance.Sync();

        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            if (Sync())
            {
                MessageBox.Show("Sync Completed!");
            }
        }
    }
}