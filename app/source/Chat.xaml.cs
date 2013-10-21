using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

using Microsoft.Phone.Shell;
using System.Windows.Data;
using Microsoft.Phone.Tasks;

namespace VNMC2013
{
    public partial class Chat : PhoneApplicationPage
    {
        private int page;

        public Chat()
        {
            page = 1;
            InitializeComponent();

            MessageCollection.Instance.OnNewMessageEventHandler += AddMessage;
            this.MessageList.ItemsSource = MessageCollection.Instance.Messages;
            this.MessageList.Loaded += MessageList_Loaded;
        }

        private void MessageList_Loaded(object sender, RoutedEventArgs e)
        {
            if (MessageCollection.Instance.Messages.Count > 0)
                MessageList.ScrollTo(MessageList.ItemsSource[MessageList.ItemsSource.Count - 1]);
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            MessageList.Focus();
            MessageList.ScrollTo(MessageList.ItemsSource[MessageList.ItemsSource.Count - 1]);
            if (string.IsNullOrEmpty(messageBox.Text)) return;

            MessageCollection.Instance.Add(new Message()
            {
                From = Person.CurrentUser.DisplayName,
                Content = messageBox.Text,
                CreatedAt = DateTime.Now,
                Type = Message.MessageType.Text
            });

            messageBox.Text = "";
        }

        private void AddMessage(bool FirstOrLast = false)
        {
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() => {
                MessageList.ItemsSource = null;
                MessageList.ItemsSource = MessageCollection.Instance.Messages;
                MessageList.Focus();
                int index = FirstOrLast ? 0 : MessageList.ItemsSource.Count - 1;
                MessageList.ScrollTo(MessageList.ItemsSource[index]);
            });
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            PhotoChooserTask photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += photoChooserTask_Completed;
            photoChooserTask.Show();
        }

        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            MessageCollection.Instance.AddPhoto(Person.CurrentUser.DisplayName, DateTime.Now, e);
        }

        private void MoreMessagesIconButton_Click(object sender, EventArgs e)
        {
            MessageCollection.Instance.LoadMessage(++page * MessageCollection.Instance.TakeMessagesPerCall);
        }
    }
}