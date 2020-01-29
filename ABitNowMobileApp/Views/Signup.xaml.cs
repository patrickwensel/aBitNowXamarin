using System;
using System.Collections.Generic;
using ABitNowMobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace ABitNowMobileApp.Views
{
    public partial class Signup : ContentPage
    {
        private readonly SignupViewModel _signupViewModel;

        public Signup(SignupViewModel signupViewModel)
        {
            InitializeComponent();
            _signupViewModel = signupViewModel;
            BindingContext = _signupViewModel;
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
            await _signupViewModel.OnViewAppearAsync();
        }
    }
}
