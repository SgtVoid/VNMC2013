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
    public partial class Roomies : PhoneApplicationPage
    {
        public Roomies()
        {
            InitializeComponent();

            ListRooms.ItemsSource = (from x in Room.All()
                select new 
            	{
                    Image1 = x.People[0] == null ? null : x.People[0].Image,
            	    Image2 = x.People[1] == null ? null : x.People[1].Image
                }).ToList();
        }
    }
}