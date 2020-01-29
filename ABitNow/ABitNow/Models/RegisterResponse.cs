using System;
using System.Collections.Generic;
using System.Text;

namespace ABitNow.Models
{
    public class RegisterResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string Token { get; set; }
    }
}
