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
using System.IO;

namespace VNMC2013
{
    public partial class Roots : PhoneApplicationPage
    {
        public Roots()
        {
            InitializeComponent();

            listSunday.ItemsSource = (from x in Person.Me.Activity.People
                                      select new {
                                          Name = x.DisplayName,
                                          Image = x.Image
                                      }).ToList();
        }
    }
}