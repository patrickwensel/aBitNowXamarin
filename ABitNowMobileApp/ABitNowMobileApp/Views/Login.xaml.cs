using ABitNowMobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace ABitNowMobileApp.Views
{
    public partial class Login : ContentPage
    {
        private readonly LoginViewModel _loginViewModel;

        public Login(LoginViewModel loginViewModel)
        {
            InitializeComponent();
            _loginViewModel = loginViewModel;
            BindingContext = _loginViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (Device.RuntimePlatform.Equals(Device.iOS))
            {
                var safeInsets = On<iOS>().SafeAreaInsets();

                FirstRow.Height = new GridLength(safeInsets.Top, GridUnitType.Absolute);
            }

            double topMargin = FirstRow.Height.Value + 120;
            FrmLoginContent.Margin = new Thickness(0, topMargin, 0, 0);
            await _loginViewModel.OnViewAppearAsync();
        }
    }
}
