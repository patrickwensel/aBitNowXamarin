﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ABitNow.Models
{
    public class Restaurant
    {
        public string Image { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Address { get; set; }

        public double Rate { get; set; }

        public string FriendlyTime { get; set; }

        public int AwayInMins { get; set; }

        public IList<string> Tags { get; set; }

        public bool IsJit { get; set; }

        public ICommand TapCommand { get; set; }
    }
}
