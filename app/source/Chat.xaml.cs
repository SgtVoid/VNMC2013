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

namespace VNMC2013
{
    public partial class Chat : PhoneApplicationPage
    {
        public Chat()
        {
            InitializeComponent();
            MessageList.ItemsSource = MessageCollection.Instance.Messages;
            MessageCollection.Instance.OnNewMessageEventHandler += AddMessage;
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            string content = string.Format("{0} {1}: {2}", DateTime.Now.ToString("d/M hh:mm"), Person.CurrentUser.DisplayName, messageBox.Text);
            MessageCollection.Instance.Add(new Message() { Content = content});

            messageBox.Text = "";
        }

        public void AddMessage()
        {
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() => {
                MessageList.ItemsSource = null;
                MessageList.ItemsSource = MessageCollection.Instance.Messages;
            });
        }
    }
}