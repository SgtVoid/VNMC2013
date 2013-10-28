using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace VNMC2013
{
    public class Message
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string Content { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Picture Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public MessageType Type { get; set; }

        public async void LoadImage()
        {
            if (this.Type != Message.MessageType.Photo) return;

            MediaLibrary library = new MediaLibrary();
            Image = library.SavedPictures.FirstOrDefault(x => x.Name == this.Content);

            if (Image == null) Image = await DropBox.Client.GetFile(this.Content);
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PrintContent
        {
            get
            {
                return string.Format("{0} {1}: {2}", CreatedAt.ToString("d/M HH:mm"), From, Content);
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BitmapImage PrintImage
        {
            get
            {
                if (Image == null) return null;
                BitmapImage img = new BitmapImage();
                img.SetSource(Image.GetImage());
                return img;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DateFrom
        {
            get
            {
                return string.Format("{0} {1}: ", CreatedAt.ToString("d/M HH:mm"), From);
            }
        }

        public enum MessageType
        {
            Text,
            Photo
        }
    }
}