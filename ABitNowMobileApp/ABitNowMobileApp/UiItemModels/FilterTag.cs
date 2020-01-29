using System;
using System.Windows.Input;

namespace ABitNowMobileApp.UiItemModels
{
    public class FilterTag : BaseModel
    {
        private string _tag;
        private bool _isSelected;
        private bool _isJit;
        private string _infoContent;

        public string Id { get; set; }

        public string Tag
        {
            get { return _tag; }
            set { SetProperty(ref _tag, value); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public bool IsJit
        {
            get { return _isJit; }
            set { SetProperty(ref _isJit, value); }
        }

        public string InfoContent
        {
            get { return _infoContent; }
            set { SetProperty(ref _infoContent, value); }
        }

        public ICommand TapCommand { get; set; }
    }
}
