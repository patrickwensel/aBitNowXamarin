using System;
using ABitNowMobileApp.Interfaces;
using ABitNowMobileApp.UiItemModels;

namespace ABitNowMobileApp.Messages
{
    public class SuggestionRemovedMessage : IMessage
    {
        public Suggestion Suggestion { get; set; }

        public SuggestionRemovedMessage(Suggestion suggestion)
        {
            Suggestion = suggestion;
        }
    }
}
