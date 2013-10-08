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
                    LoginFields.IsOpen = true;
                }
                else
                {
                    if (GlobalData.Instance.Contacts == null)
                        GlobalData.Instance.OnContactsLoaded += LoadPictures;
                    else
                        LoadPictures();
                }
            }
        }

        void LoadPictures()
        {
            foreach (var p in GlobalData.Instance.People) { p.LoadPhoto(); }
        }

        private void Programma_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Programma.xaml",UriKind.Relative));
        }
        private void Roomies_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Roomies.xaml", UriKind.Relative));
        }

        private void Activities_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Activities.xaml",UriKind.Relative));
        }
        private void POI_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/POIList.xaml", UriKind.Relative));
        }

        private void Chat_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Chat.xaml", UriKind.Relative));
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings localSettings = IsolatedStorageSettings.ApplicationSettings;
            localSettings["DisplayName"] = "MACAW\\" + DisplayName.Text;

            DisplayName.IsEnabled = false;
            Password.IsEnabled = false;
            Send.IsEnabled = false;

            GlobalData.Instance.OnSyncCompleted += SyncCompleted;
            GlobalData.Instance.OnUpdateProgress += UpdateProgressBar;
            GlobalData.Instance.OnSyncError += SyncError;
            GlobalData.Instance.Sync("MACAW\\" + DisplayName.Text, Password.Password);
        }

        private void SyncCompleted()
        {
            LoginFields.IsOpen = false;
            progressBar.Value = 0;
            Password.Password = "";
            DisplayName.IsEnabled = true;
            Password.IsEnabled = true;
            Send.IsEnabled = true;
        }

        private void UpdateProgressBar(int percentage)
        {
            progressBar.Value += percentage;
        }

        private void SyncError()
        {
            GlobalData.Instance.OnSyncError -= SyncError;
            MessageBox.Show("Je gebruiksnaam en/of wachtwoord zijn verkeerd in gevuld.");
            DisplayName.IsEnabled = true;
            Password.IsEnabled = true;
            Send.IsEnabled = true;
            progressBar.Value = 0;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            IsolatedStorageSettings localSettings = IsolatedStorageSettings.ApplicationSettings;

            LoginFields.IsOpen = true;
            if (localSettings.Contains("DisplayName"))
                DisplayName.Text = localSettings["DisplayName"].ToString().Split('\\').Last();
        }
    }
}