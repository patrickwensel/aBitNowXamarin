using ABitNow.Contracts.Services.General;
using ABitNow.Models;
using ABitNow.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;

namespace ABitNow.ViewModels
{
    public class ExternalLoginViewModel : ViewModelBase
    {
        public ExternalLoginViewModel(ISettingsService settingsService, IConnectionService connectionService, INavigationService navigationService, IDialogService dialogService) : base(connectionService, navigationService, dialogService, settingsService)
        {

        }

        private ExternalLoginProvider _selectedExternalLoginProvider;

        public ICommand WebviewNavigatedCommand => new Command<string>(OnWebviewNavigated);

        public ExternalLoginProvider SelectedExternalLoginProvider
        {
            get => _selectedExternalLoginProvider;
            set
            {
                _selectedExternalLoginProvider = value;
                OnPropertyChanged();
            }
        }

        private async void OnWebviewNavigated(string url)
        {
            IsBusy = false;
            if (!string.IsNullOrWhiteSpace(url))
            {
                string externalAuthenticate = "/Account/ExternalAuthenticate?";
                int externalAuthenticateIndex = url.IndexOf(externalAuthenticate, StringComparison.InvariantCultureIgnoreCase);

                if (externalAuthenticateIndex > -1)
                {
                    Uri myUri = new Uri(url);
                    string accessToken = HttpUtility.ParseQueryString(myUri.Query).Get("access_token");
                    string loginProvider = HttpUtility.ParseQueryString(myUri.Query).Get("LoginProvider");

                    if (!string.IsNullOrWhiteSpace(accessToken))
                    {
                        _settingsService.AccessTokenSetting = accessToken;
                        await _navigationService.NavigateToAsync<MainViewModel>();
                    }
                    else if (string.IsNullOrWhiteSpace(accessToken) && !string.IsNullOrWhiteSpace(loginProvider))
                    {
                        string email = HttpUtility.ParseQueryString(myUri.Query).Get("email");
                        string providerDisplayName = HttpUtility.ParseQueryString(myUri.Query).Get("ProviderDisplayName");
                        string providerKey = HttpUtility.ParseQueryString(myUri.Query).Get("ProviderKey");

                        if (!string.IsNullOrWhiteSpace(providerDisplayName) && !string.IsNullOrWhiteSpace(providerKey))
                        {
                            ExternalRegistrationConfirmation externalRegistrationConfirmation = new ExternalRegistrationConfirmation();
                            externalRegistrationConfirmation.Email = email;
                            externalRegistrationConfirmation.LoginProvider = loginProvider;
                            externalRegistrationConfirmation.ProviderDisplayName = providerDisplayName;
                            externalRegistrationConfirmation.ProviderKey = providerKey;

                            await _navigationService.NavigateToAsync<ExternalRegistrationViewModel>(externalRegistrationConfirmation);
                        }
                        else
                        {
                            await _navigationService.NavigateToAsync<LoginViewModel>();
                        }
                    }
                }
            }
        }

        public override async Task InitializeAsync(object data)
        {
            IsBusy = true;
            DependencyService.Get<IWebviewClearCookies>().ClearAllCookies();
            SelectedExternalLoginProvider = (ExternalLoginProvider)data;
            Title = SelectedExternalLoginProvider.Name;
        }
    }
}
