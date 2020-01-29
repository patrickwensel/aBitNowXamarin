using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ABitNowMobileApp.Data.Models;
using ABitNowMobileApp.Interfaces;
using ABitNowMobileApp.UiItemModels;
using Acr.UserDialogs;
using Xamarin.Forms;

namespace ABitNowMobileApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IPersistanceService _persistanceService;

        private string _username;
        private string _password;
        private bool _isRememberMe;
        private bool _hasAnyExternalLogin;
        private IList<SocialButton> _socialButtons;

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public bool IsRememberMe
        {
            get { return _isRememberMe; }
            set { SetProperty(ref _isRememberMe, value); }
        }

        public bool HasAnyExternalLogin
        {
            get { return _hasAnyExternalLogin; }
            set { SetProperty(ref _hasAnyExternalLogin, value); }
        }

        public IList<SocialButton> SocialButtons
        {
            get { return _socialButtons; }
            set { SetProperty(ref _socialButtons, value); }
        }

        public ICommand LoginCommand { get; set; }

        public ICommand LoginLaterCommand { get; set; }

        public ICommand SocialLoginCommand { get; set; }

        public ICommand RegisterNowCommand { get; set; }

        public LoginViewModel(INavigationService navigationService, IPersistanceService persistanceService)
        {
            _navigationService = navigationService;
            _persistanceService = persistanceService;
            LoginCommand = new Command(LoginAsync);
            LoginLaterCommand = new Command(LoginLaterAsync);
            SocialLoginCommand = new Command<ExternalLoginProvider>(SocialLoginAsync);
            RegisterNowCommand = new Command(NavigateOnRegisterScreen);
        }

        public async Task OnViewAppearAsync()
        {
            // Add API call here to fetch external login providers.
            UserDialogs.Instance.ShowLoading("Loading...");
            IList<ExternalLoginProvider> externalLoginProviders = new List<ExternalLoginProvider>()
            {
                new ExternalLoginProvider() { Name = "Facebook", Url = "https://test/api/user/ExternalLogin?provider=Facebook", State="" },
                new ExternalLoginProvider() { Name = "Google", Url = "https://test/api/user/ExternalLogin?provider=Google", State="" },
                new ExternalLoginProvider() { Name = "Twitter", Url = "https://test/api/user/ExternalLogin?provider=Twitter", State="" },
                new ExternalLoginProvider() { Name = "Microsoft", Url = "https://test/api/user/ExternalLogin?provider=Microsoft", State="" },
            };

            // Remove this line as well once API code added
            await Task.Delay(250);

            UserDialogs.Instance.HideLoading();
            SocialButtons = externalLoginProviders.Select(x => new SocialButton(x, SocialLoginCommand)).ToList();
            HasAnyExternalLogin = SocialButtons != null && SocialButtons.Count > 0;
        }

        private async void LoginAsync()
        {
            System.Diagnostics.Debug.WriteLine($"Login button tapped, Username : {Username}, Password : {Password}, IsRememberMe : {IsRememberMe}");
            _persistanceService.IsLoggedIn = true;
            await _navigationService.NavigateToFirstScreenAsync();
        }

        private async void LoginLaterAsync()
        {
            System.Diagnostics.Debug.WriteLine($"Login later button tapped");
            await _navigationService.NavigateToAsync(Enums.PageKey.Home);
        }

        private async void SocialLoginAsync(ExternalLoginProvider externalLoginProvider)
        {
            System.Diagnostics.Debug.WriteLine($"Login with {externalLoginProvider.Name} button tapped...");
        }

        private async void NavigateOnRegisterScreen()
        {
            await _navigationService.NavigateToAsync(Enums.PageKey.Signup);
        }
    }
}
