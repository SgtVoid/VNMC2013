using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNMC2013
{
    public class MessageCollection
    {
        public delegate void NewMessageEventHandler();
        public event NewMessageEventHandler OnNewMessageEventHandler;

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
            LoadMessage();
        }

        public void AddMessageFromStream(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            Messages.Add(new Message() { Content = reader.ReadToEnd() });
            OnNewMessageEventHandler();
        }

        public void Add(Message message)
        {
            App.MobileService.GetTable<Message>().InsertAsync(message);
        }

        private async void LoadMessage()
        {
            Messages = await App.MobileService.GetTable<Message>().ToListAsync();
        }
    }
}
