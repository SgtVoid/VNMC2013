
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace VNMC2013
{
    [DataContract]
    public class Person : INotifyPropertyChanged
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

        [DataMember(Name = "Id")]
        public int Id { get; set; }
        [DataMember(Name = "FirstName")]
        public string FirstName { get; set; }
        [DataMember(Name = "LastName")]
        public string LastName { get; set; }
        [DataMember(Name = "Account")]
        public string AccountName { get; set; }

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
                return image != null ? image : null;
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

        public async void LoadPhoto()
        {
            Stream stream = await Task<Stream>.Factory.StartNew(() =>
                {
                    try
                    {
                        return GlobalData.Instance.Contacts.First(
                            x => string.Equals((x.CompleteName.FirstName + x.CompleteName.MiddleName + x.CompleteName.LastName).Replace(" ", ""),
                                  this.DisplayName.Replace(" ", ""), StringComparison.OrdinalIgnoreCase)
                        ).GetPicture();
                    }
                    catch { return null; }
                });

            this.image = CreateImageFromStream(stream);
            RaisePropertyChanged("Image");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
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
