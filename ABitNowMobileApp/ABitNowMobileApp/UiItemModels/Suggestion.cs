using System;
using System.Windows.Input;

namespace ABitNowMobileApp.UiItemModels
{
    public class Suggestion
    {
        public string Content { get; set; }

        public ICommand TapCommand { get; set; }

        public ICommand RemoveCommand { get; set; }
    }
}
