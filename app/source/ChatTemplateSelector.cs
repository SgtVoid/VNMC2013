using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VNMC2013
{
    public class ChatTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Photo { get; set; }
        public DataTemplate Text { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Message message = item as Message;
            if (message != null)
            {
                return (message.Type == Message.MessageType.Text) ? Text : Photo;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
