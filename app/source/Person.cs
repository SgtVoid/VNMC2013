using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace VNMC2013
{
    class Person
    {
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
                        x => x.DisplayName == this.DisplayName
                    ).GetPicture()
                );
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
