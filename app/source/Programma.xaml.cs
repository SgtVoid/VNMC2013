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
    public partial class Roots : PhoneApplicationPage
    {
        public Roots()
        {
            InitializeComponent();

            List<string> test = new List<string>
            {
                "hoi",
                "doei"
            };
            listFriday.ItemsSource = test;
        }
    }
}