using Newtonsoft.Json;
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
    class Person
    {
        private static Person me;
        public static Person Me
        {
            get
            {
                if (me != null) return me;
                return me = Person.All.First(x => x.DisplayName == IsolatedStorageSettings.ApplicationSettings["DisplayName"].ToString());
            }
        }

        private static Person[] all;
        public static Person[] All
        {
            get
            {
                if (all != null) return all;

                string json = string.Empty;
                IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
                StreamResourceInfo resource = Application.GetResourceStream(new Uri("/VNMC2013;component/people.json", UriKind.Relative));

                using (StreamReader reader = new StreamReader(resource.Stream))
                {
                    json += reader.ReadToEnd();
                }
                return all = JsonConvert.DeserializeObject<Person[]>(json);
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

                return image = CreateImageFromStream(
                    GlobalData.Instance.Contacts.First(
                        x => x.CompleteName.FirstName + x.CompleteName.MiddleName + x.CompleteName.LastName == this.DisplayName.Replace(" ", "")
                    ).GetPicture()
                );
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
