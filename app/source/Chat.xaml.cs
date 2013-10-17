using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Data;
using Microsoft.Phone.Tasks;

namespace VNMC2013
{
    public partial class Chat : PhoneApplicationPage
    {
        public Chat()
        {
            InitializeComponent();

            this.MessageList.ItemsSource = MessageCollection.Instance.Messages;
            MessageCollection.Instance.OnNewMessageEventHandler += AddMessage;
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            MessageList.Focus();
            MessageList.ScrollIntoView(MessageList.Items.Last());
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

        public void AddMessage()
        {
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() => {
                MessageList.ItemsSource = null;
                MessageList.ItemsSource = MessageCollection.Instance.Messages;
                MessageList.Focus();
                MessageList.ScrollIntoView(MessageList.Items.Last());
            });
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            PhotoChooserTask photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += photoChooserTask_Completed;
            photoChooserTask.Show();
        }

        void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            MessageCollection.Instance.AddPhoto(Person.CurrentUser.DisplayName, DateTime.Now, e);
        }
    }
}