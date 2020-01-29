using System;
using ABitNowMobileApp.Interfaces;

namespace ABitNowMobileApp.Messages
{
    public class HideUnhideViewAnimationMessage : IMessage
    {
        public bool IsHiding { get; set; }

        public HideUnhideViewAnimationMessage(bool isHiding)
        {
            IsHiding = isHiding;
        }
    }
}
