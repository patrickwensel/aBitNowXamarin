using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace ABitNow.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationView : ContentPage
    {
        public RegistrationView()
        {
            InitializeComponent();
        }

        protected override  void OnAppearing()
        {
            base.OnAppearing();

            if (Device.RuntimePlatform.Equals(Device.iOS))
            {
                var safeInsets = On<iOS>().SafeAreaInsets();

                FirstRow.Height = new GridLength(safeInsets.Top, GridUnitType.Absolute);
            }

            double topMargin = FirstRow.Height.Value + 120;
            FrmLoginContent.Margin = new Thickness(0, topMargin, 0, 0);
        }
    }
}