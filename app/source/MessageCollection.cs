using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VNMC2013
{
    public class MessageCollection
    {
        public delegate void NewMessageEventHandler(bool FirstOrLast);
        public event NewMessageEventHandler OnNewMessageEventHandler;

        public int TakeMessagesPerCall = 15;
        public bool AllMessagesLoaded = false;

        private static MessageCollection instance;
        public static MessageCollection Instance
        {
            get
            {
                if (instance == null) instance = new MessageCollection();
                return instance;
            }
        }

        public List<Message> Messages { get; set; }

        public MessageCollection()
        {
            Messages = new List<Message>();
            LoadMessage();
        }

        public void AddMessageFromStream(Stream stream)
        {
            string reader = new StreamReader(stream).ReadToEnd();
            dynamic json = JsonConvert.DeserializeObject(reader);

            if (json["From"].Value == Person.CurrentUser.DisplayName) return;

            DateTime date = new DateTime((int)json["Year"].Value, (int)json["Month"].Value, (int)json["Day"].Value, (int)json["Hour"].Value, (int)json["Minute"].Value, (int)json["Second"].Value, DateTimeKind.Utc);
            Message message = new Message
            {
                From = json["From"].Value,
                Content = json["Content"].Value,
                CreatedAt = date.ToLocalTime(),
                Type = json["Type"].Value == "Text" ? Message.MessageType.Text : Message.MessageType.Photo
            };

            message.LoadImage();
            Messages.Add(message);

            if(OnNewMessageEventHandler != null) OnNewMessageEventHandler(false);
        }

        public void Add(Message message)
        {
            message.LoadImage();
            Messages.Add(message);
            App.MobileService.GetTable<Message>().InsertAsync(message);
            if(OnNewMessageEventHandler != null) OnNewMessageEventHandler(false);
        }

        public async void AddPhoto(string From, DateTime CreatedAt, Microsoft.Phone.Tasks.PhotoResult e)
        {
            Message message = new Message
            {
                From = From,
                CreatedAt = CreatedAt,
                Type = Message.MessageType.Photo,
                Content = await DropBox.Client.UploadFile(e.OriginalFileName, e.ChosenPhoto)
            };
            this.Add(message);
        }

        public async void LoadMessage(int skip = 0)
        {
            List<Message> newMessages = await App.MobileService.GetTable<Message>().Skip(skip).Take(TakeMessagesPerCall).OrderByDescending(x => x.Id).ToListAsync();

            AllMessagesLoaded = newMessages.Count < TakeMessagesPerCall;
            if (newMessages.Count == 0) return;

            newMessages.ForEach(x => x.LoadImage());
            Messages.InsertRange(0, newMessages.Reverse<Message>());
            if(OnNewMessageEventHandler != null) OnNewMessageEventHandler(skip != 0);
        }
    }
}
