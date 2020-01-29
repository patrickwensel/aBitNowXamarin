using System;
using System.Collections.Generic;
using System.Text;

namespace ABitNow.Models
{
    public class RegisterRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

    }
}
