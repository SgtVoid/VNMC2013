﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace VNMC2013
{
    public class Person
    {

        private static Person currentUser;
        public static Person CurrentUser
        {
            get
            {
                if (currentUser != null) return currentUser;

                return currentUser = Person.All.First(x => x.DisplayName == IsolatedStorageSettings.ApplicationSettings["DisplayName"].ToString());
            }
        }

        public static Person[] All
        {
            get
            {
                return GlobalData.Instance.People;
            }
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        private BitmapImage image;
        public BitmapImage Image
        {
            get
            {
                if (image != null) return image;

                try
                {
                    return image = CreateImageFromStream(
                        GlobalData.Instance.Contacts.First(
                            x => string.Equals((x.CompleteName.FirstName + x.CompleteName.MiddleName + x.CompleteName.LastName).Replace(" ", ""), this.DisplayName.Replace(" ", ""), StringComparison.OrdinalIgnoreCase)
                        ).GetPicture()
                    );
                }
                catch
                {
                    return null;
                }
            }
        }

        public int PrimaryActivity { get; set; }
        private Activity activity;
        public Activity Activity
        {
            get
            {
                if (activity != null) return activity;
                return activity = Activity.All.First(x => x.Id == this.PrimaryActivity);
            }
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
    }
}
