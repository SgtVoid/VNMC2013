using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace VNMC2013
{
    public partial class Contact : PhoneApplicationPage
    {
        public Contact()
        {
            InitializeComponent();
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Microsoft.Phone.Tasks.PhoneCallTask task = new Microsoft.Phone.Tasks.PhoneCallTask();
            task.PhoneNumber = "+97148850999";
            task.DisplayName = "Premier Inn Dubai Investments Park";
            task.Show();
        }

        private void callBeachClub_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Microsoft.Phone.Tasks.PhoneCallTask task = new Microsoft.Phone.Tasks.PhoneCallTask();
            task.PhoneNumber = "+97144333777";
            task.DisplayName = "Meydan Beach Club";
            task.Show();

        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Microsoft.Phone.Tasks.MapsDirectionsTask directions = new Microsoft.Phone.Tasks.MapsDirectionsTask();
            directions.End = new LabeledMapLocation("Meydan Beach Club", new System.Device.Location.GeoCoordinate(25.081153, 55.136013));
            directions.Show();
        }
    }
}