using System;
using System.Collections.Generic;
using System.Text;

namespace ABitNow.Models
{
   public class ExternalRegistrationConfirmation
    {
        public string Email { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ProviderKey { get; set; }

        public string LoginProvider { get; set; }
    }
}
