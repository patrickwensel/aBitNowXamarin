using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ABitNow.Models.UI
{
    public class FilterTag 
    {
        public FilterTag()
        {

        }

        public string Id { get; set; }

        public string Tag { get; set; }

        public bool IsSelected { get; set; }

        public bool IsJit { get; set; }
        
        public string InfoContent { get; set; }
        
        public ICommand TapCommand { get; set; }
    }
}
