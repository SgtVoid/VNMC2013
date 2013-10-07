using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Scheduler;

namespace VNMC2013
{
    public partial class Roots : PhoneApplicationPage
    {
        public Roots()
        {
            InitializeComponent();
            acitivityUserControl.DataContext = Person.CurrentUser.Activity.People;

            if (ScheduledActionService.Find("VNMC2013") == null)
                AlarmToggle.IsChecked = false;
            else
                AlarmToggle.IsChecked = true;

            TextBlockActivity.Text = Person.CurrentUser.Activity.Description;
            TextTitle.Text = Person.CurrentUser.Activity.Name;
        }

        private void AlarmToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (ScheduledActionService.Find("VNMC2013") != null) return;

            Alarm alarm = new Alarm("VNMC2013");

            alarm.Content = "Wake up!!!! It is time for " + Person.CurrentUser.Activity.Name;
            alarm.BeginTime = Person.CurrentUser.Activity.AlarmTime;
            alarm.ExpirationTime = Person.CurrentUser.Activity.AlarmTime.AddMinutes(10);
            ScheduledActionService.Add(alarm);
        }

        private void AlarmToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            ScheduledActionService.Remove("VNMC2013");
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Image image = e.OriginalSource as Image;
            BitmapImage img = new BitmapImage();
            img.UriSource = new Uri("/Assets/explosion-64.png", UriKind.Relative);
            image.Source = img;
        }
    }
}