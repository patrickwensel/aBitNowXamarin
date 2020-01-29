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
    public class SignupViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        private string _name;
        private string _lastName;
        private string _mobileNumber;
        private string _email;
        private string _password;
        private string _confirmPassword;

        private bool _hasAnyExternalLogin;

        private IList<SocialButton> _socialButtons;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        public string MobileNumber
        {
            get { return _mobileNumber; }
            set { SetProperty(ref _mobileNumber, value); }
        }

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { SetProperty(ref _confirmPassword, value); }
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

        public ICommand SocialLoginCommand { get; set; }

        public ICommand RegisterCommand { get; set; }

        public ICommand RegisterLaterCommand { get; set; }

        public SignupViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            LoginCommand = new Command(NavigateBackToLogin);
            SocialLoginCommand = new Command<ExternalLoginProvider>(SocialLoginAsync);
            RegisterCommand = new Command(RegisterAsync);
            RegisterLaterCommand = new Command(RegisterLaterAsync);
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

        private async void NavigateBackToLogin()
        {
            await _navigationService.NavigateToAsync(Enums.PageKey.Login);
        }

        private async void SocialLoginAsync(ExternalLoginProvider externalLoginProvider)
        {
            System.Diagnostics.Debug.WriteLine($"Login with {externalLoginProvider.Name} button tapped...");
        }

        private async void RegisterAsync()
        {
            System.Diagnostics.Debug.WriteLine($"Register Name : {Name}, LastName : {LastName}, MobileNumber : {MobileNumber}, Email : {Email}, Password : {Password}, ConfirmPassword : {ConfirmPassword} ");
        }

        private async void RegisterLaterAsync()
        {
            System.Diagnostics.Debug.WriteLine($"Register later tapped ");
            await _navigationService.NavigateToAsync(Enums.PageKey.Home);
        }
    }
}
