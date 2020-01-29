using System;
using ABitNowMobileApp.Enums;
using ABitNowMobileApp.Interfaces;

namespace ABitNowMobileApp.Messages
{
    public class AppLifecycleChangedMessage : IMessage
    {
        public AppLifecycleChangedMessage(AppLifecycleState lifecyleState)
        {
            CurrentLifecyleState = lifecyleState;
        }

        public AppLifecycleState CurrentLifecyleState { get; set; }
    }
}
