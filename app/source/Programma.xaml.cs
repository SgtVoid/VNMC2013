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

            List<string> test = new List<string>
            {
                "hoi",
                "doei"
            };
            listFriday.ItemsSource = (from x in GlobalData.Instance.Contacts
                                     // where x.DisplayName == "Remco Brilstra"
                                      select new
                                      {
                                          Contact = x,
                                          Name = x.DisplayName,
                                          Image = CreateImageFromStream(x.GetPicture())
                                      }).ToList();

           // ((ImageBrush)panoramathingy.Background).ImageSource = ((dynamic)listFriday.ItemsSource[0]).Image;
        }

        private BitmapImage CreateImageFromStream(Stream stream)
        {
            if (stream != null)
            {
                BitmapImage img = new BitmapImage();
                img.SetSource(stream);
                return img;
            }
            return null;
        }

        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            
        }
    }
}