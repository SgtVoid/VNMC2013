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
    public partial class ActivityDetails : PhoneApplicationPage
    {
        public ActivityDetails()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.DataContext = Activity.All.FirstOrDefault(x => x.Id == int.Parse(NavigationContext.QueryString["Id"]));
        }
    }
}