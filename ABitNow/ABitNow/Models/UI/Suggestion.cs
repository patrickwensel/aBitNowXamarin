using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ABitNow.Models.UI
{
    public class Suggestion
    {
        public string Content { get; set; }

        public ICommand TapCommand { get; set; }

        public ICommand RemoveCommand { get; set; }
    }
}
