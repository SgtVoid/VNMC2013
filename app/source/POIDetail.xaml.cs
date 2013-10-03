using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using VNMC2013.Data;
using Microsoft.Phone.Tasks;

namespace VNMC2013
{
    public partial class POIDetail : PhoneApplicationPage
    {
        public POIDetail()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs arg)
        {
            int id = Convert.ToInt32(NavigationContext.QueryString["id"]);

            if (id >= 0 &&
                id < GlobalData.Instance.PointsOfInterest.Length)
            {

                this.DataContext = GlobalData.Instance.PointsOfInterest[id];

            }
        }

        private void callBeachClub_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            POI current = ((POI)this.DataContext);

            Microsoft.Phone.Tasks.PhoneCallTask task = new Microsoft.Phone.Tasks.PhoneCallTask();
            task.PhoneNumber = current.Phone;
            task.DisplayName = current.Name;
            task.Show();
        }

        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            POI current = ((POI)this.DataContext);

            Microsoft.Phone.Tasks.MapsDirectionsTask directions = new Microsoft.Phone.Tasks.MapsDirectionsTask();
            directions.End = new LabeledMapLocation(current.Name, new System.Device.Location.GeoCoordinate(current.GeoLat, current.GeoLong));
            directions.Show();
        }

    }
}