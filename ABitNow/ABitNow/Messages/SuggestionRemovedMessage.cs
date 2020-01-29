using ABitNow.Contracts.Services.Data;
using ABitNow.Models.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABitNow.Messages
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
