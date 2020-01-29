using ABitNow.Contracts.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ABitNow.Services.General
{
    public class MessagingService : IMessagingService
    {
        public void Send<TMessage>(TMessage message, object sender = null) where TMessage : IMessage
        {
            if (sender == null)
                sender = new object();

            MessagingCenter.Send(sender, message.GetType().FullName, message);
        }

        public void Subscribe<TMessage>(object subscriber, Action<object, TMessage> callback) where TMessage : IMessage
        {
            MessagingCenter.Subscribe(subscriber, typeof(TMessage).FullName, callback);
        }

        public void Unsubscribe<TMessage>(object subscriber) where TMessage : IMessage
        {
            MessagingCenter.Unsubscribe<object, TMessage>(subscriber, typeof(TMessage).FullName);
        }
    }
}
